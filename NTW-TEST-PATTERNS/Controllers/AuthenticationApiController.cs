using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOS;
using Strategies.Auth;
using Newtonsoft.Json;
using Strategies.Authentication;
using Arquitecture.Handlers;
using Service.Services;
using Models;
using Strategies.EmailSenderStrategy;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationApiController : ControllerBase
    {
        private readonly IAuthenticationStrategyContext _authenticationStrategyContext;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRoleService _roleService;
        private readonly ISendEmailStrategyContext _sendEmailStrategyContext;
        private ILogger _logger;
        public AuthenticationApiController(IAuthenticationStrategyContext authenticationStrategyContext, IJwtHandler jwtHandler , IRoleService roleService, ISendEmailStrategyContext sendEmailStrategyContext)
        {
            _jwtHandler = jwtHandler;
            _authenticationStrategyContext = authenticationStrategyContext;
            _roleService = roleService;
            _sendEmailStrategyContext = sendEmailStrategyContext;
        }

        [HttpPost("registerByEmail")]
        public async Task<IActionResult> RegisterByEmail([FromBody] UserRegistrationData userRegistrationData)
        {
            try
            {
                var result = await _authenticationStrategyContext.RegisterUser(userRegistrationData);
                var userId = result.UserId.ToString();
                var roleNames = new List<string>();

                foreach (var userRole in result.UserRoles)
                {
                    var role = await _roleService.GetRoleByIdAsync(userRole.RoleId);
                    roleNames.Add(role.RoleName);
                }

                var token = _jwtHandler.GenerateToken(userId, roleNames, null);
                var email = userRegistrationData.Email;
                var subject = "Verification Token";
                var host = $"{Request.Scheme}://{Request.Host}";
                var message = $"Please click on the link below to verify your email address: \n\n{host}/verify-email/{token}";

                await _sendEmailStrategyContext.SendEmailAsync(email, subject, message, token);

                return Ok(new { message = "User registered successfully, check your email for the verification token" });
            }
            catch (Exception ex)
            {
                // Devolver el error en formato JSON
                _logger.LogError(ex, "NTWException al registrar usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }


    }
}
