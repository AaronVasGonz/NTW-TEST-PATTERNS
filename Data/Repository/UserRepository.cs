using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IUserRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> SaveAsync(User user);
    }

    /// <summary>
    /// Repository for handling operations related to a user.
    /// Provides methods for CRUD operations.
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all users.</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Retrieves all users from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a specific user by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            // Reads all users and returns the first matching one by ID
            var users = await ReadAsync();
            return users.FirstOrDefault(u => u.UserId == id);
        }

        /// <summary>
        /// Saves a user asynchronously. If the user exists, it updates their record; otherwise, creates a new user.
        /// </summary>
        /// <param name="user">The user to save or update.</param>
        /// <returns>The saved or updated user object.</returns>
        public async Task<User> SaveAsync(User user)
        {
            // Checks if the user exists (i.e., if they have a valid UserId)
            var exists = user.UserId > 0;

            // If the user exists, update their record; otherwise, create a new user
            if (exists)
                await UpdateAsync(user);
            else
                await CreateAsync(user);

            // Retrieves the updated list of users and returns the saved or updated user by ID
            var updatedUsers = await ReadAsync();
            return updatedUsers.SingleOrDefault(x => x.UserId == user.UserId);
        }

        /// <summary>
        /// Deletes a user by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            // Attempts to retrieve the user by their ID
            var user = await GetUserByIdAsync(id);
            if (user == null)
                return false; // If the user does not exist, return false

            // Deletes the user using the base repository
            await DeleteAsync(user);
            return true; // Returns true indicating successful deletion
        }
    }
}
