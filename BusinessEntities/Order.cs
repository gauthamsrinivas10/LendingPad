using System;
using System.Collections.Generic;

namespace BusinessEntities
{
    public class Order : IdObject
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get;  set; }
        public IEnumerable<Product> Products { get; set; }
        // Constructor to initialize the Order object with required properties
        public Order(string orderNumber, DateTime orderDate, decimal total, string customerName, IEnumerable<Product> products)
        {            
            OrderNumber = orderNumber ?? throw new ArgumentNullException(nameof(orderNumber), "Order number cannot be null or empty."); ;
            OrderDate = orderDate;
            Products = products ?? throw new ArgumentNullException(nameof(products), "Products cannot be null.");            
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName), "Customer name cannot be null or empty.");
            Total = CalculateTotal();

        }

        // Calculate total price for the order.
        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var product in Products)
            {
                total += product.CalculateTotalPrice();
            }
            return total;
        }
    }
}
