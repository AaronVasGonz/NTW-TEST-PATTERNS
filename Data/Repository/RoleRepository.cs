using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IRoleRepository
    {
        Task<bool> DeleteRoleAsync(int id);
        Task<Role> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> SaveRoleAsync(Role role);
    }

    /// <summary>
    /// Repository for handling operations related to Roles.
    /// Provides methods for CRUD operations on roles.
    /// </summary>
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        /// <summary>
        /// Retrieves all roles asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all roles.</returns>
        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            // Retrieves all roles from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a role by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>The role object if found, otherwise null.</returns>
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            // Reads all roles and returns the first matching one by ID
            var roles = await ReadAsync();
            return roles.FirstOrDefault(r => r.RoleId == id);
        }

        /// <summary>
        /// Saves a role asynchronously. If the role exists, it updates it, otherwise creates a new role.
        /// </summary>
        /// <param name="role">The role to save or update.</param>
        /// <returns>The saved or updated role object.</returns>
        public async Task<Role> SaveRoleAsync(Role role)
        {
            // Checks if the role exists (i.e., if it has a valid RoleId)
            var exists = role.RoleId > 0;

            // If the role exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(role);
            else
                await CreateAsync(role);

            // Retrieves the updated list of roles and returns the saved role by ID
            var updatedRoles = await ReadAsync();
            return updatedRoles.SingleOrDefault(r => r.RoleId == role.RoleId);
        }

        /// <summary>
        /// Deletes a role asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>A boolean indicating whether the role was successfully deleted.</returns>
        public async Task<bool> DeleteRoleAsync(int id)
        {
            // Attempts to retrieve the role by its ID
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                return false; // If the role does not exist, return false

            // Deletes the role using the base repository
            await DeleteAsync(role);
            return true; // Returns true indicating successful deletion
        }
    }
}
