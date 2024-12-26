using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Data.Repository;

public interface ICustomerRepository
{
    Task<bool> DeleteAsync(int id);
    Task<Customer> GetCustomerByIdAsync(int id);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> SaveAsync(Customer customer);
}

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await ReadAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        var customers = await ReadAsync();

        return customers.FirstOrDefault(x => x.CustomerId == id);
    }

    public async Task<Customer> SaveAsync(Customer customer)
    {

        var exists = customer.CustomerId > 0;

        if (exists)
            await UpdateAsync(customer);
        else
            await CreateAsync(customer);

        var updatedCustomers = await ReadAsync();
        return updatedCustomers.SingleOrDefault(x => x.CustomerId == customer.CustomerId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customers = await ReadAsync();
        var customer = customers.FirstOrDefault(x => x.CustomerId == id);

        if (customer == null)
            return false;

        await DeleteAsync(customer);
        return true;
    }

}
