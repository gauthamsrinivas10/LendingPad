using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister]
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new List<Product>();

        public Product CreateProduct(Product product)
        {
            product = new Product(product.Description, product.Quantity, product.Name, product.Price);
            // Use reflection to set the private Id property
            Console.WriteLine($"Creating product: {product.Name}");

            _products.Add(product); // Add the product to the in-memory list
            Console.WriteLine($"Total products: {_products.Count}");  // Check if product is added

            return product; // Return the created product
        }

        public void DeleteProduct(Guid id)
        {
            var product = _products.Find(p => p.Id == id); // Find the product by ID
            if (product != null)
            {
                _products.Remove(product); // Remove the product from the list
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {id} not found."); // Handle case where product is not found
            }
        }

        public IEnumerable<Product> GetProductByName(string filter)
        {
            return string.IsNullOrEmpty(filter)
                ? _products
                : _products.FindAll(p => p.Name.Contains(filter));
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products; 
        }

        public Product GetProductById(Guid id)
        {
            return _products.Find(p => p.Id == id); // Find and return the product by ID
        }

        public Product UpdateProduct(Guid id, Product product)
        {
            var existingProduct = _products.Find(p => p.Id == id); // Find the existing product by ID
            if (existingProduct != null)
            {
                // Use reflection to set the private properties
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                return existingProduct; // Return the updated product
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {id} not found."); // Handle case where product is not found
            }
        }
    }
}
