using Arquitecture.Handlers;
using Azure.Core;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOS;
using Newtonsoft.Json;
using NTW_TEST_PATTERNS.Models.DTOS;
using Service.Services;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.Login;

/// <summary>
/// Strategy for handling Google OAuth login.
/// Implements the ILoginStrategy interface.
/// </summary>
public class GoogleLogin : ILoginStrategy
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IRoleService _roleService;
    private readonly IPasswordHashingService _passwordHasher;
    private readonly IJwtHandler _jwtHandler;
    private List<string> _roles;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for initializing dependencies.
    /// </summary>
    /// <param name="userService">Service for user-related operations.</param>
    /// <param name="passwordHasher">Service for password hashing.</param>
    /// <param name="jwtHandler">Handler for JWT token generation.</param>
    /// <param name="userRoleService">Service for user role-related operations.</param>
    /// <param name="roleService">Service for role-related operations.</param>
    public GoogleLogin(
        IUserService userService,
        IPasswordHashingService passwordHasher,
        IJwtHandler jwtHandler,
        IUserRoleService userRoleService,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IRoleService roleService)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _jwtHandler = jwtHandler;
        _userRoleService = userRoleService;
        _roleService = roleService;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
        _roles = new List<string>();
       
    }

    /// <summary>
    /// Handles user login with Google OAuth.
    /// </summary>
    /// <param name="oAuthRequest">The OAuth request containing the Google token.</param>
    /// <returns>A JWT token as a string.</returns>
    /// <exception cref="ApplicationException">Thrown if any error occurs during login.</exception>
    public async Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest)
    {
        try
        {
            //first we need to verify the google token
            var googleUserInfo = await VerifyGoogleTokenAsync(oAuthRequest.Token);
            if (googleUserInfo == null) throw new InvalidOperationException("Invalid Google token");

            var email = oAuthRequest.Email;
            var name = oAuthRequest.Name;

            //vef if emails is different from the google email
            if (email != googleUserInfo.Email) throw new InvalidOperationException("Invalid email");

            var user = await _userService.GetUserByEmailAsync(email);


            // Generate a secure password for OAuth users
            var password = _passwordHasher.HashPassword($"GoogleLogin*{email}_{name}_{Guid.NewGuid()}");
            //Generate a username for OAuth users
            var emailPrefix = email.Split('@')[0];
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = emailPrefix,
                    Status = "Active",
                    PasswordHash = password,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                user = await _userService.SaveUserAsync(user);

                // Add default role for new OAuth users
                await _userRoleService.SaveUserRole(user.UserId, 1);
            }

            var userRoles = await _userRoleService.GetUserRoles() ??
                            throw new InvalidOperationException("User roles not found");

            foreach (var userRole in userRoles)
            {
                if (userRole.UserId == user.UserId)
                {
                    var role = await _roleService.GetRoleByIdAsync(userRole.RoleId) ??
                               throw new InvalidOperationException("Role not found");

                    _roles.Add(role.RoleName);
                }
            }
            var jwtToken = _jwtHandler.GenerateToken(user.UserId.ToString(), user.Username ,_roles, null);
            return jwtToken;
        }
        catch (Exception ex)
        {
            // Log the specific exception
            throw new ApplicationException($"An error occurred during login: {ex.Message}");
        }
    }

    public async Task<GoogleUserInfo> VerifyGoogleTokenAsync(string accessToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            // Verify the Google token
            var response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Invalid Google token");
            }
            var content = await response.Content.ReadAsStringAsync();

            var googleUserInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(content);

            return googleUserInfo;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred during login: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates the user credentials.
    /// </summary>
    /// <param name="userLoginRequest">The login request containing user credentials.</param>
    /// <returns>True if the user is valid, otherwise false.</returns>
    public bool ValidateUser(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Handles traditional user login (not implemented).
    /// </summary>
    /// <param name="userLoginRequest">The login request containing user credentials.</param>
    /// <returns>A JWT token as a string.</returns>
    public Task<string> LoginUser(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }
}
