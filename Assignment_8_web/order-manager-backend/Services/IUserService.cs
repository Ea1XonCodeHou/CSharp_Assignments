using order_manager_backend.Common;
using order_manager_backend.Models.DTO;
using order_manager_backend.Models.VO;

namespace order_manager_backend.Services
{
    public interface IUserService
    {
        Task<Result<UserVO>> Register(UserRegisterDTO registerDTO);
        Task<Result<UserVO>> Login(UserLoginDTO loginDTO);
    }
} 