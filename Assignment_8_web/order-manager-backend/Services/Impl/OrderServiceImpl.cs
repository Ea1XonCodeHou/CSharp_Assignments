using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using order_manager_backend.Models;
using order_manager_backend.Models.DTO;
using order_manager_backend.Models.VO;
using order_manager_backend.Services;

namespace order_manager_backend.Services.Impl
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderServiceImpl(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderVO> CreateOrderAsync(OrderDTO orderDto, int userId)
        {
            // 生成订单号
            string orderNumber = GenerateOrderNumber();

            // 创建订单
            var order = new Order
            {
                OrderNumber = orderNumber,
                CustomerName = orderDto.CustomerName,
                TotalAmount = orderDto.TotalAmount,
                Status = "pending",
                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 创建订单项
            foreach (var item in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    CreatedAt = DateTime.Now
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<OrderVO> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null;

            return MapToOrderVO(order);
        }

        public async Task<List<OrderVO>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Where(o => o.CreatedBy == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(MapToOrderVO).ToList();
        }

        public async Task<OrderVO> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return null;

            order.Status = status;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(id);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        private string GenerateOrderNumber()
        {
            // 生成订单号：时间戳 + 随机数
            return DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999).ToString();
        }

        private OrderVO MapToOrderVO(Order order)
        {
            return new OrderVO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedBy = order.User?.Username,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.OrderItems.Select(item => new OrderItemVO
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    CreatedAt = item.CreatedAt
                }).ToList()
            };
        }
    }
} 