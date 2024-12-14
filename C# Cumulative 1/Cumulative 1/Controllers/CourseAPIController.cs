using Cumulative_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public CourseAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all courses from the database.
        /// </summary>
        /// <returns>
        /// A list of courses, each containing the course ID, teacher ID, course code, name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method queries the "courses" table and returns a collection of course objects with relevant details.
        /// </remarks>
        /// <example>
        /// GET api/Course/listCourse -> [ { "courseId": 1, "coursecode": "http5101", "teacherid": 1, "startdate": "2018-09-04", "finishdate": "2018-12-14", "coursename": "Web Application Development" }, ... ]
        /// </example>
        [HttpGet]
        [Route(template: "listCourse")]
        public List<Course> ListCourse()
        {
            List<Course> Courses = new List<Course>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from courses";
                Command.Prepare();
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Couseid = Convert.ToInt32(ResultSet["courseid"]);
                        int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        Course CurrentCourse = new Course()
                        {
                            courseId = Couseid,
                            teacherid = teacherId,
                            coursecode = CourseCode,
                            coursename = CourseName,
                            startdate = StartDate,
                            finishdate = FinishDate

                        };

                        Courses.Add(CurrentCourse);

                    }
                }

            }
            return Courses;
        }

        /// <summary>
        /// Retrieves the details of a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>
        /// The course details, including course ID, teacher ID, course code, name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method queries the `courses` table in the database to fetch the details of the course matching the provided ID.
        /// If found, it returns the course details; otherwise, it returns null or an error.
        /// </remarks>
        /// <example>
        /// GET api/Course/FindCourse/1 -> { "courseId": 1, "coursecode": "http5101", "teacherid": 1, "startdate": "2018-09-04", "finishdate": "2018-12-14", "coursename": "Web Application Development" }
        /// </example>
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {

            Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "Select * from courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Couseid = Convert.ToInt32(ResultSet["courseid"]);
                        int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        SelectedCourse.courseId = Couseid;
                        SelectedCourse.teacherid = teacherId;
                        SelectedCourse.coursecode = CourseCode;
                        SelectedCourse.coursename = CourseName;
                        SelectedCourse.startdate = StartDate;
                        SelectedCourse.finishdate = FinishDate;
                    }
                }
            }
            return SelectedCourse;
        }

        /// <summary>
        /// Adds a new course to the system's database.
        /// </summary>
        /// <param name="CourseData">An object containing course details including course code, teacher ID, start date, end date, and course name.</param>
        /// <example>
        /// POST: api/Student/AddCourse
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "coursecode": "CS101",
        ///     "teacherid": 5,
        ///     "startdate": "2024-01-15T00:00:00",
        ///     "finishdate": "2024-06-01T00:00:00",
        ///     "coursename": "Introduction to Computer Science"
        /// }
        /// Response:
        /// 7  // Returns the ID of the newly created course.
        /// </example>
        /// <returns>
        /// The ID of the newly created course if the operation is successful. Returns 0 if the operation fails.
        /// </returns>

        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' ensures the connection is closed after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "INSERT INTO courses(coursecode, teacherid, startdate, finishdate, coursename) " +
                                      "VALUES(@coursecode, @teacherid, @startdate, @finishdate, @coursename)";

                // Adding parameters to prevent SQL injection
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);

                // Execute the query and return the ID of the newly inserted course
                Command.ExecuteNonQuery();
                return Convert.ToInt32(Command.LastInsertedId);
            }
            // Return 0 if the operation fails
            return 0;
        }

        /// <summary>
        /// Removes a course from the database using its unique ID.
        /// </summary>
        /// <param name="courseId">The ID of the course to be removed.</param>
        /// <example>
        /// DELETE: api/Student/DeleteCourse/3
        /// Response:
        /// 1  // Returns the number of rows impacted (1 if the deletion is successful).
        /// </example>
        /// <returns>
        /// The number of rows impacted by the deletion. Returns 0 if the deletion fails.
        /// </returns>
        [HttpDelete(template: "DeleteCourse/{courseId}")]
        public int DeleteCourse(int courseId)
        {
            // 'using' ensures the connection is closed after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "DELETE FROM courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", courseId);

                // Execute the query and return the number of rows affected
                return Command.ExecuteNonQuery();
            }
            // Return 0 if the operation fails
            return 0;
        }

        /// <summary>
        /// Updates the details of an existing course in the database. The course data is provided in the request body, 
        /// and the course ID is specified in the request query.
        /// </summary>
        /// <param name="CourseData">The course object containing the updated details for the course.</param>
        /// <param name="CourseId">The unique ID (primary key) of the course to be updated.</param>
        /// <example>
        /// PUT: api/Course/UpdateCourse/4
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "coursecode": "CS101",
        ///     "teacherid": 2,
        ///     "startdate": "2024-01-01",
        ///     "finishdate": "2024-05-01",
        ///     "coursename": "Introduction to Computer Science"
        /// } -> 
        /// {
        ///     "courseid": 4,
        ///     "coursecode": "CS101",
        ///     "teacherid": 2,
        ///     "startdate": "2024-01-01",
        ///     "finishdate": "2024-05-01",
        ///     "coursename": "Introduction to Computer Science"
        /// }
        /// </example>
        /// <returns>
        /// The updated course object with the new details.
        /// </returns>
        [HttpPut("UpdateCourse/{CourseId}")]
        public Course UpdateCourse(int CourseId, [FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Parameterized query to prevent SQL injection
                Command.CommandText = "UPDATE courses SET coursecode=@coursecode, teacherid=@teacherid, startdate=@startdate, finishdate=@finishdate, coursename=@coursename WHERE courseid=@id";
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);
                Command.Parameters.AddWithValue("@id", CourseId);

                
                Command.ExecuteNonQuery();

               
               
            }

            // Return the updated course details
            return FindCourse(CourseId); // Return the updated course
        }


    }


}

