using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Data.Repository
{
    /// <summary>
    /// Interface that defines the basic operations for a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity to be handled by the repository.</typeparam>
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Asynchronously creates a new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the creation was successful.</returns>
        Task<bool> CreateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Asynchronously checks if an entity exists in the repository.
        /// </summary>
        /// <param name="entity">The entity to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the entity exists.</returns>
        Task<bool> ExistsAsync(T entity);

        /// <summary>
        /// Asynchronously reads all entities from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities.</returns>
        Task<IEnumerable<T>> ReadAsync();

        /// <summary>
        /// Asynchronously updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the update was successful.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously updates multiple entities in the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the update was successful.</returns>
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);
    }

    /// <summary>
    /// Implementation of the base repository, handling CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of entity handled by the repository.</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        public RepositoryBase()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Asynchronously creates a new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the creation was successful.</returns>
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Asynchronously updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the update was successful.</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _context.Update(entity);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Asynchronously updates multiple entities in the repository.
        /// </summary>
        /// <param name="entities">The collection of entities to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the update was successful.</returns>
        public async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
        {
            try
            {
                _context.UpdateRange(entities);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Asynchronously deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _context.Remove(entity);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Asynchronously reads all entities from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities.</returns>
        public async Task<IEnumerable<T>> ReadAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Asynchronously checks if an entity exists in the repository.
        /// </summary>
        /// <param name="entity">The entity to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the entity exists.</returns>
        public async Task<bool> ExistsAsync(T entity)
        {
            try
            {
                var items = await ReadAsync();
                return items.Any(x => x.Equals(entity));
            }
            catch (Exception ex)
            {
                throw new NTWException(ex);
            }
        }

        /// <summary>
        /// Saves the changes asynchronously to the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the changes were saved successfully.</returns>
        protected async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

