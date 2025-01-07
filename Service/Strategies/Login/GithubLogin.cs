using Arquitecture.Handlers;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Models.DTOS;
using Newtonsoft.Json;
using Service.Services;
using Services;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EFModels;
using NTW_TEST_PATTERNS.Models.DTOS;
using System.Net.Http.Headers;

namespace Service.Strategies.Login;
public class GithubLogin : ILoginStrategy
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
    public GithubLogin(
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

    public Task<string> LoginUser(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }

      public async Task<string> LoginUserWithOAuth(OAuthRequest oAuthRequest)
    {
        try
        {
            // First, we try to get the access token
            var accessToken = await GetGithubAccessTokenAsync(oAuthRequest.Code);

            // Then, we get the user info
            var githubUserInfo = await GetGithubUserInfoAsync(accessToken);

            // We create a random password
            var password = _passwordHasher.HashPassword($"GitHubLogin*{githubUserInfo.Name}_{Guid.NewGuid()}");

            // Check if user already exists in the database
            var user = await _userService.GetUserByEmailAsync(githubUserInfo.Email);

            if (user == null)
            {
                // If the user does not exist, create a new one
                user = new User
                {
                    Email = githubUserInfo.Email,
                    Username = githubUserInfo.Login,
                    Status = "Active",
                    PasswordHash = password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                user = await _userService.SaveUserAsync(user);

                // Assign the user the default role
                await _userRoleService.SaveUserRole(user.UserId, 1);
            }

            // Get the roles for the user
            var userRoles = await _userRoleService.GetUserRoles() ??
                            throw new InvalidOperationException("User roles not found");

            // Loop through user roles and assign them to the roles list
            foreach (var userRole in userRoles)
            {
                if (userRole.UserId == user.UserId)
                {
                    var role = await _roleService.GetRoleByIdAsync(userRole.RoleId) ??
                               throw new InvalidOperationException("Role not found");

                    _roles.Add(role.RoleName);
                }
            }

            var token = _jwtHandler.GenerateToken(user.UserId.ToString(), user.Username ,_roles, null);

            return token;
        }
        catch (Exception ex)
        {
            throw new Exception("Error while trying to login with GitHub", ex);
        }
    }

    public async Task<GithubUserInfo> GetGithubUserInfoAsync(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new Exception("Access token is null or empty.");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        request.Headers.Add("User-Agent", "NTW-TEST-PATTERNS"); // GitHub requiere un User-Agent

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            // Leer y deserializar el contenido de la respuesta
            string content = await response.Content.ReadAsStringAsync();
            var userData = JsonConvert.DeserializeObject<GithubUserInfo>(content);
            userData.Email = userData.Login + "@github.com";
            return userData;
        }
        else
        {
            throw new Exception("GitHub authentication failed. Status code: " + response.StatusCode);
        }
    }
    public async Task<string> GetGithubAccessTokenAsync(string code)
    {
        var clientId = "Ov23li5lIOZ76y1Itrtd";
        var clientSecret = "fc477f8ea94c8483d0c0fc66891650ce74b14789";

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("GitHub client credentials are not properly configured.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
        {
            Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("code", code)
        })
        };
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        // Use SendAsync instead of PostAsync
        using var tokenResponse = await _httpClient.SendAsync(request);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            var errorContent = await tokenResponse.Content.ReadAsStringAsync();
            throw new Exception($"Cannot get the access token from GitHub. Status: {tokenResponse.StatusCode}, Response: {errorContent}");
        }

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenContent);
        var accessToken = tokenData?.GetValueOrDefault("access_token");

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new Exception("Access token not received from GitHub");
        }

        return accessToken;
    }
    public bool ValidateUser(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }
}
