using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.validations;

public interface IGeneralValidationFunctions
{
    void ValidateStringField(string fieldValue, string fieldName);
    void ValidateIntField(int fieldValue, string fieldName);

    void ValidateFormFileFiled(IFormFile file);

    void ValidateMultipleFormFileFiled(IEnumerable<IFormFile> files);

    void ValidateDateTimeField(DateTime fieldValue, string fieldName);
}
public class GeneralValidationFunctions : IGeneralValidationFunctions
{
    public void ValidateStringField(string fieldValue, string fieldName)
    {
        Guard.Against.NullOrWhiteSpace(fieldValue, fieldName);
        Guard.Against.InvalidInput(fieldValue, fieldName,
            (value) => value.Length <= 100 && value.Length >= 3,
            $"{fieldName} must be between 3 and 100 characters.");
        Guard.Against.InvalidFormat(fieldValue, fieldName,
            @"^[a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]*$",
            $"{fieldName} must be alphanumeric, can include accents and spaces.");
    }

    public void ValidateIntField(int fieldValue, string fieldName)
    {
        Guard.Against.OutOfRange(fieldValue, fieldName, 1, 10000);
        Guard.Against.NegativeOrZero(fieldValue, fieldName);
    }

    public void ValidateDateTimeField(DateTime fieldValue, string fieldName)
    {
        Guard.Against.Default(fieldValue, fieldName);
        Guard.Against.OutOfRange(fieldValue.Year, fieldName, 1900, 2100);
    }

    public void ValidateFormFileFiled(IFormFile file)
    {
        Guard.Against.Null(file, nameof(file));
        Guard.Against.Zero(file.Length, nameof(file));
        Guard.Against.OutOfRange(file.Length, nameof(file), 1, 1048576);
    }

    public void ValidateMultipleFormFileFiled(IEnumerable<IFormFile> files)
    {
        Guard.Against.Null(files, nameof(files));
        Guard.Against.Zero(files.Count(), nameof(files));
        Guard.Against.OutOfRange(files.Count(), nameof(files), 1, 5);
        foreach (var file in files)
        {
            ValidateFormFileFiled(file);
        }
    }
}
