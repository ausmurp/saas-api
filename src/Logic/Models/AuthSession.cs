namespace SaaSApi.Logic.Models
{
    public class AuthSession
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public long ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}