using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOS;
using Strategies.Authentication;
using Newtonsoft.Json;
using Arquitecture.Handlers;
using Service.Services;
using Models;
using Strategies.EmailSenderStrategy;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.Data;
using Service.Strategies.Login;
using NTW_TEST_PATTERNS.Models.DTOS;
using Services;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationApiController : ControllerBase
    {
        private readonly IAuthenticationStrategyContext _authenticationStrategyContext;
        private readonly ILoginStrategyContext _loginStrategyContext;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService  _userService;
        private readonly ISendEmailStrategyContext _sendEmailStrategyContext;
        private readonly ILogger<AuthenticationApiController> _logger;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public AuthenticationApiController(
            IAuthenticationStrategyContext authenticationStrategyContext,
            ILoginStrategyContext loginStrategyContext,
            IJwtHandler jwtHandler,
            IRoleService roleService,
            ISendEmailStrategyContext sendEmailStrategyContext,
            IUserService userService,
            IPasswordHashingService passwordHashingService,
            IHttpClientFactory httpClientFactory,
            IUserRoleService userRoleService,
            IConfiguration configuration,
            ILogger<AuthenticationApiController> logger)
        {
            _jwtHandler = jwtHandler;
            _authenticationStrategyContext = authenticationStrategyContext;
            _loginStrategyContext = loginStrategyContext;
            _roleService = roleService;
            _userService = userService;
            _passwordHashingService = passwordHashingService;
            _userRoleService = userRoleService;
            _sendEmailStrategyContext = sendEmailStrategyContext;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("registerByEmail")]
        public async Task<IActionResult> RegisterByEmail([FromBody] UserRegistrationData userRegistrationData)
        {
            try
            {
                var result = await _authenticationStrategyContext.RegisterUser(userRegistrationData);
                var userId = result.UserId.ToString();
                var username = result.Username;
                var roleNames = new List<string>();

                foreach (var userRole in result.UserRoles)
                {
                    var role = await _roleService.GetRoleByIdAsync(userRole.RoleId);
                    roleNames.Add(role.RoleName);
                }

                var token = _jwtHandler.GenerateToken(userId, username ,roleNames, null);
                var email = userRegistrationData.Email;
                var subject = "Verification Token";
                var host = $"{Request.Scheme}://{Request.Host}";
                var message = $"Please click on the link below to verify your email address: \n\n{host}/verify-email/{token}";

                await _sendEmailStrategyContext.SendEmailAsync(email, subject, message, token);

                return Ok(new { message = "User registered successfully, check your email for the verification token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user");

                var errorDetails = new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };

                return StatusCode(500, errorDetails);
            }
        }

        [HttpPost("loginByEmail")]
        public async Task<IActionResult> LoginByEmailOrUsername([FromBody] UserLoginRequest userLoginRequest)
        {
            try
            {
                var result =  await _loginStrategyContext.LoginUserBYEmailOrUsername(userLoginRequest);
                return Ok(new { jwt = result });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user");
                var errorDetails = new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };
                return StatusCode(500, errorDetails);
            }
        }

        [HttpPost("OAuth")]
        public async Task<IActionResult> LoginByGoogle([FromBody] OAuthRequest  oAuthRequest)
        {
            try
            {
                switch (oAuthRequest.Provider)
                {
                    case "Google":
                         _loginStrategyContext.setLoginStrategy(new GoogleLogin(_userService,_passwordHashingService,_jwtHandler, _userRoleService, _configuration ,_httpClientFactory  ,_roleService ));
                        var result = await _loginStrategyContext.LoginUserWithOAuth(oAuthRequest);
                        return Ok(new { jwt = result });
                    case "Github":
                        _loginStrategyContext.setLoginStrategy(new GithubLogin(_userService, _passwordHashingService, _jwtHandler, _userRoleService, _configuration, _httpClientFactory, _roleService));
                        var result2 = await _loginStrategyContext.LoginUserWithOAuth(oAuthRequest);
                        return Ok(new { jwt = result2 });
                    default:
                        return BadRequest(new { message = "Invalid OAuth provider" });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user with OAuth");
                var errorDetails = new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };
                return StatusCode(500, errorDetails);
            }
        }
    }
}
