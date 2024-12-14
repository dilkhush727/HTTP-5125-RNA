using Cumulative_1.Models;
using Cumulative_1.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        // Constructor to initialize the Student API controller
        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET: Displays a list of all students
        [HttpGet]
        public IActionResult List()
        {
            // Retrieve the list of students from the API
            List<Student> Students = _api.ListStudent();

            // Pass the list to the view for display
            return View(Students);
        }

        // GET: Displays the details of a specific student by ID
        [HttpGet]
        public IActionResult Show(int id)
        {
            // Retrieve the student details from the API
            Student SelectedStudent = _api.FindStudent(id);

            // Pass the student details to the view
            return View(SelectedStudent);
        }

        // GET: Displays the form to create a new student
        [HttpGet]
        public IActionResult New(int id)
        {
            // Returns the empty form view for creating a new student
            return View();
        }

        // POST: Handles the creation of a new student
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            // Add the new student to the database
            int StudentId = _api.AddStudent(NewStudent);

            // Redirects to the "Show" action with the ID of the newly created student
            return RedirectToAction("Show", new { id = StudentId });
        }

        // GET: Displays a confirmation page before deleting a student
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // Retrieve the student details for confirmation
            Student SelectedStudent = _api.FindStudent(id);

            // Pass the student details to the confirmation view
            return View(SelectedStudent);
        }

        // POST: Handles the deletion of a student by ID
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Delete the student from the database
            int StudentId = _api.DeleteStudent(id);

            // Redirects to the "List" action after deletion
            return RedirectToAction("List");
        }
        // GET method to show the current student's details before editing
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find the selected student by ID
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);  // Pass the student data to the Edit view
        }

        // POST method to handle the updated student data
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, DateTime EnrollDate, string StudentNumber)
        {
            // Create a new Student object to store the updated values
            Student UpdatedStudent = new Student();
            UpdatedStudent.StudentFName = StudentFName;
            UpdatedStudent.StudentLName = StudentLName;
            UpdatedStudent.EnrollDate = EnrollDate;
            UpdatedStudent.StudentNumber = StudentNumber;

            // Call the API to update the student in the database
            _api.UpdateStudent(id, UpdatedStudent);

            // Redirect to the Show page to display the updated student details
            return RedirectToAction("Show", new { id = id });
        }

    }
}
