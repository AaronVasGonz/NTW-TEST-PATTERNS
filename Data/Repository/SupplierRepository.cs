using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    /// <summary>
    /// Interface for Supplier repository that defines CRUD operations.
    /// </summary>
    public interface ISupplierRepository
    {
        /// <summary>
        /// Deletes a supplier asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to delete.</param>
        /// <returns>A boolean indicating whether the supplier was successfully deleted.</returns>
        Task<bool> DeleteSupplierAsync(int id);

        /// <summary>
        /// Retrieves all suppliers asynchronously.
        /// </summary>
        /// <returns>An enumerable list of suppliers.</returns>
        Task<IEnumerable<Supplier>> GetSuppliersAsync();

        /// <summary>
        /// Retrieves a supplier by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the supplier to retrieve.</param>
        /// <returns>The supplier object if found, otherwise null.</returns>
        Task<Supplier> GetSupplierByIdAsync(int id);

        /// <summary>
        /// Saves a supplier asynchronously. If the supplier exists, it updates it, otherwise creates a new supplier.
        /// </summary>
        /// <param name="supplier">The supplier to save or update.</param>
        /// <returns>The saved or updated supplier object.</returns>
        Task<Supplier> SaveSupplierAsync(Supplier supplier);
    }

    /// <summary>
    /// Supplier repository that implements ISupplierRepository interface.
    /// Provides methods for CRUD operations on suppliers.
    /// </summary>
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        /// <summary>
        /// Retrieves all suppliers asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all suppliers.</returns>
        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            // Retrieves all suppliers from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a supplier by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the supplier to retrieve.</param>
        /// <returns>The supplier object if found, otherwise null.</returns>
        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            // Reads all suppliers and returns the first matching one by ID
            var suppliers = await ReadAsync();
            return suppliers.FirstOrDefault(s => s.SupplierId == id);
        }

        /// <summary>
        /// Saves a supplier asynchronously. If the supplier exists, it updates it; otherwise, it creates a new supplier.
        /// </summary>
        /// <param name="supplier">The supplier to save or update.</param>
        /// <returns>The saved or updated supplier object.</returns>
        public async Task<Supplier> SaveSupplierAsync(Supplier supplier)
        {
            // Checks if the supplier exists (i.e., if it has a valid SupplierId)
            var exists = supplier.SupplierId != null && supplier.SupplierId > 0;

            // If the supplier exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(supplier);
            else
                await CreateAsync(supplier);

            // Retrieves the updated list of suppliers and returns the saved supplier by ID
            var updated = await ReadAsync();
            return updated.SingleOrDefault(x => x.SupplierId == supplier.SupplierId);
        }

        /// <summary>
        /// Deletes a supplier asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier to delete.</param>
        /// <returns>A boolean indicating whether the supplier was successfully deleted.</returns>
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            // Attempts to retrieve the supplier by its ID
            var supplier = await GetSupplierByIdAsync(id);
            if (supplier == null)
                return false; // If the supplier does not exist, return false

            // Deletes the supplier using the base repository
            await DeleteAsync(supplier);
            return true; // Returns true indicating successful deletion
        }
    }
}
