using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBDA.Models.Models;

namespace SBDA.API.DBContext
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Member> Members { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<Designation> Designations { get; set; }
    }
}
