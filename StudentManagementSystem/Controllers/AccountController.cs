using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            // Seed departments select list data for the registration form dropdown
            ViewBag.Departments = new SelectList(_context.tblDepartment, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Student student, string ConfirmPassword)
        {
            // 1. Validation Check: Check if Username already exists in the system
            if (_context.tblStudent.Any(u => u.Username == student.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
            }

            // 2. Validation Check: Confirm Passwords match perfectly
            if (student.Password != ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Passwords do not match.");
            }

            // If all validation rules pass, save the student to PostgreSQL database
            if (ModelState.IsValid)
            {
                // Securely hash the password string using BCrypt
                student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
                student.Role = "Student"; // Self-registered accounts default to the Student role

                _context.tblStudent.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            // If we reached here, validation failed. Reload the department list data so the dropdown doesn't crash!
            ViewBag.Departments = new SelectList(_context.tblDepartment, "DepartmentId", "DepartmentName", student.DepartmentId);
            return View(student);
        }

        // GET: Account/Login
        public IActionResult Login() => View();

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _context.tblStudent.FirstOrDefault(u => u.Username == username);

            // Validate password matching utilizing the BCrypt verifier tool
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Establish unique HttpContext Session contexts to secure portal paths
                HttpContext.Session.SetString("UserRole", user.Role ?? "Student");
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);

                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else
                    return RedirectToAction("Profile", "Student");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            // Clear current login active session records entirely
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}