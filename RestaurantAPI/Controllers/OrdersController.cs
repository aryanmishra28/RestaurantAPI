using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.DTOs;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // ⭐ PLACE ORDER
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var customer = await _context.Customers.FindAsync(dto.CustomerId);

            if (customer == null)
                return BadRequest("Invalid Customer");

            decimal total = 0;

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Items)
            {
                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);

                if (menuItem == null)
                    return BadRequest("Invalid Menu Item");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    Price = menuItem.Price
                };

                total += menuItem.Price * item.Quantity;

                _context.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount
            });
        }

        // ⭐ UPDATE ORDER STATUS (ADMIN)
        [HttpPut("status/{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound("Order not found");

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // ⭐ GET CUSTOMER ORDER HISTORY
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(int customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return Ok(orders);
        }

        // ⭐ GET ALL ORDERS (ADMIN DASHBOARD)
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            return Ok(orders);
        }
    }
}