using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using System;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{

    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
         /// Retrieves a list of all students from the database.
        /// </summary>
        /// <returns>
         /// A list of `Student` objects containing details such as student ID, first name, last name, student number, and enrollment date.
        /// </returns>
        /// <remarks>
            /// This method, executes a query to select all records from the "students" table,
            /// and maps each record to a `Student` object. The resulting list is then returned.
        /// </remarks>
        
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            List<Student> Students = new List<Student>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from students";
                Command.Prepare();
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);
                        Student CurrentStudent = new Student()
                        {
                            StudentId = id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrollDate = EnrolDate,
                            StudentNumber = StudentNumber,

                        };
                        Students.Add(CurrentStudent);
                    }
                }

            }
            return Students;
        }

        /// <summary>
        /// Retrieves the details of a specific student from the database based on their student ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>
        /// A `Student` object containing the student's ID, first name, last name, student number, and enrollment date.
        /// If no student is found, returns an empty `Student` object.
        /// </returns>
        /// <remarks>
        /// This, executes a query to fetch the student record with the given student ID,
        /// and maps the result to a `Student` object. The details of the student are returned after the query execution.
        /// </remarks>
        
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student SelectedStudents = new Student();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        SelectedStudents.StudentId = StudentId;
                        SelectedStudents.StudentFName = FirstName;
                        SelectedStudents.StudentLName = LastName;
                        SelectedStudents.EnrollDate = EnrolDate;
                        SelectedStudents.StudentNumber = StudentNumber;

                    }
                }
            }
            return SelectedStudents;
        }

    }
}