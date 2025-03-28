using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_5.common;

namespace Assignment_5.model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> Details { get; set; }
        public decimal TotalAmount => Details.Sum(d => d.Quantity * d.UnitPrice);

        public Order()
        {
            Details = new List<OrderDetail>();
        }

        public Order(int orderId, string customerName)
        {
            OrderId = orderId;
            CustomerName = customerName;
            OrderDate = DateTime.Now;
            Details = new List<OrderDetail>();
        }

        public void AddDetail(OrderDetail detail)
        {
            if (!Details.Contains(detail))
            {
                Details.Add(detail);
            }
            else
            {
                throw new ApplicationException($"OrderDetail for product {detail.ProductName} already exists in this order.");
            }
        }

        public void RemoveDetail(string productName)
        {
            OrderDetail detail = Details.FirstOrDefault(d => d.ProductName == productName);
            if (detail != null)
            {
                Details.Remove(detail);
            }
            else
            {
                throw new ApplicationException($"OrderDetail for product {productName} not found in this order.");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Order other = (Order)obj;
            return OrderId == other.OrderId;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode();
        }

        public override string ToString()
        {
            string result = $"Order ID: {OrderId}, Customer: {CustomerName}, Date: {OrderDate.ToShortDateString()}, Total Amount: {TotalAmount:C}\n";
            result += "Details:\n";
            foreach (var detail in Details)
            {
                result += $"  {detail}\n";
            }
            return result;
        }
    }
}
