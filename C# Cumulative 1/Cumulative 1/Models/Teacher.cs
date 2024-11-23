using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string? TeacherFName { get; set; }

        public string? TeacherLName { get; set; }

        public DateTime TeacherHireDate { get; set; }

        public string? TeacherSalary { get; set; }

        public string? TeacherEmpNu { get; set; }

        public List<string> CourseNames { get; set; } = new List<string>();
    }
}