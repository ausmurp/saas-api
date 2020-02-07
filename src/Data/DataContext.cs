using Microsoft.EntityFrameworkCore;
using SaaSApi.Data.Entities;

namespace SaaSApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }

    }
}