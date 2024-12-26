using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IProductRepository
    {
        Task<bool> DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> SaveProductAsync(Product product);
    }

    /// <summary>
    /// Repository for handling operations related to Products.
    /// Provides methods for CRUD operations on products.
    /// </summary>
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>An enumerable list of all products.</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            // Retrieves all products from the base repository
            return await ReadAsync();
        }

        /// <summary>
        /// Retrieves a product by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product object if found, otherwise null.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            // Reads all products and returns the first matching one by ID
            var products = await ReadAsync();
            return products.FirstOrDefault(p => p.ProductId == id);
        }

        /// <summary>
        /// Saves a product asynchronously. If the product exists, it updates it, otherwise creates a new product.
        /// </summary>
        /// <param name="product">The product to save or update.</param>
        /// <returns>The saved or updated product object.</returns>
        public async Task<Product> SaveProductAsync(Product product)
        {
            // Checks if the product exists (i.e., if it has a valid ProductId)
            var exists = product.ProductId > 0;

            // If the product exists, update it; otherwise, create a new one
            if (exists)
                await UpdateAsync(product);
            else
                await CreateAsync(product);

            // Retrieves the updated list of products and returns the saved product by ID
            var updatedProducts = await ReadAsync();
            return updatedProducts.SingleOrDefault(p => p.ProductId == product.ProductId);
        }

        /// <summary>
        /// Deletes a product asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A boolean indicating whether the product was successfully deleted.</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            // Attempts to retrieve the product by its ID
            var product = await GetProductByIdAsync(id);
            if (product == null)
                return false; // If the product does not exist, return false

            // Deletes the product using the base repository
            await DeleteAsync(product);
            return true; // Returns true indicating successful deletion
        }
    }
}
