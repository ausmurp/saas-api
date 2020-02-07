using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    public class User : PersistedModel<int>
    {
        public User()
        {
            Logins = new List<Login>();
        }

        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(500)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string Phone { get; set; }
        public string ImageUrl { get; set; }







        public ICollection<Login> Logins { get; set; }






    }
}