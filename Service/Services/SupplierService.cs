using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface ISupplierService
{
    Task<bool> DeleteSupplierAsync(int id);
    Task<Supplier> GetSupplierByIdAsync(int id);

    Task<Supplier> GetSupplierBYName(string name);

    Task<IEnumerable<Supplier>> GetSuppliersAsync();
    Task<Supplier> SaveSupplierAsync(Supplier supplier);
}

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
    {
        return await _supplierRepository.GetSuppliersAsync();
    }

    public async Task<Supplier> GetSupplierByIdAsync(int id)
    {
        return await _supplierRepository.GetSupplierByIdAsync(id);
    }

    public async Task<Supplier> SaveSupplierAsync(Supplier supplier)
    {
        return await _supplierRepository.SaveSupplierAsync(supplier);
    }

    public async Task<Supplier> GetSupplierBYName(string name)
    {
        return await _supplierRepository.GetSupplierBYNameAsync(name);
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        return await _supplierRepository.DeleteSupplierAsync(id);
    }
}
