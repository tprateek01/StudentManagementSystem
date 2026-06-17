using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data; // Added for ApplicationDbContext
using StudentManagementSystem.Models;
using System.Diagnostics;
using System.Linq; // Added for .Any()

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor injection to access your live Render database
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Fresh standalone URL route to insert your admin record manually
        public string CreateAdminAccount()
        {
            var exists = _context.tblStudent.Any(u => u.Username == "tprateek01");
            if (!exists)
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
                return "Admin account 'tprateek01' with password 'admin123' created successfully!";
            }
            return "Admin account already exists in database.";
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