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
    public bool ValidateProductRequest(ProductRequest productRequest)
    {
        try
        {
            //null validation
            Guard.Against.Null(productRequest, nameof(productRequest));

            //string validation
            Guard.Against.NullOrWhiteSpace(productRequest.ProductName, nameof(productRequest.ProductName));
            Guard.Against.NullOrWhiteSpace(productRequest.SupplierName, nameof(productRequest.SupplierName));
            Guard.Against.NullOrWhiteSpace(productRequest.CategoryName, nameof(productRequest.CategoryName));
            Guard.Against.NullOrWhiteSpace(productRequest.Unit, nameof(productRequest.Unit));
            Guard.Against.NullOrWhiteSpace(productRequest.Status, nameof(productRequest.Status));

            // Validación de longitud
            Guard.Against.InvalidInput(productRequest.ProductName, nameof(productRequest.ProductName),
                (name) => name.Length <= 100 && name.Length >= 3, "Product name must be between 3 and 100 characters");

            //number validation
            Guard.Against.NegativeOrZero(productRequest.Price, nameof(productRequest.Price));
            Guard.Against.Negative(productRequest.Stock, nameof(productRequest.Stock));

            //format validation
            Guard.Against.InvalidFormat(productRequest.ProductName, nameof(productRequest.ProductName),
           @"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]*$", "Product name must be alphanumeric, can include accents and spaces.");
            Guard.Against.InvalidFormat(productRequest.SupplierName, nameof(productRequest.SupplierName),
                @"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]*$", "Supplier name must be alphanumeric, can include accents and spaces.");
            Guard.Against.InvalidFormat(productRequest.CategoryName, nameof(productRequest.CategoryName),
                @"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]*$", "Category name must be alphanumeric, can include accents and spaces.");
            Guard.Against.InvalidFormat(productRequest.Unit, nameof(productRequest.Unit),
                @"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]*$", "Unit must be alphanumeric, can include accents and spaces.");

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
