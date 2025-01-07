using Arquitecture.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("verify-email")]
    [ApiController]
    public class VerifyAuthenticationController : ControllerBase
    {

        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;
        public VerifyAuthenticationController(IJwtHandler jwtHandler, IUserService userService)
        {
            _jwtHandler = jwtHandler;
            _userService = userService;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            //first of all we verify the incoming token
            var tokenIsValid = _jwtHandler.GetPrincipalFromToken(token);
            if (tokenIsValid == null)
            {
                return BadRequest("Invalid token");
            }
            //if the token is valid means that the email is verified
            //now were goin to extract the user id from the token
            foreach (var claim in tokenIsValid.Claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

            var userIdClaim = tokenIsValid.FindFirst(ClaimTypes.NameIdentifier); 
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID in token");
            }

            //now we are goin to get the user and update his status to Active
            var user = await  _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

             user.Status = "Active";

            var updatedUser = await _userService.SaveUserAsync(user);

            if (updatedUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user status");
            }
            //now we redirect to the verified web page and pass the token as a query parameter
            return Redirect($"http://localhost:5173/signUp/EmailVerify?token={token}");
        }

    }
}
