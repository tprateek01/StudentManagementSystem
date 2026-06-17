using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student/Profile
        public IActionResult Profile()
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return RedirectToAction("Login", "Account");

            var student = _context.tblStudent.Find(currentUserId);
            return View(student);
        }

        // POST: Student/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(Student updatedData)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return RedirectToAction("Login", "Account");

            var databaseRecord = _context.tblStudent.Find(currentUserId);
            if (databaseRecord != null)
            {
                // Assign permitted student editing scope
                databaseRecord.Course = updatedData.Course;
                databaseRecord.Semester = updatedData.Semester;
                databaseRecord.CGPA = updatedData.CGPA;
                databaseRecord.SGPA = updatedData.SGPA;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            return RedirectToAction("Profile");
        }
    }
}