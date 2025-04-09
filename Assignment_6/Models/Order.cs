using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Assignment_6.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        
        public string CustomerName { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        [NotMapped]
        public decimal TotalAmount => Details?.Sum(d => d.UnitPrice * d.Quantity) ?? 0;
        
        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();
        
        public Order()
        {
            OrderId = Guid.NewGuid().ToString().Substring(0, 8);
            OrderDate = DateTime.Now;
        }
        
        public void AddDetail(OrderDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));
                
            Details.Add(detail);
        }
        
        public void RemoveDetail(OrderDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));
                
            Details.Remove(detail);
        }
    }
} 