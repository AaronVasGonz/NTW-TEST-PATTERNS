using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IShipperService
{
    Task<bool> DeleteShipperAsync(int id);
    Task<Shipper> GetShipperByIdAsync(int id);
    Task<IEnumerable<Shipper>> GetShippersAsync();
    Task<Shipper> SaveShipperAsync(Shipper shipper);
}

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _shipperRepository;
    public ShipperService(IShipperRepository shipperRepository)
    {
        _shipperRepository = shipperRepository;
    }

    public async Task<IEnumerable<Shipper>> GetShippersAsync()
    {
        return await _shipperRepository.GetAllAsync();

    }

    public async Task<Shipper> GetShipperByIdAsync(int id)
    {
        return await _shipperRepository.GetByIdAsync(id);
    }

    public async Task<Shipper> SaveShipperAsync(Models.Shipper shipper)
    {
        return await _shipperRepository.SaveAsync(shipper);
    }

    public async Task<bool> DeleteShipperAsync(int id)
    {
        return await _shipperRepository.DeleteAsync(id);
    }
}
