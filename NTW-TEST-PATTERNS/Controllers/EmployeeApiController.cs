using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Service.Services;
using Service.Services.validations.Employees;
using Service.Strategies.ImageUploader;

namespace NTW_TEST_PATTERNS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeApiController(
    IEmployeeService employeeService,
    IValidateEmployeeService validateEmployeeService,
    IImageUploaderContext imageUploaderContext,
    IImageConverterService imageConverterService) : Controller
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IValidateEmployeeService _validateEmployeeService = validateEmployeeService;
    private readonly IImageUploaderContext _imageUploader = imageUploaderContext;
    private readonly IImageConverterService _imageConverterService = imageConverterService;

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetEmployeesAsync();
        return employees == null ? throw new KeyNotFoundException("Employees not found") : (IActionResult)Ok(employees);
    }

    [HttpGet("employee/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return employee == null ? throw new KeyNotFoundException("Employee not found") : (IActionResult)Ok(employee);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployee(EmployeeRequest employeeRequest)
    {
        _validateEmployeeService.ValidateEmployeeRequest(employeeRequest);

        //convert the image 
        var imageStream = await _imageConverterService.ConvertImageToStreamAsync(employeeRequest.Photo);
        if (imageStream != null)
            throw new Exception("Image not converted");

        //upload the image
        var imageUrl = await _imageUploader.UploadImageAsync(imageStream, employeeRequest.FirstName + employeeRequest.LastName);
        if (imageUrl == null)
            throw new Exception("The images couldn't be uploaded");


        //create a new employee intance
        var employee = new Employee
        {
            FirstName = employeeRequest.FirstName,
            LastName = employeeRequest.LastName,
            BirthDate = employeeRequest.BirthDate,
            Notes = employeeRequest.Notes,
            Photo = imageUrl,
            UserId = employeeRequest.UserId,
        };

        //save the employee
        var employeeSaved = await _employeeService.SaveEmployeeAsync(employee);
        return employeeSaved == null ? throw new Exception("Employee not saved") : (IActionResult)Ok(employeeSaved);

    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployee(EmployeeRequest employeeRequest)
    {
        _validateEmployeeService.ValidateEmployeeRequest(employeeRequest);

        //convert the image 
        var imageStream = await _imageConverterService.ConvertImageToStreamAsync(employeeRequest.Photo);
        if (imageStream != null)
            throw new Exception("Image not converted");
        //upload the image
        var imageUrl = await _imageUploader.UploadImageAsync(imageStream, employeeRequest.FirstName + employeeRequest.LastName);
        if (imageUrl == null)
            throw new Exception("The images couldn't be uploaded");
        //create a new employee intance
        var employee = new Employee
        {
            EmployeeId = employeeRequest.EmployeeId,
            FirstName = employeeRequest.FirstName,
            LastName = employeeRequest.LastName,
            BirthDate = employeeRequest.BirthDate,
            Notes = employeeRequest.Notes,
            Photo = imageUrl,
            UserId = employeeRequest.UserId,
        };
        //save the employee
        var employeeSaved = await _employeeService.SaveEmployeeAsync(employee);
        return employeeSaved == null ? throw new Exception("Employee not updated") : (IActionResult)Ok(employeeSaved);
    }
}
