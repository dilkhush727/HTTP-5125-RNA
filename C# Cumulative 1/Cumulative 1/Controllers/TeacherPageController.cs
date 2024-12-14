using Cumulative_1.Models;
using Cumulative_1.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cumulative_1.Controllers
{
    // The controller that handles actions related to the Teacher page
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        // Constructor to initialize the API controller
        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: Display a list of teachers, optionally filtered by start and end dates
        [HttpGet]
        public IActionResult List(DateTime? StartDate, DateTime? EndDate)
        {
            // Retrieves the list of teachers based on the given date range
            List<Teacher> Teachers = _api.ListTeachers(StartDate, EndDate);

            // Returns the list to the view
            return View(Teachers);  
        }

        // GET: Display a specific teacher's details by their ID
        [HttpGet]
        public IActionResult Show(int id)
        {
            // Validate the provided teacher ID
            if (id <= 0)
            {
                // Set an error message to be displayed above the content
                ViewBag.ErrorMessage = "Invalid Teacher ID. Please provide a valid ID.";

                // Return an error view if the ID is invalid
                return View("Error");  
            }

            // Get the selected teacher from the API
            var selectedTeacher = _api.FindTeacher(id);

            // Check if the teacher exists
            if (selectedTeacher == null)
            {
                // Set an error message to be displayed above the content
                ViewBag.ErrorMessage = "The specified teacher does not exist. Please check the Teacher ID.";

                // Return an error view if the teacher doesn't exist
                return View("Error");  
            }

            // Retrieve the courses assigned to the teacher
            var teacherCourses = _api.GetCoursesByTeacher(id);

            // Prepare the view model with the teacher's details and courses
            var viewModel = new TeacherCoursesViewModel
            {
                Teacher = selectedTeacher,
                // Set empty list if no courses are found
                Courses = teacherCourses ?? new List<string>() 
            };

            // Return the view with the teacher's details and courses
            return View(viewModel);  
        }

        // GET: Display a page for adding a new teacher
        [HttpGet]
        public IActionResult New(int id)
        {
            // Return the view to create a new teacher
            return View(); 
        }

        // POST: Handle the submission of a new teacher's data
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            // Call API to add the new teacher and retrieve their ID
            int TeacherId = _api.AddTeacher(NewTeacher);

            // Redirect to the "Show" action to display the newly created teacher's details
            return RedirectToAction("Show", new { id = TeacherId });
        }

        // GET: Confirm deletion of a teacher
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // Retrieve the teacher's details to confirm deletion
            Teacher SelectedTeacher = _api.FindTeacher(id);
            
            // Return the confirmation view
            return View(SelectedTeacher);  
        }

        // POST: Handle the deletion of a teacher by their ID
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Call API to delete the teacher and retrieve their ID
            int TeacherId = _api.DeleteTeacher(id);

            // Redirect to the "List" action to display the updated list of teachers
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }
        [HttpPost]
        public IActionResult Update(int id, string TeacherFName, string TeacherLName, DateTime TeacherHireDate, string TeacherEmpNu, decimal TeacherSalary)
        {
            Teacher UpdatedTeacher = new Teacher();
            UpdatedTeacher.TeacherFName = TeacherFName;
            UpdatedTeacher.TeacherLName = TeacherLName;
            UpdatedTeacher.TeacherHireDate = TeacherHireDate;
            UpdatedTeacher.TeacherEmpNu = TeacherEmpNu;
            UpdatedTeacher.TeacherSalary = TeacherSalary;

            // not doing anything with the response
            _api.UpdateTeacher(id, UpdatedTeacher);
            // redirects to show teacher
            return RedirectToAction("Show", new { id = id });
        }
    }
}
