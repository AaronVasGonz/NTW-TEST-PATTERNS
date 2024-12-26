using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService  _userService;
        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = "Get Users")]
        public async Task<IActionResult> GetUsers() {
            try
            {

               var users = await _userService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }}
        }
   
    }

