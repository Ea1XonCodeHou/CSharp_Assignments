 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_5.model;
using Assignment_5.common;

namespace Assignment_5.service
{
    public class OrderService
    {
        private List<Order> orders;

        public OrderService()
        {
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
            {
                throw new ApplicationException($"Order with ID {order.OrderId} already exists.");
            }
            orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            Order order = GetOrderById(orderId);
            if (order != null)
            {
                orders.Remove(order);
            }
            else
            {
                throw new ApplicationException($"Order with ID {orderId} not found.");
            }
        }

        public void UpdateOrder(Order updatedOrder)
        {
            Order existingOrder = GetOrderById(updatedOrder.OrderId);
            if (existingOrder != null)
            {
                int index = orders.IndexOf(existingOrder);
                orders[index] = updatedOrder;
            }
            else
            {
                throw new ApplicationException($"Order with ID {updatedOrder.OrderId} not found.");
            }
        }

        public Order GetOrderById(int orderId)
        {
            return orders.FirstOrDefault(o => o.OrderId == orderId);
            //FirstOrDefault 是 LINQ 中的一个方法，它用于查找集合中符合特定条件的第一个元素。
        }

        public List<Order> QueryByCustomerName(string customerName)
        {
            return orders.Where(o => o.CustomerName.Contains(customerName))
                         .OrderBy(o => o.TotalAmount)
                         .ToList();
        }

        public List<Order> QueryByProductName(string productName)
        {
            return orders.Where(o => o.Details.Any(d => d.ProductName.Contains(productName)))
                         .OrderBy(o => o.TotalAmount)
                         .ToList();
        }

        public List<Order> QueryByAmountRange(decimal minAmount, decimal maxAmount)
        {
            return orders.Where(o => o.TotalAmount >= minAmount && o.TotalAmount <= maxAmount)
                         .OrderBy(o => o.TotalAmount)
                         .ToList();
        }

        public List<Order> GetAllOrders()
        {
            return new List<Order>(orders);
        }

        // Default sort by OrderId
        public void SortOrders()
        {
            orders = orders.OrderBy(o => o.OrderId).ToList();
        }

        // Custom sort using Lambda expression
        public void SortOrders(Func<Order, object> keySelector)
        {
            orders = orders.OrderBy(keySelector).ToList();
        }
    }
} 