using Ardalis.GuardClauses;
using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Products;

public interface IValidateProductService
{
    bool ValidateProductRequest(ProductRequest productRequest);
}

public class ValidateProductService : IValidateProductService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions;
    public ValidateProductService(IGeneralValidationFunctions generalValidationFunctions)
    {
        _generalValidationFunctions = generalValidationFunctions;
    }
    public bool ValidateProductRequest(ProductRequest productRequest)
    {
        try
        {
            //null validation
            Guard.Against.Null(productRequest, nameof(productRequest));

            //string validation
            _generalValidationFunctions.ValidateStringField(productRequest.ProductName, nameof(productRequest.ProductName));
            _generalValidationFunctions.ValidateStringField(productRequest.SupplierName, nameof(productRequest.SupplierName));
            _generalValidationFunctions.ValidateStringField(productRequest.CategoryName, nameof(productRequest.CategoryName));
            _generalValidationFunctions.ValidateStringField(productRequest.Unit, nameof(productRequest.Unit));

            //IMAGES VALIDATION
            Guard.Against.Null(productRequest.Images, nameof(productRequest.Images));
            Guard.Against.OutOfRange(productRequest.Images.Count(), nameof(productRequest.Images), 1, 5);
            //foreach (var image in productRequest.Images)
            //{
            //    Guard.Against.Null(image, nameof(productRequest.Images));
            //    Guard.Against.InvalidInput(image.ContentType, nameof(image.ContentType),
            //        (type) => new[] { "image/jpeg", "image/png" }.Contains(type), "Invalid image type");
            //}

            //number validation
            Guard.Against.NegativeOrZero(productRequest.Price, nameof(productRequest.Price));
            Guard.Against.Negative(productRequest.Stock, nameof(productRequest.Stock));

            //value validation for status
            Guard.Against.InvalidInput(productRequest.Status, nameof(productRequest.Status),
                (status) => new[] { "active", "inactive", "discontinued" }.Contains(status),
                "Invalid status value");

            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Product validation failed: {ex.Message}", ex);
        }
    }
}
