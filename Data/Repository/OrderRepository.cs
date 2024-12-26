using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IOrderRepository
    {
        Task<bool> DeleteOrderAsync(int id);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> SaveOrderAsync(Order order);
    }

    /// <summary>
    /// Repository for handling operations related to Orders.
    /// Provides methods for CRUD operations on orders.
    /// </summary>
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        /// <summary>
        /// Retrieves all orders asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all orders.</returns>
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            // Retrieves all orders from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves an order by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order object if found, otherwise null.</returns>
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            // Reads all orders and returns the first matching one by ID
            var orders = await ReadAsync();
            return orders.FirstOrDefault(o => o.OrderId == id);
        }

        /// <summary>
        /// Saves an order asynchronously. If the order exists, it updates it, otherwise creates a new order.
        /// </summary>
        /// <param name="order">The order to save or update.</param>
        /// <returns>The saved or updated order object.</returns>
        public async Task<Order> SaveOrderAsync(Order order)
        {
            // Checks if the order exists (i.e., if it has a valid OrderId)
            var exists = order.OrderId > 0;

            // If the order exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(order);
            else
                await CreateAsync(order);

            // Retrieves the updated list of orders and returns the saved order by ID
            var updatedOrders = await ReadAsync();
            return updatedOrders.SingleOrDefault(o => o.OrderId == order.OrderId);
        }

        /// <summary>
        /// Deletes an order asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A boolean indicating whether the order was successfully deleted.</returns>
        public async Task<bool> DeleteOrderAsync(int id)
        {
            // Attempts to retrieve the order by its ID
            var order = await GetOrderByIdAsync(id);
            if (order == null)
                return false; // If the order does not exist, return false

            // Deletes the order using the base repository
            await DeleteAsync(order);
            return true; // Returns true indicating successful deletion
        }
    }
}
