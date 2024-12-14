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

        // Constructor to initialize the database context
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetches a list of all students from the database.
        /// </summary>
        /// <example>
        /// GET: api/Student/listStudents -> [{"studentId":1,"studentFName":"Sarah","studentLName":"Valdez","enrollDate":"2018-06-18T00:00:00","studentNumber":"N1678"},...]
        /// </example>
        /// <returns>
        /// A list of student objects containing student details.
        /// </returns>
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            List<Student> Students = new List<Student>();

            // Open a connection to the database
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to select all students
                Command.CommandText = "SELECT * FROM students";
                Command.Prepare();

                // Execute the query and read the data
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Create a new Student object and add it to the list
                        Student CurrentStudent = new Student()
                        {
                            StudentId = id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrollDate = EnrolDate,
                            StudentNumber = StudentNumber
                        };

                        Students.Add(CurrentStudent);
                    }
                }
            }
            return Students;
        }

        /// <summary>
        /// Retrieves the details of a specific student by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the student whose details are to be fetched.</param>
        /// <example>
        /// GET: api/Student/FindStudent/1 -> {"studentId":1,"studentFName":"Sarah","studentLName":"Valdez","enrollDate":"2018-06-18T00:00:00","studentNumber":"N1678"}
        /// </example>
        /// <returns>
        /// A Student object containing the details of the specified student.
        /// </returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student SelectedStudent = new Student();

            // Open a connection to the database
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to find a student by ID
                Command.CommandText = "SELECT * FROM students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // Execute the query and read the data
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Map database fields to the Student object
                        SelectedStudent.StudentId = StudentId;
                        SelectedStudent.StudentFName = FirstName;
                        SelectedStudent.StudentLName = LastName;
                        SelectedStudent.EnrollDate = EnrolDate;
                        SelectedStudent.StudentNumber = StudentNumber;
                    }
                }
            }
            return SelectedStudent;
        }

        /// <summary>
        /// Adds a new student record to the database.
        /// </summary>
        /// <param name="StudentData">An object containing the details of the student to be added.</param>
        /// <example>
        /// POST: api/Student/AddStudent
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// { "studentId": 0, "studentFName": "John", "studentLName": "Doe", "enrollDate": "2024-11-29T04:09:02.628Z", "studentNumber": "S1234" }
        /// -> Returns the ID of the newly added student.
        /// </example>
        /// <returns>
        /// The ID of the student that was successfully added to the database.
        /// </returns>
        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            // Open a connection to the database
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to insert a new student
                Command.CommandText = "INSERT INTO students(studentfname, studentlname, studentnumber, enroldate) VALUES(@studentfname, @studentlname, @studentnumber, @enroldate)";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollDate);

                // Execute the query and return the new student ID
                Command.ExecuteNonQuery();
                return Convert.ToInt32(Command.LastInsertedId);
            }
            // Return 0 if the operation fails
            return 0;
        }

        /// <summary>
        /// Removes a student record from the database using their unique ID.
        /// </summary>
        /// <param name="StudentId">The ID of the student to be deleted.</param>
        /// <example>
        /// DELETE: api/Student/DeleteStudent/1
        /// -> Returns 1 if the deletion is successful.
        /// </example>
        /// <returns>
        /// The number of rows affected by the operation (1 if successful, 0 if unsuccessful).
        /// </returns>
        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            // Open a connection to the database
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to delete a student by ID
                Command.CommandText = "DELETE FROM students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", StudentId);

                // Execute the query and return the number of affected rows
                return Command.ExecuteNonQuery();
            }
            // Return 0 if the operation fails
            return 0;
        }

        /// <summary>
        /// Updates an existing student record in the database. The updated student data is provided in the request body, 
        /// and the student ID is specified in the request query.
        /// </summary>
        /// <param name="StudentData">The student object containing the updated student information.</param>
        /// <param name="StudentId">The unique ID (primary key) of the student to be updated.</param>
        /// <example>
        /// PUT: api/Student/UpdateStudent/4
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "StudentFName": "Sumit",
        ///     "StudentLName": "",
        ///     "EnrollDate": "2024-01-01",
        ///     "StudentNumber": "S12345"
        /// } -> 
        /// {
        ///     "StudentId": 4,
        ///     "StudentFName": "Sumit",
        ///     "StudentLName": "Singh",
        ///     "EnrollDate": "2024-01-01",
        ///     "StudentNumber": "S12345"
        /// }
        /// </example>
        /// <returns>
        /// The updated student object with the new details.
        /// </returns>
        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "UPDATE students SET studentfname=@studentfname, studentlname=@studentlname,enroldate=@enroldate, studentnumber=@studentnum WHERE studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollDate);
                Command.Parameters.AddWithValue("@studentnum", StudentData.StudentNumber);

                // Add student ID to specify which student to update
                Command.Parameters.AddWithValue("@id", StudentId);

                // Execute the query to update the student's details
                Command.ExecuteNonQuery();
            }

            // Return the updated student details
            return FindStudent(StudentId);
        }
    }
}
