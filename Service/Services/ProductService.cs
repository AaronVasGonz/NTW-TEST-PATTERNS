using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IProductService
{
    Task<bool> DeleteProductAsync(int id);
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> SaveProductAsync(Product product);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _productRepository.GetProductsAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetProductByIdAsync(id);
    }

    public async Task<Product> SaveProductAsync(Product product)
    {
        return await _productRepository.SaveProductAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteProductAsync(id);
    }

}
