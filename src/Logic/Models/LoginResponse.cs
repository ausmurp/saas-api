namespace SaaSApi.Logic.Models
{
    public class LoginResponse
    {
        public AuthSession Session { get; set; }

        public UserDto User { get; set; }
    }
}