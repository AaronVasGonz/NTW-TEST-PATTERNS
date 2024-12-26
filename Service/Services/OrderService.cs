using Data.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IOrderService
{
    Task<bool> DeleteOrderAsync(int id);
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> SaveOrderAsync(Order order);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetOrderByIdAsync(id);
    }

    public async Task<Order> SaveOrderAsync(Order order)
    {
        return await _orderRepository.SaveOrderAsync(order);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        return await _orderRepository.DeleteOrderAsync(id);
    }
}
