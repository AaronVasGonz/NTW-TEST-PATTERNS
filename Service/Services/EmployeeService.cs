using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IEmployeeService
{
    Task<bool> DeleteEmployeeAsync(int id);
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee> SaveEmployeeAsync(Employee employee);
}

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _employeeRepository.GetEmployeesAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await _employeeRepository.GetEmployeeByIdAsync(id);
    }

    public async Task<Employee> SaveEmployeeAsync(Employee employee)
    {
        return await _employeeRepository.SaveAsync(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        return await _employeeRepository.DeleteAsync(id);
    }
}
