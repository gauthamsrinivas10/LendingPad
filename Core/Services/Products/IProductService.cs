using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    public interface IProductService
    {
        Product CreateProduct(Product product);
        Product UpdateProduct(Guid id, Product product);
        void DeleteProduct(Guid id);
        Product GetProductById(Guid id);
        IEnumerable<Product> GetProductByName(string filter);
        IEnumerable<Product> GetAllProducts();
    }
}
