using Data.Repository;
using Data.Repository.Data.Repository;
using Microsoft.IdentityModel.Tokens;
using Strategies.userRoles;
using Service.Services;
using Services;
using System.Text;
using Service.Mappers;
using Strategies.Authentication;
using Arquitecture;
using Arquitecture.Handlers;
using Strategies.EmailSenderStrategy;
using Service.Strategies.EmailSenderStrategy;
using Architecture;
using System.Security.Claims;
using Service.Strategies.Login;
using Service.Services.validations.Products;
using Service.Strategies.ImageUploader;
using Service.Filters;
using Service.Services.validations;
using Service.Services.validations.Suppliers;
using Service.Services.validations.Categories;
using Service.Services.validations.Roles;
using Service.Services.validations.Customers;
using Service.Services.validations.Employees;
using Service.Services.validations.Users;
using NTW_TEST_PATTERNSV.FAQBot.FAQBot;
using Microsoft.Bot.Builder;
using FAQBot.Bot;
using Service.Services.IA;
using OllamaSharp;


var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173", "http://localhost:8080")
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .AllowAnyMethod());
});

EnvConfig.Initialize();

/*
 * This are environment variables will be stay in the final result cloud service, 
 * just for now  i'm gonna handle in a hardcode way.
 * They are just demostrative and in the final result all of theese will goin to be changed
 */
var jtwKey = "885d8ee4a3c69838a0ca8474a10c0365213b31a3c6fa9222ae9e92cb63927c601a16c8e78213679dd9c4bbe5e0cc4704166366e1eea0aa82234142e89483d70e";
var firebaseBucketStorage = "spmccr-d02b1.appspot.com";
var ollamaApiUri = new Uri("http://localhost:11434");

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<IValidationListofRolesContext, ValidationListofRolesContext>();
builder.Services.AddScoped<IRoleAssigmentContext, RoleAssigmentContext>();
builder.Services.AddScoped<IAssignRolesStrategy, DefaultAssignRolesStrategy>();
builder.Services.AddScoped<IValidateRolesStrategy, DefaultValidationRolesStrategy>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IRegistrationStrategy, EmailRegisterAuthentication>();
builder.Services.AddScoped<IAuthenticationStrategyContext, AuthenticationStrategyContext>();
builder.Services.AddScoped<IJwtHandler>(provider => new JwtHandler(jtwKey, 60, "sub", ClaimTypes.Role));
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailSenderStrategy, SendDefaultEmailStrategy>();
builder.Services.AddScoped<ISendEmailStrategyContext, SendEmailStrategyContext>();
builder.Services.AddScoped<ILoginStrategy, EmailorUsernameLogin>();
builder.Services.AddScoped<IValidateProductService, ValidateProductService>();
builder.Services.AddScoped<IGeneralValidationFunctions, GeneralValidationFunctions>();
builder.Services.AddScoped<IValidateSuppliersService, ValidateSuppliersService>();
builder.Services.AddScoped<IValidateCategoryService, ValidateCategoryService>();
builder.Services.AddScoped<IValidateRoleService, ValidateRoleService>();
builder.Services.AddScoped<IValidateCustomerService, ValidateCustomerService>();
builder.Services.AddScoped<IValidateEmployeeService, ValidateEmployeeService>();
builder.Services.AddScoped<IValidateUserRequestService, ValidateUserRequestService>();
builder.Services.AddScoped<ILoginStrategy, GoogleLogin>();
builder.Services.AddScoped<ILoginStrategy, GithubLogin>();
builder.Services.AddScoped<ILoginStrategyContext, LoginStrategyContext>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IImageUploaderContext, ImageUploaderContext>();
builder.Services.AddScoped<IUploadImageStrategy, UploadImageWithFirebase>();
builder.Services.AddScoped<IImageConverterService, ImageConverterService>();
builder.Services.AddScoped<IExeptionLogRepository, ExeptionLogRepository>();
builder.Services.AddScoped<IExceptionLogService, ExceptionLogService>();
builder.Services.AddSingleton<IFAQService, FAQService>();
builder.Services.AddSingleton<IBot, FAQBotService>();
builder.Services.AddScoped<IOllamaService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<OllamaService>>();
    return new OllamaService(ollamaApiUri, logger);
});
builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddScoped<IFirebaseStorageService>(provider =>
{
    return new FirebaseStorageService(firebaseBucketStorage);
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jtwKey));
    var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature);

    opt.RequireHttpsMetadata = false;

    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signinKey,
    };

});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Hello, world!");
    return "Hello, world!";
});

app.Run();
