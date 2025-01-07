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

public interface ILoginStrategyContext
{
    Task<string> LoginUserBYEmailOrUsername(UserLoginRequest userLoginRequest);
    void setLoginStrategy(ILoginStrategy loginStrategy);

    Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest);
}

public class LoginStrategyContext : ILoginStrategyContext
{
    private ILoginStrategy _loginStrategy;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IRoleService _roleService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtHandler _jwtHandler;

    public LoginStrategyContext(ILoginStrategy loginStrategy, IUserService userService, IUserRoleService userRoleService, IRoleService roleService, IPasswordHashingService passwordHashingService, IJwtHandler jwtHandler)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _roleService = roleService;
        _passwordHashingService = passwordHashingService;
        _jwtHandler = jwtHandler;
        _loginStrategy = loginStrategy ?? new EmailorUsernameLogin(userService, userRoleService, roleService, passwordHashingService, jwtHandler);
    }

    public void setLoginStrategy(ILoginStrategy loginStrategy)
    {
        _loginStrategy = loginStrategy ?? throw new ArgumentNullException(nameof(loginStrategy));
    }

    public async Task<string> LoginUserBYEmailOrUsername(UserLoginRequest userLoginRequest)
    {
        if (_loginStrategy == null)
        {
            throw new ArgumentNullException(nameof(_loginStrategy));
        }
        return await _loginStrategy.LoginUser(userLoginRequest);
    }

    public async Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest)
    {
        if (_loginStrategy == null)
        {
            throw new ArgumentNullException(nameof(_loginStrategy));
        }
        return await _loginStrategy.LoginUserWithOAuth(oAuthRequest);
    }
}
