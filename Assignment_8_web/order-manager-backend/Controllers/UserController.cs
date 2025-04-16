using Microsoft.AspNetCore.Mvc;
using order_manager_backend.Common;
using order_manager_backend.Models.DTO;
using order_manager_backend.Models.VO;
using order_manager_backend.Services;

namespace order_manager_backend.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<Result<UserVO>> Register([FromBody] UserRegisterDTO registerDTO)
        {
            return await _userService.Register(registerDTO);
        }

        [HttpPost("login")]
        public async Task<Result<UserVO>> Login([FromBody] UserLoginDTO loginDTO)
        {
            return await _userService.Login(loginDTO);
        }
    }
} 