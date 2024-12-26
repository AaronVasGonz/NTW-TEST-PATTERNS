using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IOrderDetailRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail> GetByIdAsync(int id);
        Task<OrderDetail> SaveAsync(OrderDetail orderDetail);
    }

    /// <summary>
    /// Repository for handling operations related to order detail.
    /// Provides methods for CRUD operations.
    /// </summary>
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        /// <summary>
        /// Retrieves all order details asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all order details.</returns>
        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            // Retrieves all order details from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a specific order detail by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order detail to retrieve.</param>
        /// <returns>The order detail object if found, otherwise null.</returns>
        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            // Reads all order details and returns the first matching one by ID
            var items = await ReadAsync();
            return items.FirstOrDefault(o => o.OrderDetailId == id);
        }

        /// <summary>
        /// Saves an order detail asynchronously. If it exists, it updates it; otherwise, creates a new one.
        /// </summary>
        /// <param name="orderDetail">The order detail to save or update.</param>
        /// <returns>The saved or updated order detail object.</returns>
        public async Task<OrderDetail> SaveAsync(OrderDetail orderDetail)
        {
            // Checks if the order detail exists (i.e., if it has a valid OrderDetailId)
            var exists = orderDetail.OrderDetailId > 0;

            // If it exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(orderDetail);
            else
                await CreateAsync(orderDetail);

            // Retrieves the updated list and returns the saved or updated order detail by ID
            var updatedItems = await ReadAsync();
            return updatedItems.SingleOrDefault(x => x.OrderDetailId == orderDetail.OrderDetailId);
        }

        /// <summary>
        /// Deletes an order detail by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order detail to delete.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            // Attempts to retrieve the order detail by its ID
            var orderDetail = await GetByIdAsync(id);
            if (orderDetail == null)
                return false; // If the order detail does not exist, return false

            // Deletes the order detail using the base repository
            await DeleteAsync(orderDetail);
            return true; // Returns true indicating successful deletion
        }
    }
}
