using Ardalis.GuardClauses;
using Arquitecture.Handlers;
using Models.DTOS;
using NTW_TEST_PATTERNS.Models.DTOS;
using Service.Services;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.Login;

public class EmailorUsernameLogin : ILoginStrategy
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IRoleService _roleService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtHandler _jwtHandler;
    private List<string> _roles;

    public EmailorUsernameLogin(IUserService userService, IUserRoleService userRoleService, IRoleService roleService ,IPasswordHashingService passwordHashingService, IJwtHandler jwtHandler)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _roleService = roleService;
        _passwordHashingService = passwordHashingService;
        _jwtHandler = jwtHandler;
        _roles = new List<string>();
    }
    public async Task<string> LoginUser(UserLoginRequest userLoginRequest)
    {
        //first we need to validate the user
        if (!ValidateUser(userLoginRequest))
        {
            throw new Exception("Invalid user");
        }
        //now we need to get the user
        var user = await _userService.GetUserByEmailAsync(userLoginRequest.EmailUsername);
        if (user == null)
        {
            user = await _userService.GetUserByUsernameAsync(userLoginRequest.EmailUsername);
            if (user == null) throw new InvalidOperationException("User not found");
        }

        //now we need to verify the status of the user
        if (user.Status != "Active") throw new InvalidOperationException("User is not active");

        //now we need to verify the password
        if (!_passwordHashingService.VerifyPassword(userLoginRequest.Password, user.PasswordHash)) throw new InvalidOperationException("Invalid password");

        //now we need to get the roles
        var userRoles = await _userRoleService.GetUserRoles() ?? throw new InvalidOperationException("User roles not found");

        //now we need to get the roles of the user
        foreach (var userRole in userRoles)
        {
            if (userRole.UserId == user.UserId)
            {
                var role = await _roleService.GetRoleByIdAsync(userRole.RoleId);
                if (role == null) throw new InvalidOperationException("Role not found");
                _roles.Add(role.RoleName);
            }
        }
        //now we need to generate the token
        var token = _jwtHandler.GenerateToken(user.UserId.ToString(), user.Username ,_roles);

        return token;
    }

    public Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest)
    {
        throw new NotImplementedException();
    }

    public bool ValidateUser(UserLoginRequest userLoginRequest)
    {
        try
        {
          Guard.Against.Null(userLoginRequest, nameof(userLoginRequest));
          Guard.Against.NullOrEmpty(userLoginRequest.EmailUsername, nameof(userLoginRequest.EmailUsername));
          Guard.Against.NullOrEmpty(userLoginRequest.Password, nameof(userLoginRequest.Password));
        }
        catch (Exception ex)
        {
            throw new Exception("Error validating user", ex);
        }
        return true;
    }
}
