using Microsoft.Extensions.Logging;
using Models.DTOS;
using Service.Mappers;
using Service.Services;
using Services;
using Strategies.Auth;
using System;
using System.Threading.Tasks;

using Strategies.Auth;
using Models;

public interface IAuthenticationStrategyContext
    {
        Task<User> RegisterUser(UserRegistrationData userRegistrationData);
        void SetRegistrationStrategy(IRegistrationStrategy registrationStrategy);
    }

    /// <summary>
    /// Context class for handling different registration strategies.
    /// </summary>
    public class AuthenticationStrategyContext : IAuthenticationStrategyContext
    {
        private IRegistrationStrategy _registrationStrategy;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IUserMapper _userMapper;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationStrategyContext"/> class.
        /// </summary>
        /// <param name="registrationStrategy">The initial registration strategy.</param>
        public AuthenticationStrategyContext(IRegistrationStrategy registrationStrategy, IUserRoleService userRoleService ,IUserService userService, IPasswordHashingService passwordHashingService, IUserMapper userMapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _passwordHashingService = passwordHashingService ?? throw new ArgumentNullException(nameof(passwordHashingService));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
            _registrationStrategy = registrationStrategy ?? new EmailRegisterAuthentication(_userService, _userRoleService,_passwordHashingService, _userMapper);

        }

        /// <summary>
        /// Sets a new registration strategy.
        /// </summary>
        /// <param name="registrationStrategy">The new registration strategy.</param>
        public void SetRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        {
            _registrationStrategy = registrationStrategy ?? throw new ArgumentNullException(nameof(registrationStrategy));
        }

        /// <summary>
        /// Registers a user using the current registration strategy.
        /// </summary>
        /// <param name="userRegistrationData">The user registration data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
        public Task<User> RegisterUser(UserRegistrationData userRegistrationData)
        {
            if (_registrationStrategy == null)
            {
                throw new InvalidOperationException("Registration strategy is not set.");
            }

            return _registrationStrategy.RegisterUser(userRegistrationData);
        }
    }
