using Ardalis.GuardClauses;
using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Suppliers;

public interface IValidateSuppliersService
{
    bool ValidateSupplierRequest(SupplierRequest supplierRequest);
}

public class ValidateSuppliersService : IValidateSuppliersService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions;
    public ValidateSuppliersService(IGeneralValidationFunctions generalValidationFunctions)
    {
        _generalValidationFunctions = generalValidationFunctions;
    }
    public bool ValidateSupplierRequest(SupplierRequest supplierRequest)
    {
        try
        {
            //validate null
            Guard.Against.Null(supplierRequest, nameof(supplierRequest));
            //validate string fields
            _generalValidationFunctions.ValidateStringField(supplierRequest.SupplierName, "SupplierName");
            _generalValidationFunctions.ValidateStringField(supplierRequest.ContactName, "ContactName");
            _generalValidationFunctions.ValidateStringField(supplierRequest.Address, "Address");
            _generalValidationFunctions.ValidateStringField(supplierRequest.City, "City");
            _generalValidationFunctions.ValidateStringField(supplierRequest.PostalCode, "PostalCode");
            _generalValidationFunctions.ValidateStringField(supplierRequest.Country, "Country");
            _generalValidationFunctions.ValidateStringField(supplierRequest.Phone, "Phone");

            //validate status
            Guard.Against.NullOrWhiteSpace(supplierRequest.Status, nameof(supplierRequest.Status));
            Guard.Against.InvalidInput(supplierRequest.Status, nameof(supplierRequest.Status),
                (status) => Enum.TryParse(typeof(SupplierStatus), status, true, out _),
                "Invalid status value. Allowed values are 'Active' or 'Inactive'.");

            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Supplier validation failed: {ex.Message}", ex);
        }
    }


}
public enum SupplierStatus
{
    Active,
    Inactive
}