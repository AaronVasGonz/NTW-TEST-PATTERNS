using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Models;
namespace Service.Services;

public interface ICustomerService
{
    Task<bool> DeleteCustomerAsync(int id);
    Task<Customer> GetCustomerByIdAsync(int id);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> SaveCustomerAsync(Customer customer);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _customerRepository.GetCustomersAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetCustomerByIdAsync(id);
    }

    public async Task<Customer> SaveCustomerAsync(Customer customer)
    {
        return await _customerRepository.SaveAsync(customer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        return await _customerRepository.DeleteAsync(id);
    }
}
