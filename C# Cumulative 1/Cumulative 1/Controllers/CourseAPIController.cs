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
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        Course CurrentCourse = new Course()
                        {
                            courseId = CourseId,
                            teacherid = TeacherId,
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
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        SelectedCourse.courseId = CourseId;
                        SelectedCourse.teacherid = TeacherId;
                        SelectedCourse.coursecode = CourseCode;
                        SelectedCourse.coursename = CourseName;
                        SelectedCourse.startdate = StartDate;
                        SelectedCourse.finishdate = FinishDate;
                    }
                }
            }
            return SelectedCourse;
        }

    }

}
