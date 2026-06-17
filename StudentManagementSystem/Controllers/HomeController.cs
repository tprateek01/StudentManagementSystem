using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Collections.Generic; // CRITICAL: Added for List<> to avoid compiler errors!
using System.Diagnostics;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Single execution link to populate your live cloud database completely
        public string CreateAdminAccount()
        {
            System.Text.StringBuilder statusMessage = new System.Text.StringBuilder();

            // 1. Seed Departments if they are currently missing
            if (!_context.tblDepartment.Any())
            {
                var departments = new List<Department>
                {
                    new Department { DepartmentName = "Computer Science" },
                    new Department { DepartmentName = "Information Technology" },
                    new Department { DepartmentName = "Electronics Engineering" },
                    new Department { DepartmentName = "Mechanical Engineering" }
                };
                _context.tblDepartment.AddRange(departments);
                _context.SaveChanges();
                statusMessage.AppendLine("Departments populated successfully! | ");
            }
            else
            {
                statusMessage.AppendLine("Departments already exist. | ");
            }

            // 2. Seed Admin Account if it doesn't exist
            var adminExists = _context.tblStudent.Any(u => u.Username == "tprateek01");
            if (!adminExists)
            {
                var admin = new Student
                {
                    Name = "System Administrator",
                    Username = "tprateek01",
                    Role = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123")
                };

                _context.tblStudent.Add(admin);
                _context.SaveChanges();
                statusMessage.AppendLine("Admin account 'tprateek01' created successfully!");
            }
            else
            {
                statusMessage.AppendLine("Admin account already exists.");
            }

            return statusMessage.ToString();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}