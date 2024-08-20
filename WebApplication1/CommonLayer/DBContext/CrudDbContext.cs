using Microsoft.EntityFrameworkCore;
using WebApplication1.CommonLayer.Models;
namespace WebApplication1.CommonLayer.DBContext
{
    public class CrudDbContext:DbContext
    {
        public DbSet<AddInformationModel> Students { get; set; }
        public DbSet<EmployeeInfo> Table { get; set; }

        public CrudDbContext(DbContextOptions<CrudDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DemoDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example of configuring a primary key
            modelBuilder.Entity<AddInformationModel>()
                .HasKey(c => c.EmailId);
   
            
            modelBuilder.Entity<EmployeeInfo>().HasKey(c=>c.empId);
            // Further model configuration can go here
        }
    }
}
