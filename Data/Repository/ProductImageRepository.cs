using NTW_TEST_PATTERNS.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository;

public interface IProductImageRepository
{
    Task<bool> DeleteProductImageAsync(int id);
    Task<Product_Image> GetProductImageByIdAsync(int id);
    Task<IEnumerable<Product_Image>> GetProductImagesAsync();
    Task<IEnumerable<Product_Image>> GetProductImagesByProductIdAsync(int productId);
    Task<Product_Image> SaveProductImageAsync(Product_Image productImage);

    Task<bool> DeleteProductImagesByProductIdAsync(int productId);
}

public class ProductImageRepository : RepositoryBase<Product_Image>, IProductImageRepository
{

    public async Task<IEnumerable<Product_Image>> GetProductImagesAsync()
    {
        return await ReadAsync();
    }

    public async Task<Product_Image> GetProductImageByIdAsync(int id)
    {
        var productImages = await ReadAsync();
        return productImages.FirstOrDefault(p => p.ProductImageId == id);
    }

    public async Task<IEnumerable<Product_Image>> GetProductImagesByProductIdAsync(int productId)
    {
        var productImages = await ReadAsync();
        return productImages.Where(p => p.ProductId == productId);
    }

    public async Task<Product_Image> SaveProductImageAsync(Product_Image productImage)
    {
        var exists = productImage.ProductImageId > 0;
        if (exists)
            await UpdateAsync(productImage);
        else
            await CreateAsync(productImage);
        return productImage;
    }

    public async Task<bool> DeleteProductImageAsync(int id)
    {
        var productImage = await GetProductImageByIdAsync(id);
        return await DeleteAsync(productImage);
    }

  
    public async Task<bool> DeleteProductImagesByProductIdAsync(int productId)
    {
        var productImages = await GetProductImagesByProductIdAsync(productId);
        foreach (var productImage in productImages)
        {
            await DeleteAsync(productImage);
        }
        if (productImages.Count() > 0)
            return true;
        return false;
    }
}
