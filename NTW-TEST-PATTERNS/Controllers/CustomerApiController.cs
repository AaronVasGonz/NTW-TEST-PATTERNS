using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Models.DTOS;
using Service.Services.validations.Categories;
using Service.Services.validations.Customers;
using Models;

namespace NTW_TEST_PATTERNS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerApiController(ICustomerService customerService, IValidateCustomerService validateCustomerService) : Controller
{
    private readonly ICustomerService _customerService = customerService;
    private readonly IValidateCustomerService _validateCustomerService = validateCustomerService;

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetCustomersAsync();
        return customers == null ? throw new KeyNotFoundException("Customers not found") : (IActionResult)Ok(customers);
    }

    [HttpGet("customer/{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return customer == null ? throw new KeyNotFoundException("Customer not found") : (IActionResult)Ok(customer);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveCustomer(CustomerRequest customerRequest)
    {
        _validateCustomerService.ValidateCustomerRequest(customerRequest);

        //create a new customer intance
        var customer = new Customer
        {
            CustomerName = customerRequest.Name,
            ContactName = customerRequest.Contact,
            Address = customerRequest.Address,
            City = customerRequest.City,
            Country = customerRequest.Country,
            PostalCode = customerRequest.PostalCode,
            UserId = customerRequest.UserId,
        };

        //save the customer
        var customerSaved = await _customerService.SaveCustomerAsync(customer);

        return customerSaved == null ? throw new Exception("Customer not saved") : (IActionResult)Ok(customerSaved);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCustomer(CustomerRequest customerRequest)
    {
        _validateCustomerService.ValidateCustomerRequest(customerRequest);
        //create a new customer intance
        var customer = new Customer
        {
            CustomerId = customerRequest.CustomerId,
            CustomerName = customerRequest.Name,
            ContactName = customerRequest.Contact,
            Address = customerRequest.Address,
            City = customerRequest.City,
            Country = customerRequest.Country,
            PostalCode = customerRequest.PostalCode,
            UserId = customerRequest.UserId,
        };
        //update the customer
        var customerUpdated = await _customerService.SaveCustomerAsync(customer);
        return customerUpdated == null ? throw new Exception("Customer not updated") : (IActionResult)Ok(customerUpdated);
    }
}
