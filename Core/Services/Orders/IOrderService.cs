using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IOrderService
    {
        Order CreateOrder(Order orderDetails);
        Order GetOrderById(Guid orderId);
        Order UpdateOrder(Guid orderId, Order orderDetails);
        void DeleteOrder(Guid orderId);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrderByName(string filter);
        void AddOrders(IEnumerable<Order> orders);
        decimal CalculateTotalCostOfAllOrders();

    }
}
