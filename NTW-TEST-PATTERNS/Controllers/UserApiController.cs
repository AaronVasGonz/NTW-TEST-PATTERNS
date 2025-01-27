using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Models.EFModels;
using Service.Services;
using Service.Services.validations.Users;
using Services;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IValidateUserRequestService _validateUserRequestService;
        public UserApiController(
            IUserService userService,
            IValidateUserRequestService validateUserRequestService,
            IRoleService roleService,
            IUserRoleService userRoleService
            )
        {
            _userService = userService;
            _validateUserRequestService = validateUserRequestService;
            _userRoleService = userRoleService;
            _roleService = roleService;
            _userRoleService = userRoleService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if (users == null)
                throw new KeyNotFoundException("Users not found");

            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user id");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return Ok(user);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromBody] UserRequest userRequest)
        {
            if (userRequest == null)
                throw new ArgumentNullException("User is null");


            //validate the user request
            _validateUserRequestService.ValidateUserRequest(userRequest);

            var roles = new List<Role>();
            foreach (var role in userRequest.Roles)
            {
                var savedRole = await _roleService.GetRoleByNameAsync(role);
                if (savedRole == null)
                    throw new KeyNotFoundException("Role not found");
                roles.Add(savedRole);
            }

            var user = new User
            {
                Username = userRequest.Username,
                PasswordHash = userRequest.Password,
                Email = userRequest.Email,
                Status = userRequest.Status
            };
            var savedUser = await _userService.SaveUserAsync(user);
            if (savedUser == null)
                throw new Exception("Failed to save user");

            foreach (var role in roles)
            {
                var savedUserRoles = await _userRoleService.SaveUserRole(savedUser.UserId ?? 0, role.RoleId ?? 0);
                if (savedUserRoles == null)
                    throw new Exception("Failed to save user roles");
            }

            return Ok(new { message = "User saved successfully" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            if (userRequest == null)
                throw new ArgumentNullException("User is null");
            //validate the user request
            _validateUserRequestService.ValidateUserRequest(userRequest);

            var user = await _userService.GetUserByIdAsync(userRequest.Id ?? 0);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.Username = userRequest.Username;

            user.PasswordHash = userRequest.Password;

            user.Email = userRequest.Email;

            user.Status = userRequest.Status;

            var updatedUser = await _userService.SaveUserAsync(user);

            if (updatedUser == null)
                throw new Exception("Failed to update user");

            return Ok(new { message = "User updated successfully" });
        }
    }

}

