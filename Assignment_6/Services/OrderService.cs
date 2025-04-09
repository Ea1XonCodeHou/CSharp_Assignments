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
            // ȷ�����ݿⴴ��
            dbContext.Database.EnsureCreated();
        }
        
        // ��ȡ���ж���
        public List<Order> GetAllOrders()
        {
            return dbContext.Orders
                .Include(o => o.Details)
                .ToList();
        }
        
        // ����ID��ȡ����
        public Order GetOrderById(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentNullException(nameof(orderId));
                
            return dbContext.Orders
                .Include(o => o.Details)
                .FirstOrDefault(o => o.OrderId == orderId);
        }
        
        // ��Ӷ���
        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
        }
        
        // ���¶���
        public void UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            var existingOrder = dbContext.Orders
                .Include(o => o.Details)
                .FirstOrDefault(o => o.OrderId == order.OrderId);
                
            if (existingOrder == null)
                throw new KeyNotFoundException($"���� {order.OrderId} ������");
                
            // ���»�����Ϣ
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.OrderDate = order.OrderDate;
            
            // ɾ�����Ƴ�����ϸ
            var detailsToRemove = existingOrder.Details
                .Where(oldDetail => !order.Details.Any(newDetail => 
                    newDetail.DetailId == oldDetail.DetailId && oldDetail.DetailId != 0))
                .ToList();
                
            foreach (var detail in detailsToRemove)
            {
                existingOrder.Details.Remove(detail);
                dbContext.OrderDetails.Remove(detail);
            }
            
            // ���»������ϸ
            foreach (var newDetail in order.Details)
            {
                var existingDetail = existingOrder.Details
                    .FirstOrDefault(d => d.DetailId == newDetail.DetailId && newDetail.DetailId != 0);
                    
                if (existingDetail != null)
                {
                    // ����������ϸ
                    existingDetail.ProductName = newDetail.ProductName;
                    existingDetail.UnitPrice = newDetail.UnitPrice;
                    existingDetail.Quantity = newDetail.Quantity;
                }
                else
                {
                    // �������ϸ
                    newDetail.OrderId = existingOrder.OrderId;
                    existingOrder.Details.Add(newDetail);
                }
            }
            
            dbContext.SaveChanges();
        }
        
        // ɾ������
        public void RemoveOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentNullException(nameof(orderId));
                
            var order = dbContext.Orders.Find(orderId);
            if (order == null)
                throw new KeyNotFoundException($"���� {orderId} ������");
                
            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();
        }
        
        // ���ݿͻ����Ʋ�ѯ
        public List<Order> QueryByCustomerName(string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
                return GetAllOrders();
                
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.CustomerName.Contains(customerName))
                .ToList();
        }
        
        // ������Ʒ���Ʋ�ѯ
        public List<Order> QueryByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return GetAllOrders();
                
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.Details.Any(d => d.ProductName.Contains(productName)))
                .ToList();
        }
        
        // ���ݽ�Χ��ѯ
        public List<Order> QueryByAmountRange(decimal min, decimal max)
        {
            return dbContext.Orders
                .Include(o => o.Details)
                .Where(o => o.Details.Sum(d => d.UnitPrice * d.Quantity) >= min && 
                           o.Details.Sum(d => d.UnitPrice * d.Quantity) <= max)
                .ToList();
        }
        
        // ���򶩵�
        public void SortOrders<TKey>(Expression<Func<Order, TKey>> keySelector)
        {
            // �����������ݿ���ͨ����ѯʵ��
        }
    }
} 