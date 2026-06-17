using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = "Student";

        public string? Course { get; set; }
        public int? Semester { get; set; }
        public decimal? CGPA { get; set; }
        public decimal? SGPA { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public string? Hometown { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Must be a valid 10-digit Indian mobile number.")]
        public string? Mobile { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}