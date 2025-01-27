using Ardalis.GuardClauses;
using Models.DTOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations.Roles;

public interface IValidateRoleService
{
    bool ValidateRoleRequest(RoleRequest roleRequest);
}

public class ValidateRoleService(IGeneralValidationFunctions generalValidationFunctions) : IValidateRoleService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions = generalValidationFunctions;

    public bool ValidateRoleRequest(RoleRequest roleRequest)
    {
        try
        {
            Guard.Against.Null(roleRequest, nameof(roleRequest));

            _generalValidationFunctions.ValidateStringField(roleRequest.RoleName, nameof(roleRequest.RoleName));
            _generalValidationFunctions.ValidateStringField(roleRequest.Description, nameof(roleRequest.Description));

            return true;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Role validation failed: {ex.Message}", ex);
        }
    }
}
