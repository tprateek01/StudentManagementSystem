using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard(string searchString, string deptSearch)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var studentsQuery = _context.tblStudent.Include(s => s.Department).Where(s => s.Role == "Student");

            if (!string.IsNullOrEmpty(searchString))
            {
                studentsQuery = studentsQuery.Where(s => s.Username.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(deptSearch))
            {
                studentsQuery = studentsQuery.Where(s => s.Department.DepartmentName.Contains(deptSearch));
            }

            var studentsList = studentsQuery.ToList();

            ViewBag.TotalStudents = _context.tblStudent.Count(s => s.Role == "Student");
            ViewBag.TotalDepartments = _context.tblDepartment.Count();
            ViewBag.AvgCgpa = _context.tblStudent.Where(s => s.Role == "Student" && s.CGPA.HasValue).Average(s => (double?)s.CGPA) ?? 0.0;

            return View(studentsList);
        }

        // GET: Admin/EditStudent/5
        public IActionResult EditStudent(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var student = _context.tblStudent.Find(id);
            if (student == null) return NotFound();

            ViewBag.Departments = new SelectList(_context.tblDepartment, "DepartmentId", "DepartmentName", student.DepartmentId);
            return View(student);
        }

        // POST: Admin/EditStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Student model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var student = _context.tblStudent.Find(model.Id);
            if (student != null)
            {
                student.Name = model.Name;
                student.Username = model.Username;
                student.Course = model.Course;
                student.Semester = model.Semester;
                student.CGPA = model.CGPA;
                student.SGPA = model.SGPA;
                student.DepartmentId = model.DepartmentId;

                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            ViewBag.Departments = new SelectList(_context.tblDepartment, "DepartmentId", "DepartmentName", model.DepartmentId);
            return View(model);
        }

        // GET: Admin/DeleteStudent/5
        public IActionResult DeleteStudent(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var student = _context.tblStudent.Find(id);
            if (student != null)
            {
                _context.tblStudent.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // GET: Admin/ViewAdmins
        public IActionResult ViewAdmins()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var admins = _context.tblStudent.Where(s => s.Role == "Admin").ToList();
            return View(admins);
        }

        // GET: Admin/ManageDepartments
        public IActionResult ManageDepartments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var departments = _context.tblDepartment.ToList();
            return View(departments);
        }

        // POST: Admin/AddDepartment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDepartment(string departmentName)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrEmpty(departmentName))
            {
                var dept = new Department { DepartmentName = departmentName };
                _context.tblDepartment.Add(dept);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageDepartments");
        }

        // GET: Admin/DeleteDepartment/5
        public IActionResult DeleteDepartment(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var dept = _context.tblDepartment.Find(id);
            if (dept != null)
            {
                _context.tblDepartment.Remove(dept);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageDepartments");
        }
    }
}