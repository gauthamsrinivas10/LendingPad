using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();

        public Order CreateOrder(Order orderDetails)
        {
            orderDetails = new Order(orderDetails.OrderNumber, orderDetails.OrderDate, orderDetails.Total, orderDetails.CustomerName, orderDetails.Products);
            _orders.Add(orderDetails); // Add the order to the list
            return orderDetails; // Return the created order
        }

        public void DeleteOrder(Guid orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                _orders.Remove(order); // Remove the order from the list
            }
            else
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found.");
            }
        }

        public IEnumerable<Order> GetOrderByName(string filter)
        {
            return string.IsNullOrEmpty(filter)
                ? _orders
                : _orders.FindAll(order => order.CustomerName.Contains(filter));
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders;
        }

        public Order GetOrderById(Guid orderId)
        {
            return _orders.Find(order => order.Id == orderId); // Find the order by ID
        }

        public Order UpdateOrder(Guid orderId, Order orderDetails)
        {
            var existingOrder = _orders.Find(order => order.Id == orderId); // Find the existing order by ID
            if (existingOrder != null)
            {
                existingOrder.OrderNumber = orderDetails.OrderNumber;
                existingOrder.CustomerName = orderDetails.CustomerName;
                existingOrder.OrderDate = orderDetails.OrderDate;
                existingOrder.Products = orderDetails.Products;
                return existingOrder; // Return the updated order
            }
            else
            {
                throw new InvalidOperationException($"Order with ID {orderId} not found.");
            }
        }

        public void AddOrders(IEnumerable<Order> orders)
        {
            foreach(var order in orders)
            {
                _orders.Add(order);
            }

        }

        public decimal CalculateTotalCostOfAllOrders()
        {
            return _orders.Sum(order => order.Total);
        }

       
    }
}
