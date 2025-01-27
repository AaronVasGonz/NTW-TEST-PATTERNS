using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using NTW_TEST_PATTERNS.Models.EFModels;
using Service.Services;
using Service.Services.validations.Products;
using Service.Strategies.ImageUploader;
namespace NTW_TEST_PATTERNS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;
    private readonly ICategoryService _categoryService;
    private readonly IValidateProductService _validateProductService;
    private readonly IImageUploaderContext _imageUploaderContext;
    private readonly IImageConverterService _imageConverterService;
    private readonly IProductImageService _productImageService;
    public ProductsApiController(
        IProductService productService,
        ISupplierService supplierService,
        ICategoryService categoryService,
        IValidateProductService validateProductService,
        IImageUploaderContext imageUploaderContext,
        IImageConverterService imageConverterService,
        IProductImageService productImageService
        )
    {
        _productService = productService;
        _supplierService = supplierService;
        _categoryService = categoryService;
        _validateProductService = validateProductService;
        _imageUploaderContext = imageUploaderContext;
        _imageConverterService = imageConverterService;
        _productImageService = productImageService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetProducts()
    {
        var productsResponse = new List<ProductResponse>();
        var products = await _productService.GetProductsAsync();
        foreach (var product in products)
        {
            int productId = int.TryParse(product.ProductId.ToString(), out productId) ? productId : 0;
            var images = await _productImageService.GetProductImagesByProductIdAsync(productId);
            var category = await _categoryService.GetCategoryByIdAsync(product.CategoryId);
            var supplier = await _supplierService.GetSupplierByIdAsync(product.SupplierId);
            var imagesUrls = new List<string>();

            foreach (var image in images)
            {
                imagesUrls.Add(image.ImageUrl);
            }

            var productResponse = new ProductResponse
            {
                Id = product.ProductId,
                ProductName = product.ProductName,
                CategoryName = category.CategoryName,
                SupplierName = supplier.SupplierName,
                Price = product.Price,
                Unit = product.Unit,
                Stock = product.Stock,
                Status = product.Status,
                Images = imagesUrls
            };
            productsResponse.Add(productResponse);
        }

        return productsResponse == null ? throw new KeyNotFoundException("Products not found") : (IActionResult)Ok(productsResponse);

    }

    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        var images = await _productImageService.GetProductImagesByProductIdAsync(id);
        var imagesUrls = new List<string>();
        var category = await _categoryService.GetCategoryByIdAsync(product.CategoryId);
        var supplier = await _supplierService.GetSupplierByIdAsync(product.SupplierId);

        foreach (var image in images)
        {
            imagesUrls.Add(image.ImageUrl);
        }

        var productResponse = new ProductResponse
        {
            Id = product.ProductId,
            ProductName = product.ProductName,
            CategoryName = category.CategoryName,
            SupplierName = supplier.SupplierName,
            Price = product.Price,
            Unit = product.Unit,
            Stock = product.Stock,
            Status = product.Status,
            Images = imagesUrls
        };

        return productResponse == null ? throw new KeyNotFoundException("Product not found") : (IActionResult)Ok(productResponse);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveProduct([FromForm] ProductRequest productRequest)
    {
        if (productRequest == null)
        {
            throw new ArgumentException("Invalid product");
        }

        //now we need to validate the product fields
        var isValidProduct = _validateProductService.ValidateProductRequest(productRequest);
        if (!isValidProduct)
        {
            throw new InvalidOperationException("Invalid Product Properties");
        }

        //check if the supplir exists and return the supplier id
        var supplier = await _supplierService.GetSupplierBYName(productRequest.SupplierName);
        if (supplier == null)
        {
            throw new KeyNotFoundException("Invalid Supplier Name (not found)");
        }
        //check if the category exists and return the category id
        var category = await _categoryService.GetCategoryByNameAsync(productRequest.CategoryName);
        if (category == null)
        {
            throw new KeyNotFoundException("Invalid Category Name (not found");
        }

        //save the product
        //map the product request to the product model
        var product = new Product
        {
            ProductName = productRequest.ProductName,
            CategoryId = category.CategoryId ?? 0,
            SupplierId = supplier.SupplierId ?? 0,
            Price = productRequest.Price,
            Unit = productRequest.Unit,
            Stock = productRequest.Stock,
            Status = productRequest.Status
        };

        var productSave = await _productService.SaveProductAsync(product);
        //convert the images to streams
        var imagesStreams = await _imageConverterService.ConvertImagesToStreamsAsync(productRequest.Images);
        //upload the images
        var imagesUrls = await _imageUploaderContext.UploadMultipleImageAsync(imagesStreams, productSave.ProductName);
        //save the images in the database
        foreach (var imageUrl in imagesUrls)
        {
            var productImage = new Product_Image
            {
                ProductId = productSave.ProductId,
                ImageUrl = imageUrl
            };
            await _productImageService.SaveProductImageAsync(productImage);
        }

        return Ok(new { message = "Product has been added successfully" });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductRequest productRequest)
    {

        if (productRequest == null)
        {
            throw new ArgumentException("Invalid product");
        }

        //now we need to validate the product fields
        var isValidProduct = _validateProductService.ValidateProductRequest(productRequest);
        if (!isValidProduct)
        {
            throw new InvalidOperationException("Invalid Product Properties");
        }

        //check if the supplir exists and return the supplier id
        var supplier = await _supplierService.GetSupplierBYName(productRequest.SupplierName);
        if (supplier == null)
        {
            throw new KeyNotFoundException("Invalid Supplier Name (not found)");
        }

        //check if the category exists and return the category id
        var category = await _categoryService.GetCategoryByNameAsync(productRequest.CategoryName);
        if (category == null)
        {
            throw new KeyNotFoundException("Invalid Category Name (not found");
        }

        //save the product
        //map the product request to the product model
        var product = new Product
        {
            ProductId = productRequest.Id,
            ProductName = productRequest.ProductName,
            CategoryId = category.CategoryId ?? 0,
            SupplierId = supplier.SupplierId ?? 0,
            Price = productRequest.Price,
            Unit = productRequest.Unit,
            Stock = productRequest.Stock,
            Status = productRequest.Status
        };

        var productSave = await _productService.SaveProductAsync(product);

        //convert the images to streams
        var imagesStreams = await _imageConverterService.ConvertImagesToStreamsAsync(productRequest.Images);

        int productId = int.TryParse(productRequest.Id.ToString(), out productId) ? productId : 0;
        //now we need to delete the old images by pproduct
        var deletedImages = await _productImageService.DeleteProductImagesByProductIdAsync(productId);
        //upload the new images
        var imagesUrls = await _imageUploaderContext.UploadMultipleImageAsync(imagesStreams, productSave.ProductName);

        //save the images in the database
        foreach (var imageUrl in imagesUrls)
        {
            var productImage = new Product_Image
            {
                ProductId = productSave.ProductId,
                ImageUrl = imageUrl
            };
            var savedImages = await _productImageService.SaveProductImageAsync(productImage);
            if (savedImages == null)
                throw new InvalidOperationException("Error saving the images");
        }

        return Ok(new { message = "Product has been updated successfully" });
    }

}

