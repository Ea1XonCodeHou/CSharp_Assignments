namespace order_manager_backend.Models.DTO
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
} 