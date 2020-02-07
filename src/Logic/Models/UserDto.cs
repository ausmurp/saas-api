using System.ComponentModel.DataAnnotations;

namespace SaaSApi.Logic.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(500)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string Phone { get; set; }
        public string ImageUrl { get; set; }






    }
}