
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOS;
using Ardalis.GuardClauses;
using Arquitecture.Validations.Guard;
using Service.Services;
using Services;
using Models;
using Service.Mappers;
using Microsoft.Extensions.Logging;


namespace Strategies.Auth;

public class EmailRegisterAuthentication : IRegistrationStrategy
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserMapper _userMapper;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly List<int> _idRoles;
       
        private readonly ILogger _logger;
        public EmailRegisterAuthentication(IUserService userService, IUserRoleService userRoleService ,IPasswordHashingService passwordHashingService, IUserMapper userMapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
            _passwordHashingService = passwordHashingService ?? throw new ArgumentNullException(nameof(passwordHashingService));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _idRoles = new List<int> { 1 };
        }

    public async Task<User> RegisterUser(UserRegistrationData userRegistrationData)
    {
        if (!ValidateUser(userRegistrationData))
        {
            throw new ArgumentException("Invalid User Registration Data");
        }

        userRegistrationData.Password = _passwordHashingService.HashPassword(userRegistrationData.Password);
        User user = _userMapper.MapUserFromUserRegistrationData(userRegistrationData);
        user.Status = "Inactive";
        var savedUser = await _userService.SaveUserAsync(user);
        var savedUserRole = await _userRoleService.SaveUserRole(savedUser.UserId, _idRoles[0]);
      
        if (savedUser != null && savedUserRole != null)
        {
            savedUser.UserRoles.Add(savedUserRole);
            return savedUser;
        }
        else
        {
            throw new InvalidOperationException("Failed to save user or user role.");
        }
    }

    public Task<User> RegisterUserWithOAuth(string oauthToken)
    {
        throw new NotImplementedException();
    }

    public bool ValidateUser(UserRegistrationData userRegistrationData)
        {
            try
            {
                Guard.Against.Null(userRegistrationData, nameof(userRegistrationData));
                Guard.Against.NullOrEmpty(userRegistrationData.Email, nameof(userRegistrationData.Email));
                Guard.Against.InvalidEmail(userRegistrationData.Email, nameof(userRegistrationData.Email));
                Guard.Against.NullOrEmpty(userRegistrationData.UserName, nameof(userRegistrationData.UserName));
                Guard.Against.NullOrEmpty(userRegistrationData.Password, nameof(userRegistrationData.Password));
                Guard.Against.InvalidPassword(userRegistrationData.Password, nameof(userRegistrationData.Password));
            }
            catch (ArgumentException ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "Invalid User Registration Data");
                return false;
            }
            return true;
        }

}
