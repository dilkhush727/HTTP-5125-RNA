using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Models
{
    public class TeacherCoursesViewModel : Controller
    {
        public Teacher Teacher { get; set; }
        public List<string> Courses { get; set; }
    }
}
