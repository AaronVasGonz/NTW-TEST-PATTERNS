﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using NTW_TEST_PATTERNS.Models.EFModels;
using Service.Services;
using Service.Services.validations.Products;
using Service.Strategies.ImageUploader;
using Services;
using Sprache;

namespace NTW_TEST_PATTERNS.Controllers
{
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
        //extra comment

        [HttpGet("getAll")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                return Ok( products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveProduct([FromForm] ProductRequest productRequest)
        {
            try
            {
                if (productRequest == null)
                {
                    return BadRequest("Invalid product");
                }

                //now we need to validate the product fields
                var isValidProduct = _validateProductService.ValidateProductRequest(productRequest);
                if (!isValidProduct)
                {
                    return BadRequest("Invalid Product Data");
                }

                //check if the supplir exists and return the supplier id
                var supplier = await _supplierService.GetSupplierBYName(productRequest.SupplierName);
                if (supplier == null)
                {
                    return BadRequest("Invalid Supplier");
                }
                //check if the category exists and return the category id
                var category = await _categoryService.GetCategoryByNameAsync(productRequest.CategoryName);
                if (category == null)
                {
                    return BadRequest("Invalid Category");
                }

                //save the product
                //map the product request to the product model
                var product = new Product
                {
                    ProductName = productRequest.ProductName,
                    CategoryId = category.CategoryId,
                    SupplierId = supplier.SupplierId,
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
            catch (Exception ex)
            {
                var errorDetails = new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };
                return StatusCode(500, errorDetails);
            }
        }
    }
}
    
    

