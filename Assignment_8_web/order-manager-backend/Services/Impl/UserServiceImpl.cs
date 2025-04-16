using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using order_manager_backend.Common;
using order_manager_backend.Models;
using order_manager_backend.Models.DTO;
using order_manager_backend.Models.VO;

namespace order_manager_backend.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserServiceImpl(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Result<UserVO>> Register(UserRegisterDTO registerDTO)
        {
            // 检查用户名是否已存在
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDTO.Username);

            if (existingUser != null)
            {
                return Result<UserVO>.Fail("用户名已存在");
            }

            // 创建新用户
            var user = new User
            {
                Username = registerDTO.Username,
                Password = registerDTO.Password, // 简化：直接存储密码，实际项目中应该加密
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // 生成简单的token
            var token = GenerateSimpleToken(user);

            return Result<UserVO>.Success(new UserVO
            {
                Id = user.Id,
                Username = user.Username,
                Token = token
            });
        }

        public async Task<Result<UserVO>> Login(UserLoginDTO loginDTO)
        {
            // 查找用户
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null || user.Password != loginDTO.Password) // 简化：直接比较密码，实际项目中应该使用加密比较
            {
                return Result<UserVO>.Fail("用户名或密码错误");
            }

            // 生成简单的token
            var token = GenerateSimpleToken(user);

            return Result<UserVO>.Success(new UserVO
            {
                Id = user.Id,
                Username = user.Username,
                Token = token
            });
        }

        private string GenerateSimpleToken(User user)
        {
            // 简化：直接返回一个简单的token字符串
            return $"demo-token-{user.Id}-{user.Username}";
        }
    }
} 