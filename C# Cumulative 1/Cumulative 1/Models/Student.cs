using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string? StudentFName { get; set; }

        public string? StudentLName { get; set; }

        public DateTime EnrollDate { get; set; }

        public string? StudentNumber { get; set; }

    }
}
