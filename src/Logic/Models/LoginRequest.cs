using System.ComponentModel.DataAnnotations;

namespace SaaSApi.Logic.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberDevice { get; set; }
    }
}