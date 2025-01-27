using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOS;

namespace Service.Services.validations.Users;

public interface IValidateUserRequestService
{
    void ValidateUserRequest(UserRequest userRequest);
}

public class ValidateUserRequestService : IValidateUserRequestService
{
    private readonly IGeneralValidationFunctions _generalValidationFunctions;
    public ValidateUserRequestService(IGeneralValidationFunctions generalValidationFunctions)
    {
        _generalValidationFunctions = generalValidationFunctions;
    }

    public void ValidateUserRequest(UserRequest userRequest)
    {
        try
        {
            //null validation
            Guard.Against.Null(userRequest, nameof(userRequest));
            //string validation
            _generalValidationFunctions.ValidateStringField(userRequest.Username, nameof(userRequest.Username));
            _generalValidationFunctions.ValidateStringField(userRequest.Email, nameof(userRequest.Email));
            _generalValidationFunctions.ValidateStringField(userRequest.Password, nameof(userRequest.Password));
            foreach (var role in userRequest.Roles)
            {
                _generalValidationFunctions.ValidateStringField(role, nameof(userRequest.Roles));
            }
            //value validation of status
            Guard.Against.InvalidInput(userRequest.Status, nameof(userRequest.Status),
                (status) => Enum.TryParse(typeof(UserStatus), status, true, out _),
                "Invalid status value");

        }
        catch (Exception ex)
        {
            throw new ValidationException($"User validation failed: {ex.Message}", ex);
        }
    }
}

public enum UserStatus
{
    Active,
    Inactive,
    Discontinued
}
