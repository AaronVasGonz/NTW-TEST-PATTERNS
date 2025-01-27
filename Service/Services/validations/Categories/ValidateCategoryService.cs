using Ardalis.GuardClauses;
using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Categories;

public interface IValidateCategoryService
{
    void ValidateCategoryRequest(CategoryRequest categoryRequest);
}

public class ValidateCategoryService : IValidateCategoryService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions;
    public ValidateCategoryService(IGeneralValidationFunctions generalValidationFunctions)
    {
        _generalValidationFunctions = generalValidationFunctions;
    }

    public void ValidateCategoryRequest(CategoryRequest categoryRequest)
    {
        try
        {
            _generalValidationFunctions.ValidateStringField(categoryRequest.CategoryName, nameof(CategoryRequest.CategoryName));
            _generalValidationFunctions.ValidateStringField(categoryRequest.Description, nameof(CategoryRequest.Description));
            _generalValidationFunctions.ValidateStringField(categoryRequest.Status, nameof(CategoryRequest.Status));

            //value validation for status
            Guard.Against.InvalidInput(categoryRequest.Status, nameof(categoryRequest.Status),
                (status) => Enum.TryParse(typeof(CategoryStatus), status, true, out _),
                "Invalid status value");
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Category validation failed: {ex.Message}", ex);
        }
    }
}

public enum CategoryStatus
{
    Active,
    Inactive,
    Discontinued
}
