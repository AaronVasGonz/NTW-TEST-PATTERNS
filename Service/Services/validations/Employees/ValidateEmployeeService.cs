using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Employees;

public interface IValidateEmployeeService
{
    bool ValidateEmployeeRequest(EmployeeRequest employeeRequest);
}

public class ValidateEmployeeService(IGeneralValidationFunctions generalValidationFunctions) : IValidateEmployeeService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions = generalValidationFunctions;

    public bool ValidateEmployeeRequest(EmployeeRequest employeeRequest)
    {
        try
        {
            _generalValidationFunctions.ValidateStringField(employeeRequest.FirstName, nameof(employeeRequest.FirstName));
            _generalValidationFunctions.ValidateStringField(employeeRequest.LastName, nameof(employeeRequest.LastName));
            _generalValidationFunctions.ValidateStringField(employeeRequest.Notes, nameof(employeeRequest.Notes));
            _generalValidationFunctions.ValidateIntField(employeeRequest.UserId ?? 0, nameof(employeeRequest.UserId));
            _generalValidationFunctions.ValidateFormFileFiled(employeeRequest.Photo);
            _generalValidationFunctions.ValidateDateTimeField(employeeRequest.BirthDate ?? DateTime.Now, nameof(employeeRequest.BirthDate));
            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Employee validation failed: {ex.Message}", ex);
        }
    }


}
