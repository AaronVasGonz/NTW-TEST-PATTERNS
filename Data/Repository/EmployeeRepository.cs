using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Data.Repository;

public interface IEmployeeRepository
{
    Task<bool> DeleteAsync(int id);
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee> SaveAsync(Employee employee);
}

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await ReadAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        var employees = await ReadAsync();

        return employees.FirstOrDefault(x => x.EmployeeId == id);
    }

    public async Task<Employee> SaveAsync(Employee employee)
    {

        var exists = employee.EmployeeId > 0;

        if (exists)
            await UpdateAsync(employee);
        else
            await CreateAsync(employee);

        var updatedEmployees = await ReadAsync();
        return updatedEmployees.SingleOrDefault(x => x.EmployeeId == employee.EmployeeId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employees = await ReadAsync();
        var employee = employees.FirstOrDefault(x => x.EmployeeId == id);

        if (employee == null)
            return false;

        await DeleteAsync(employee);
        return true;
    }

}
