using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_5.model;
using Assignment_5.common;
using Assignment_5.service;

namespace Assignment_5
{
    class Program
    {
        private static OrderService orderService = new OrderService();
        private static int nextOrderId = 1;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Add some sample data
            InitializeSampleData();

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddOrder();
                            break;
                        case "2":
                            DeleteOrder();
                            break;
                        case "3":
                            ModifyOrder();
                            break;
                        case "4":
                            QueryOrders();
                            break;
                        case "5":
                            DisplayAllOrders();
                            break;
                        case "6":
                            SortOrders();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("无效选择，请重试。");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"操作失败: {ex.Message}");
                }

                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("===== 订单管理系统 =====");
            Console.WriteLine("1. 添加订单");
            Console.WriteLine("2. 删除订单");
            Console.WriteLine("3. 修改订单");
            Console.WriteLine("4. 查询订单");
            Console.WriteLine("5. 显示所有订单");
            Console.WriteLine("6. 订单排序");
            Console.WriteLine("0. 退出");
            Console.Write("请选择操作: ");
        }

        private static void AddOrder()
        {
            Console.WriteLine("\n===== 添加订单 =====");
            
            Console.Write("客户名称: ");
            string customerName = Console.ReadLine();

            Order order = new Order(nextOrderId++, customerName);

            bool addingDetails = true;
            while (addingDetails)
            {
                try
                {
                    Console.WriteLine("\n添加订单明细:");
                    
                    Console.Write("商品名称: ");
                    string productName = Console.ReadLine();
                    
                    Console.Write("单价: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal unitPrice) || unitPrice <= 0)
                    {
                        Console.WriteLine("无效的单价，请输入一个正数。");
                        continue;
                    }
                    
                    Console.Write("数量: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                    {
                        Console.WriteLine("无效的数量，请输入一个正整数。");
                        continue;
                    }
                    
                    OrderDetail detail = new OrderDetail(productName, unitPrice, quantity);
                    order.AddDetail(detail);
                    
                    Console.Write("是否继续添加订单明细? (y/n): ");
                    addingDetails = Console.ReadLine().ToLower() == "y";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"添加订单明细失败: {ex.Message}");
                }
            }

            orderService.AddOrder(order);
            Console.WriteLine($"订单添加成功，订单号: {order.OrderId}");
        }

        private static void DeleteOrder()
        {
            Console.WriteLine("\n===== 删除订单 =====");
            
            Console.Write("请输入要删除的订单号: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("无效的订单号。");
                return;
            }

            orderService.RemoveOrder(orderId);
            Console.WriteLine($"订单 {orderId} 已成功删除。");
        }

        private static void ModifyOrder()
        {
            Console.WriteLine("\n===== 修改订单 =====");
            
            Console.Write("请输入要修改的订单号: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("无效的订单号。");
                return;
            }

            Order order = orderService.GetOrderById(orderId);
            if (order == null)
            {
                throw new ApplicationException($"订单号 {orderId} 不存在。");
            }

            Console.WriteLine(order);

            Console.Write("新客户名称 (留空保持不变): ");
            string customerName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                order.CustomerName = customerName;
            }

            bool modifyingDetails = true;
            while (modifyingDetails)
            {
                Console.WriteLine("\n订单明细操作:");
                Console.WriteLine("1. 添加明细");
                Console.WriteLine("2. 删除明细");
                Console.WriteLine("3. 完成修改");
                Console.Write("请选择: ");
                
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddDetailToOrder(order);
                        break;
                    case "2":
                        RemoveDetailFromOrder(order);
                        break;
                    case "3":
                        modifyingDetails = false;
                        break;
                    default:
                        Console.WriteLine("无效选择。");
                        break;
                }
            }

            orderService.UpdateOrder(order);
            Console.WriteLine("订单修改成功。");
        }

        private static void AddDetailToOrder(Order order)
        {
            try
            {
                Console.Write("商品名称: ");
                string productName = Console.ReadLine();
                
                Console.Write("单价: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal unitPrice) || unitPrice <= 0)
                {
                    Console.WriteLine("无效的单价，请输入一个正数。");
                    return;
                }
                
                Console.Write("数量: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("无效的数量，请输入一个正整数。");
                    return;
                }
                
                OrderDetail detail = new OrderDetail(productName, unitPrice, quantity);
                order.AddDetail(detail);
                Console.WriteLine("明细添加成功。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加明细失败: {ex.Message}");
            }
        }

        private static void RemoveDetailFromOrder(Order order)
        {
            try
            {
                Console.Write("请输入要删除的商品名称: ");
                string productName = Console.ReadLine();
                
                order.RemoveDetail(productName);
                Console.WriteLine("明细删除成功。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除明细失败: {ex.Message}");
            }
        }

        private static void QueryOrders()
        {
            Console.WriteLine("\n===== 查询订单 =====");
            Console.WriteLine("1. 按客户名称查询");
            Console.WriteLine("2. 按商品名称查询");
            Console.WriteLine("3. 按订单金额范围查询");
            Console.Write("请选择查询方式: ");
            
            string choice = Console.ReadLine();
            List<Order> results = new List<Order>();
            
            switch (choice)
            {
                case "1":
                    Console.Write("请输入客户名称: ");
                    string customerName = Console.ReadLine();
                    results = orderService.QueryByCustomerName(customerName);
                    break;
                case "2":
                    Console.Write("请输入商品名称: ");
                    string productName = Console.ReadLine();
                    results = orderService.QueryByProductName(productName);
                    break;
                case "3":
                    Console.Write("请输入最小金额: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal minAmount) || minAmount < 0)
                    {
                        Console.WriteLine("无效的金额，请输入一个非负数。");
                        return;
                    }
                    
                    Console.Write("请输入最大金额: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal maxAmount) || maxAmount < minAmount)
                    {
                        Console.WriteLine("无效的金额，最大金额必须大于或等于最小金额。");
                        return;
                    }
                    
                    results = orderService.QueryByAmountRange(minAmount, maxAmount);
                    break;
                default:
                    Console.WriteLine("无效的选择。");
                    return;
            }
            
            DisplayOrdersList(results);
        }
        
        private static void DisplayAllOrders()
        {
            Console.WriteLine("\n===== 所有订单 =====");
            List<Order> allOrders = orderService.GetAllOrders();
            DisplayOrdersList(allOrders);
        }
        
        private static void SortOrders()
        {
            Console.WriteLine("\n===== 订单排序 =====");
            Console.WriteLine("1. 按订单号排序");
            Console.WriteLine("2. 按客户名称排序");
            Console.WriteLine("3. 按订单金额排序");
            Console.Write("请选择排序方式: ");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    orderService.SortOrders(o => o.OrderId);
                    Console.WriteLine("已按订单号排序。");
                    break;
                case "2":
                    orderService.SortOrders(o => o.CustomerName);
                    Console.WriteLine("已按客户名称排序。");
                    break;
                case "3":
                    orderService.SortOrders(o => o.TotalAmount);
                    Console.WriteLine("已按订单金额排序。");
                    break;
                default:
                    Console.WriteLine("无效的选择。");
                    return;
            }
            
            DisplayAllOrders();
        }
        
        private static void DisplayOrdersList(List<Order> orders)
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("没有找到符合条件的订单。");
                return;
            }
            
            Console.WriteLine($"找到 {orders.Count} 个订单：\n");
            foreach (Order order in orders)
            {
                Console.WriteLine(order);
                Console.WriteLine(new string('-', 40));
            }
        }

        private static void InitializeSampleData()
        {
            // Sample order 1 - 数码产品
            Order order1 = new Order(nextOrderId++, "李明");
            order1.AddDetail(new OrderDetail("iPhone 13 Pro", 7999.00m, 1));
            order1.AddDetail(new OrderDetail("AirPods Pro", 1999.00m, 1));
            order1.AddDetail(new OrderDetail("iPhone 13 保护壳", 299.00m, 2));
            orderService.AddOrder(order1);

            // Sample order 2 - 数码产品
            Order order2 = new Order(nextOrderId++, "张华");
            order2.AddDetail(new OrderDetail("MacBook Pro 14", 14999.00m, 1));
            order2.AddDetail(new OrderDetail("Magic Mouse", 699.00m, 1));
            order2.AddDetail(new OrderDetail("USB-C转接器", 199.00m, 2));
            orderService.AddOrder(order2);

            // Sample order 3 - 数码产品
            Order order3 = new Order(nextOrderId++, "王芳");
            order3.AddDetail(new OrderDetail("iPad Pro 11", 6799.00m, 1));
            order3.AddDetail(new OrderDetail("Apple Pencil", 999.00m, 1));
            order3.AddDetail(new OrderDetail("iPad 保护套", 399.00m, 1));
            orderService.AddOrder(order3);

            // Sample order 4 - 数码产品
            Order order4 = new Order(nextOrderId++, "赵伟");
            order4.AddDetail(new OrderDetail("华为MateBook X Pro", 9999.00m, 1));
            order4.AddDetail(new OrderDetail("华为无线鼠标", 199.00m, 1));
            order4.AddDetail(new OrderDetail("Type-C数据线", 59.00m, 3));
            order4.AddDetail(new OrderDetail("笔记本背包", 399.00m, 1));
            orderService.AddOrder(order4);

            // Sample order 5 - 数码产品
            Order order5 = new Order(nextOrderId++, "陈丽");
            order5.AddDetail(new OrderDetail("小米12 Pro", 4999.00m, 1));
            order5.AddDetail(new OrderDetail("小米手环7", 249.00m, 1));
            order5.AddDetail(new OrderDetail("20W快充", 99.00m, 2));
            order5.AddDetail(new OrderDetail("手机钢化膜", 49.00m, 3));
            orderService.AddOrder(order5);
        }
    }
}