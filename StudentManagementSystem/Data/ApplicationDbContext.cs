using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System.Linq; // Make sure this is present for .Any()

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

        // ADD THIS SEED METHOD BELOW
        // ADD THIS SEED METHOD BELOW
        public void SeedAdminUser()
        {
            // Clear any lingering tracking and look directly at the database using C# PascalCase properties
            if (!tblStudent.Any(u => u.Username == "tprateek01"))
            {
                var admin = new Student
                {
                    // Use the exact Capitalized properties your C# Student model defines
                    Name = "System Administrator",
                    Username = "tprateek01",
                    Role = "Admin",
                    // This hashes 'admin123' using BCrypt to match your AccountController verification logic
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123")
                };

                tblStudent.Add(admin);
                SaveChanges();
            }
        }
    }
}