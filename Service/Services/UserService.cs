using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Strategies.userRoles;
using Data.Repository;
namespace Services
{
    public interface IUserService
    {
        Task<bool> DeleteUserAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> SaveUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
    }

    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IRoleAssigmentContext _roleAssigmentContext;

        private readonly IValidationListofRolesContext _validationListofRolesContext;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IRoleAssigmentContext roleAssigmentContext, IValidationListofRolesContext validationListofRolesContext)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleAssigmentContext = roleAssigmentContext;
            _validationListofRolesContext = validationListofRolesContext;

        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await _userRepository.SaveAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}
