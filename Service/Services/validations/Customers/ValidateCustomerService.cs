using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Customers;

public interface IValidateCustomerService
{
    bool ValidateCustomerRequest(CustomerRequest customerRequest);
}

public class ValidateCustomerService(IGeneralValidationFunctions generalValidationFunctions) : IValidateCustomerService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions = generalValidationFunctions;

    public bool ValidateCustomerRequest(CustomerRequest customerRequest)
    {
        try
        {
            _generalValidationFunctions.ValidateStringField(customerRequest.Name, nameof(customerRequest.Name));
            _generalValidationFunctions.ValidateStringField(customerRequest.Contact, nameof(customerRequest.Contact));
            _generalValidationFunctions.ValidateStringField(customerRequest.Address, nameof(customerRequest.Address));
            _generalValidationFunctions.ValidateStringField(customerRequest.City, nameof(customerRequest.City));
            _generalValidationFunctions.ValidateStringField(customerRequest.Country, nameof(customerRequest.Country));
            _generalValidationFunctions.ValidateStringField(customerRequest.PostalCode, nameof(customerRequest.PostalCode));
            _generalValidationFunctions.ValidateIntField(customerRequest.UserId ?? 0, nameof(customerRequest.UserId));
            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Customer validation failed: {ex.Message}", ex);
        }
    }
}
