using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Assignment_6.Models;
using Assignment_6.Data;

namespace Assignment_6.Services
{
    public class OrderService
    {
        private readonly OrderDbContext dbContext;
        
        public OrderService()
        {
            dbContext = new OrderDbContext();
            // 确保数据库创建
            dbContext.Database.EnsureCreated();
        }
        
        // 获取所有订单
        public List<Order> GetAllOrders()
        {
            return dbContext.Orders
                .Include(o => o.Details)
                .ToList();
        }
        
        // 根据ID获取订单
        public Order GetOrderById(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentNullException(nameof(orderId));
                
            return dbContext.Orders
                .Include(o => o.Details)
                .FirstOrDefault(o => o.OrderId == orderId);
        }
        
        // 添加订单
        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
        }
        
        // 更新订单
        public void UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            var existingOrder = dbContext.Orders
                .Include(o => o.Details)
                .FirstOrDefault(o => o.OrderId == order.OrderId);
                
            if (existingOrder == null)
                throw new KeyNotFoundException($"订单 {order.OrderId} 不存在");
                
            // 更新基本信息
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.OrderDate = order.OrderDate;
            
            // 删除已移除的明细
            var detailsToRemove = existingOrder.Details
                .Where(oldDetail => !order.Details.Any(newDetail => 
                    newDetail.DetailId == oldDetail.DetailId && oldDetail.DetailId != 0))
                .ToList();
                
            foreach (var detail in detailsToRemove)
            {
                existingOrder.Details.Remove(detail);
                dbContext.OrderDetails.Remove(detail);
            }
            
            // 更新或添加明细
            foreach (var newDetail in order.Details)
            {
                var existingDetail = existingOrder.Details
                    .FirstOrDefault(d => d.DetailId == newDetail.DetailId && newDetail.DetailId != 0);
                    
                if (existingDetail != null)
                {
                    // 更新现有明细
                    existingDetail.ProductName = newDetail.ProductName;
                    existingDetail.UnitPrice = newDetail.UnitPrice;
                    existingDetail.Quantity = newDetail.Quantity;
                }
                else
                {
                    // 添加新明细
                    newDetail.OrderId = existingOrder.OrderId;
                    existingOrder.Details.Add(newDetail);
                }
            }
            
            dbContext.SaveChanges();
        }
        
        // 删除订单
        public void RemoveOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentNullException(nameof(orderId));
                
            var order = dbContext.Orders.Find(orderId);
            if (order == null)
                throw new KeyNotFoundException($"订单 {orderId} 不存在");
                
            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();
        }
        
        // 根据客户名称查询
        public List<Order> QueryByCustomerName(string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
                return GetAllOrders();
                
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.CustomerName.Contains(customerName))
                .ToList();
        }
        
        // 根据商品名称查询
        public List<Order> QueryByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return GetAllOrders();
                
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.Details.Any(d => d.ProductName.Contains(productName)))
                .ToList();
        }
        
        // 根据金额范围查询
        public List<Order> QueryByAmountRange(decimal min, decimal max)
        {
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.Details.Sum(d => d.UnitPrice * d.Quantity) >= min && 
                           o.Details.Sum(d => d.UnitPrice * d.Quantity) <= max)
                .ToList();
        }
        
        // 排序订单
        public void SortOrders<TKey>(Expression<Func<Order, TKey>> keySelector)
        {
            // 排序功能在数据库中通过查询实现
        }
    }
} 