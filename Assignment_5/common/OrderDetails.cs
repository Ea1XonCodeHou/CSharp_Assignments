using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_5.common
{
    public class OrderDetail
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public OrderDetail() { }

        public OrderDetail(string productName, decimal unitPrice, int quantity)
        {
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            OrderDetail other = (OrderDetail)obj;
            return ProductName == other.ProductName;
        }

        public override int GetHashCode()
        {
            return ProductName.GetHashCode();
        }

        public override string ToString()
        {
            return $"Product: {ProductName}, Price: {UnitPrice:C}, Quantity: {Quantity}, Subtotal: {UnitPrice * Quantity:C}";
        }
    }
}
