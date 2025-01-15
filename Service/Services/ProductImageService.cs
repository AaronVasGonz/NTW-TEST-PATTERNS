using Data.Repository;
using NTW_TEST_PATTERNS.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IProductImageService
{
    Task<bool> DeleteProductImageAsync(int id);
    Task<Product_Image> GetProductImageByIdAsync(int id);
    Task<IEnumerable<Product_Image>> GetProductImagesAsync();
    Task<IEnumerable<Product_Image>> GetProductImagesByProductIdAsync(int productId);
    Task<Product_Image> SaveProductImageAsync(Product_Image productImage);
}

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _productImageRepository;
    public ProductImageService(IProductImageRepository productImageRepository)
    {
        _productImageRepository = productImageRepository;
    }

    public async Task<IEnumerable<Product_Image>> GetProductImagesAsync()
    {
        return await _productImageRepository.GetProductImagesAsync();
    }

    public async Task<Product_Image> GetProductImageByIdAsync(int id)
    {
        return await _productImageRepository.GetProductImageByIdAsync(id);
    }

    public async Task<IEnumerable<Product_Image>> GetProductImagesByProductIdAsync(int productId)
    {
        return await _productImageRepository.GetProductImagesByProductIdAsync(productId);
    }

    public async Task<Product_Image> SaveProductImageAsync(Product_Image productImage)
    {
        return await _productImageRepository.SaveProductImageAsync(productImage);
    }

    public async Task<bool> DeleteProductImageAsync(int id)
    {
        return await _productImageRepository.DeleteProductImageAsync(id);
    }
}
