using System.ComponentModel.DataAnnotations;

namespace SaaSApi.Logic.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}