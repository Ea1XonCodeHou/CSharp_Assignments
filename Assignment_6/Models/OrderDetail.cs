using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_6.Models
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        
        public string ProductName { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public int Quantity { get; set; }
        
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        
        public Order Order { get; set; }
        
        public OrderDetail() { }
        
        public OrderDetail(string productName, decimal unitPrice, int quantity)
        {
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            
            if (unitPrice <= 0)
                throw new ArgumentException("单价必须大于0", nameof(unitPrice));
            UnitPrice = unitPrice;
            
            if (quantity <= 0)
                throw new ArgumentException("数量必须大于0", nameof(quantity));
            Quantity = quantity;
        }
    }
} 