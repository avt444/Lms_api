using Microsoft.EntityFrameworkCore;
using WebApplication5.Models.Entities;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<TaskEntity> COURSEDETAILS { get; set; }

        

        
    }

}
