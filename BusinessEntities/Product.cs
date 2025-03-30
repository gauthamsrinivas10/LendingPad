using System;

namespace BusinessEntities
{
    public class Product : IdObject
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get;  set; }
        public int Quantity { get;  set; }

        public Product(string description, int quantity, string name, decimal price) : base()
        {            
            Price = price;
            Quantity = quantity;
            Name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
            Description = description ?? throw new ArgumentNullException(nameof(description), "Description cannot be null or empty."); ;
        }
        public decimal CalculateTotalPrice()
        {
            return Price * Quantity;
        }
    }
}
