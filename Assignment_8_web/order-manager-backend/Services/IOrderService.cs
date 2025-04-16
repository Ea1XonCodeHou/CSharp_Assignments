using System.Collections.Generic;
using System.Threading.Tasks;
using order_manager_backend.Models.DTO;
using order_manager_backend.Models.VO;

namespace order_manager_backend.Services
{
    public interface IOrderService
    {
        Task<OrderVO> CreateOrderAsync(OrderDTO orderDto, int userId);
        Task<OrderVO> GetOrderByIdAsync(int id);
        Task<List<OrderVO>> GetOrdersByUserAsync(int userId);
        Task<OrderVO> UpdateOrderStatusAsync(int id, string status);
        Task<bool> DeleteOrderAsync(int id);
    }
} 