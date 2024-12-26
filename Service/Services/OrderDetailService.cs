using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IOrderDetailService
{
    Task<bool> DeleteOrderDetailAsync(int id);
    Task<OrderDetail> GetOrderDetailByIdAsync(int id);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync();
    Task<OrderDetail> SaveOrderDetailAsync(OrderDetail orderDetail);
}

public class OrderDetailService : IOrderDetailService
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    public OrderDetailService(IOrderDetailRepository orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync()
    {
        return await _orderDetailRepository.GetAllAsync();
    }

    public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
    {
        return await _orderDetailRepository.GetByIdAsync(id);
    }

    public async Task<OrderDetail> SaveOrderDetailAsync(OrderDetail orderDetail)
    {
        return await _orderDetailRepository.SaveAsync(orderDetail);
    }

    public async Task<bool> DeleteOrderDetailAsync(int id)
    {
        return await _orderDetailRepository.DeleteAsync(id);
    }
}
