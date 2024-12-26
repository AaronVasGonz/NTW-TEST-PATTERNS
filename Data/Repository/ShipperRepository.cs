using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IShipperRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Shipper>> GetAllAsync();
        Task<Shipper> GetByIdAsync(int id);
        Task<Shipper> SaveAsync(Shipper shipper);
    }

    /// <summary>
    /// Repository for handling operations related to a shipper.
    /// Provides methods for CRUD operations.
    /// </summary>
    public class ShipperRepository : RepositoryBase<Shipper>, IShipperRepository
    {
        /// <summary>
        /// Retrieves all shippers asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all shippers.</returns>
        public async Task<IEnumerable<Shipper>> GetAllAsync()
        {
            // Retrieves all shippers from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a specific shipper by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the shipper to retrieve.</param>
        /// <returns>The shipper object if found, otherwise null.</returns>
        public async Task<Shipper> GetByIdAsync(int id)
        {
            // Reads all shippers and returns the first matching one by ID
            var items = await ReadAsync();
            return items.FirstOrDefault(s => s.ShipperId == id);
        }

        /// <summary>
        /// Saves a shipper asynchronously. If it exists, it updates it; otherwise, creates a new one.
        /// </summary>
        /// <param name="shipper">The shipper to save or update.</param>
        /// <returns>The saved or updated shipper object.</returns>
        public async Task<Shipper> SaveAsync(Shipper shipper)
        {
            // Checks if the shipper exists (i.e., if it has a valid ShipperId)
            var exists = shipper.ShipperId > 0;

            // If it exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(shipper);
            else
                await CreateAsync(shipper);

            // Retrieves the updated list and returns the saved or updated shipper by ID
            var updatedItems = await ReadAsync();
            return updatedItems.SingleOrDefault(x => x.ShipperId == shipper.ShipperId);
        }

        /// <summary>
        /// Deletes a shipper by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the shipper to delete.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            // Attempts to retrieve the shipper by its ID
            var shipper = await GetByIdAsync(id);
            if (shipper == null)
                return false; // If the shipper does not exist, return false

            // Deletes the shipper using the base repository
            await DeleteAsync(shipper);
            return true; // Returns true indicating successful deletion
        }
    }
}
