using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> tblStudent { get; set; }
        public DbSet<Department> tblDepartment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Force Entity Framework to look for lowercase tables and columns
            modelBuilder.Entity<Student>().ToTable("tblstudent");
            modelBuilder.Entity<Department>().ToTable("tbldepartment");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    // Converts column mappings to lowercase dynamically
                    property.SetColumnName(property.Name.ToLower());
                }
            }
        }
    }
}