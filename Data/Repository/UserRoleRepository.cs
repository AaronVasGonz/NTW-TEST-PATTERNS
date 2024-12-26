using Models;
using Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    /// <summary>
    /// Interface for UserRole repository.
    /// Provides methods for CRUD operations on user roles.
    /// </summary>
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Retrieves all user roles asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all user roles.</returns>
        Task<IEnumerable<UserRole>> GetUserRolesAsync();

        /// <summary>
        /// Saves a user role asynchronously. If the user role exists, it updates it, otherwise creates a new user role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>The saved or updated user role object.</returns>
        Task<UserRole> SaveUserRoleAsync(int userId, int roleId);
    }

    /// <summary>
    /// Repository for handling operations related to UserRoles.
    /// Provides methods for CRUD operations on user roles.
    /// </summary>
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        /// <summary>
        /// Retrieves all user roles asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all user roles.</returns>
        public async Task<IEnumerable<UserRole>> GetUserRolesAsync()
        {
            // Retrieves all roles from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Saves a user role asynchronously. If the user role exists, it updates it, otherwise creates a new user role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>The saved or updated user role object.</returns>
        public async Task<UserRole> SaveUserRoleAsync(int userId, int roleId)
        {
            // Check if the UserRole relationship already exists
            var existingUserRole = (await ReadAsync()).SingleOrDefault(r => r.UserId == userId && r.RoleId == roleId);

            if (existingUserRole != null)
            {
                // Update the existing relationship if necessary
                existingUserRole.RoleId = roleId;
                await UpdateAsync(existingUserRole);
                return existingUserRole;
            }
            else
            {
                // Create a new UserRole relationship
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };
                await CreateAsync(userRole);
                return userRole;
            }
        }
    }
}
