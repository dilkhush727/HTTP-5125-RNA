using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string? TeacherFName { get; set; }

        public string? TeacherLName { get; set; }

        public DateTime TeacherHireDate { get; set; }

        public Decimal? TeacherSalary { get; set; }

        public string? TeacherEmpNu { get; set; }


    }
}
