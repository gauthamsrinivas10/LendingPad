using BusinessEntities;
using Core.Factories;
using Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService), "product service cannot be null.");
        }


        [Route("{id:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProductById(Guid Id)
        {
            try
            {
                // Retrieve all products from the product service
                var product = _productService.GetProductById(Id);
                if (product == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound); // Return 404 if no products found
                }
                return Request.CreateResponse(HttpStatusCode.OK, product); // Return 200 OK with the list of products
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception
            }
        }

        
        [Route("GetProductByName")]
        [HttpGet]
        public HttpResponseMessage GetProductByName(string filter)
        {
            try
            {
                // Retrieve all products by Name from the product service
                var products = _productService.GetProductByName(filter);
                if (products == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No products found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, products);
                // Return 200 OK with the select product
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occurred while fetching products with following Trace details {ex.Message}");
            }
        }

        [Route("GetAllProducts")]
        [HttpGet]
        public HttpResponseMessage GetAllProducts()
        {
            try
            {
                // Retrieve all products from the product service
                var products = _productService.GetAllProducts();
                if (products == null || !products.Any())
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No products found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, products);
                // Return 200 OK with the list of products
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occurred while fetching products with following Trace details {ex.Message}");
            }
        }

        [Route("createProduct")]
        [HttpPost]
        public HttpResponseMessage Createproduct([FromBody] Product product)
        {
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "product cannot be null."); // Return 400 Bad Request if product is null
            }
            try
            {
                // Call the product service to create a new product
                var createdproduct = _productService.CreateProduct(product);
                return Request.CreateResponse(HttpStatusCode.Created, new { id = createdproduct.Id }); // Return 201 Created with the location of the new product
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception
            }
        }

        [Route("{Id:guid}/deleteProduct")]
        [HttpDelete]
        public HttpResponseMessage Deleteproduct(Guid Id)
        {
            try
            {
                // Call the product service to delete the product by ID
                var product = _productService.GetProductById(Id);
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "product not found."); // Return 404 Not Found if product doesn't exist
                }
                _productService.DeleteProduct(Id); // Delete the product
                return Request.CreateResponse(HttpStatusCode.NoContent); // Return 204 No Content on successful deletion
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception
            }
        }

        [Route("{Id:guid}/updateproduct")]
        [HttpPut]
        public HttpResponseMessage Updateproduct(Guid Id, [FromBody] Product updatedproduct)
        {
            if (updatedproduct == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Updated product cannot be null."); // Return 400 Bad Request if updated product is null
            }
            try
            {
                // Call the product service to get the existing product
                var existingproduct = _productService.GetProductById(Id);
                if (existingproduct == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "product not found."); // Return 404 Not Found if product doesn't exist
                }
                // Update the product with new details
                var product = _productService.UpdateProduct(Id, updatedproduct);
                return Request.CreateResponse(HttpStatusCode.OK, new { id = product.Id, name = product.Name, description = product.Description, price = product.Price, quantity = product.Quantity });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception
            }
        }


    }

}