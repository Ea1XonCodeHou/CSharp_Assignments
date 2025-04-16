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
            // ����û����Ƿ��Ѵ���
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDTO.Username);

            if (existingUser != null)
            {
                return Result<UserVO>.Fail("�û����Ѵ���");
            }

            // �������û�
            var user = new User
            {
                Username = registerDTO.Username,
                Password = registerDTO.Password, // �򻯣�ֱ�Ӵ洢���룬ʵ����Ŀ��Ӧ�ü���
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // ���ɼ򵥵�token
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
            // �����û�
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null || user.Password != loginDTO.Password) // �򻯣�ֱ�ӱȽ����룬ʵ����Ŀ��Ӧ��ʹ�ü��ܱȽ�
            {
                return Result<UserVO>.Fail("�û������������");
            }

            // ���ɼ򵥵�token
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
            // �򻯣�ֱ�ӷ���һ���򵥵�token�ַ���
            return $"demo-token-{user.Id}-{user.Username}";
        }
    }
} 