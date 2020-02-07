using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaaSApi.Data.Framework;

namespace SaaSApi.Data.Entities
{
    public class Login : PersistedModel<int>
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }





        public User User { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }





    }
}