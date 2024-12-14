using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        // Constructor to initialize the Course API controller
        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        // Displays a list of all courses
        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }

        // Displays the details of a specific course
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // Displays the form to create a new course
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // Handles the creation of a new course
        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int courseId = _api.AddCourse(NewCourse);

            // Redirects to the "Show" action with the ID of the newly created course
            return RedirectToAction("Show", new { id = courseId });
        }

        // Displays a confirmation view to delete a specific course
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // Handles the deletion of a course
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int courseId=_api.DeleteCourse(id);

            // Redirects to the "List" action after successful deletion
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Fetch the course using the API
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Update(int id, string CourseCode, int TeacherId, DateTime StartDate, DateTime FinishDate, string CourseName)
        {
            Course UpdatedCourse = new Course();

            UpdatedCourse.coursecode = CourseCode;
            UpdatedCourse.teacherid = TeacherId;
            UpdatedCourse.startdate = StartDate;
            UpdatedCourse.finishdate = FinishDate;
            UpdatedCourse.coursename = CourseName;
            
            // Update the course using the API (assuming _api.UpdateCourse works similarly to the Teacher example)
            _api.UpdateCourse(id, UpdatedCourse);

            // Redirect to the 'Show' action to display the updated course
            return RedirectToAction("Show", new { id = id });
        }

    }
}
