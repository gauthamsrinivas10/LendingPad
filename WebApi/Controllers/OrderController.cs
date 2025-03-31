using BusinessEntities;
using Core.Services.Orders;
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
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService), "Order service cannot be null.");
        }

        [HttpGet]
        [Route("{id:guid}")]
        public HttpResponseMessage GetOrderbyId(Guid id)
        {
            try
            {
                // Retrieve all orders from the order service  
                var order = _orderService.GetOrderById(id);
                if (order == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound); // Return 404 if no orders found  
                }
                return Request.CreateResponse(HttpStatusCode.OK, order); // Return 200 OK with the list of orders  
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception  
            }
        }

        [HttpGet]
        [Route("GetAllOrders")]
        public HttpResponseMessage GetAllOrders()
        {
            try
            {
                // Retrieve all orders from the order service  
                var orders = _orderService.GetAllOrders();
                if (orders == null || !orders.Any())
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No orders found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, orders);//Returns with list of orders  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occurred while fetching orders with following Trace details {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetOrderByName")]
        public HttpResponseMessage GetOrderByName(string filter)
        {
            try
            {
                // Retrieve all orders from the order service  
                var orders = _orderService.GetOrderByName(filter);
                if (orders == null || !orders.Any())
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No orders found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, orders);//Returns with list of orders  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occurred while fetching orders with following Trace details {ex.Message}");
            }
        }
        [Route("createOrder")]
        [HttpPost]
        public HttpResponseMessage CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Order cannot be null."); // Return 400 Bad Request if order is null  
            }
            try
            {
                // Call the order service to create a new order  
                var createdOrder = _orderService.CreateOrder(order);                

                return Request.CreateResponse(HttpStatusCode.Created, new { id = createdOrder.Id }); // Return 201 Created with the location of the new order  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception  
            }
        }

        [Route("{orderId:guid}/deleteOrder")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(Guid orderId)
        {
            try
            {
                // Call the order service to delete the order by ID  
                var order = _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found."); // Return 404 Not Found if order doesn't exist  
                }
                _orderService.DeleteOrder(orderId); // Delete the order  
                return Request.CreateResponse(HttpStatusCode.NoContent); // Return 204 No Content on successful deletion  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception  
            }
        }

        [Route("{orderId:guid}/updateOrder")]
        [HttpPut]
        public HttpResponseMessage UpdateOrder(Guid orderId, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Updated order cannot be null."); // Return 400 Bad Request if updated order is null  
            }
            try
            {
                // Call the order service to get the existing order  
                var existingOrder = _orderService.GetOrderById(orderId);
                if (existingOrder == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found."); // Return 404 Not Found if order doesn't exist  
                }
                // Update the order with new details  
                var order = _orderService.UpdateOrder(orderId, updatedOrder);
                return Request.CreateResponse(HttpStatusCode.OK, new { id = order.Id }); // Return 200 OK with the updated order ID  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception  
            }
        }

        [Route("createMultipleOrder")]
        [HttpPost]
        public HttpResponseMessage CreateMultipleOrder([FromBody] IEnumerable<Order> order)
        {
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Order cannot be null."); // Return 400 Bad Request if order is null  
            }
            try
            {
                // Call the order service to create a new order  
                _orderService.AddOrders(order);

                return Request.CreateResponse(HttpStatusCode.Created, "Orders placed successfully"); // Return 201 Created with the location of the new order  
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); // Return 500 Internal Server Error on exception  
            }
        }

        [Route("GetTotalCostOfAllOrders")]
        [HttpGet]
        public HttpResponseMessage GetTotalCostOfAllOrders()
        {
            try
            {
                // Retrieve Total cost
                var total = _orderService.CalculateTotalCostOfAllOrders();

                return Request.CreateResponse(HttpStatusCode.OK, $"Total cost of Multiple orders is {total}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occurred while fetching orders with following Trace details {ex.Message}");
            }
        }
    }

}