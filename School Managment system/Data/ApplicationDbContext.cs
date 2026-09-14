using Microsoft.EntityFrameworkCore;
using School_Managment_system.Models;

namespace School_Managment_system.Data
{ 

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> StudentData { get; set; }

        public DbSet<Cards> Carddata { get; set; }


    }

}