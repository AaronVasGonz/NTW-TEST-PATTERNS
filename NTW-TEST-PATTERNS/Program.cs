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


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .AllowAnyMethod());
});

EnvConfig.Initialize();

//now load the key from the environment variables\
var jtwKey = Environment.GetEnvironmentVariable("JWT_KEY");

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddScoped<IAssignRolesStrategy, DefaultAssignRolesStrategy >();
builder.Services.AddScoped<IValidateRolesStrategy,DefaultValidationRolesStrategy>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IRegistrationStrategy, EmailRegisterAuthentication>();
builder.Services.AddScoped<IAuthenticationStrategyContext, AuthenticationStrategyContext>();
builder.Services.AddScoped<IJwtHandler>(provider => new JwtHandler(jtwKey, 60,"sub" , ClaimTypes.Role));
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailSenderStrategy, SendDefaultEmailStrategy>();
builder.Services.AddScoped<ISendEmailStrategyContext, SendEmailStrategyContext>();
builder.Services.AddScoped<ILoginStrategy, EmailorUsernameLogin>();
builder.Services.AddScoped<ILoginStrategy, GoogleLogin>();
builder.Services.AddScoped<ILoginStrategy, GithubLogin>();
builder.Services.AddScoped<ILoginStrategyContext, LoginStrategyContext>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jtwKey));
    var  signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature);

    opt.RequireHttpsMetadata = false;

    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signinKey,
    };

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

app.Run();
