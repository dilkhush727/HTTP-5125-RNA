using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using System;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public TeacherAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
            /// Retrieves a list of teachers, along with their associated courses, optionally filtered by a hire date range.
        /// </summary>
        /// <param name="StartDate">The start date of the hire date range (optional).</param>
        /// <param name="EndDate">The end date of the hire date range (optional).</param>
        /// <returns>
            /// A list of teachers, including their ID, first name, last name, hire date, salary, employee number, and the names of the courses they teach.
        /// </returns>
        /// <remarks>
            /// This method queries the database to retrieve teacher and course information. If a hire date range is provided, it filters the teachers by that range; otherwise, it returns all teachers along with their course details.
        /// </remarks>
        /// <example>
            /// GET api/Teacher/ListTeachers -> [{"teacherId":1,"teacherFName":"Alexander","teacherLName":"Bennett","teacherHireDate":"2016-08-05","teacherSalary":"55.30","teacherEmpNu":"T378","courseNames":["Web Application Development"]}, ...]
            /// GET api/Teacher/ListTeachers?StartDate=2016-01-01&EndDate=2018-01-01 -> [{"teacherId":1,"teacherFName":"Alexander","teacherLName":"Bennett","teacherHireDate":"2016-08-05","teacherSalary":"55.30","teacherEmpNu":"T378","courseNames":["Web Application Development"]}, ...]
        /// </example>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<Teacher> Teachers = new List<Teacher>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                string query = "SELECT * FROM teachers INNER JOIN courses ON teachers.teacherid = courses.teacherid";

                bool hasConditions = false;
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    query += " WHERE hiredate BETWEEN @startDate AND @endDate";
                    Command.Parameters.AddWithValue("@startDate", StartDate.Value);
                    Command.Parameters.AddWithValue("@endDate", EndDate.Value);
                    hasConditions = true;
                }

                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    Dictionary<int, Teacher> teacherDict = new Dictionary<int, Teacher>();

                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string TeacherEmpNu = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        string TeacherSalary = ResultSet["salary"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();

                        if (!teacherDict.ContainsKey(Id))
                        {
                            teacherDict[Id] = new Teacher()
                            {
                                TeacherId = Id,
                                TeacherFName = FirstName,
                                TeacherLName = LastName,
                                TeacherHireDate = TeacherHireDate,
                                TeacherSalary = TeacherSalary,
                                TeacherEmpNu = TeacherEmpNu,
                                CourseNames = new List<string>()
                            };
                        }
                        teacherDict[Id].CourseNames.Add(CourseName);
                    }

                    Teachers.AddRange(teacherDict.Values);
                }
            }
            return Teachers;
        }

        /// <summary>
            /// Retrieves the details of a specific teacher, including the courses they teach, based on the teacher's ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to retrieve.</param>
        /// <returns>
         /// The teacher's details, such as their first name, last name, employee number, hire date, salary, and a list of courses they teach.
        /// </returns>
        /// <remarks>
            /// This method queries the database to fetch the teacher's information and the courses they are assigned to. If the teacher is found, their details and associated courses will be returned.
        /// </remarks>
        /// <example>
            /// GET api/Teacher/FindTeacher/1 -> { "teacherId": 1, "teacherFName": "Alexander", "teacherLName": "Bennett", "teacherHireDate": "2016-08-05", "teacherSalary": "55.30", "teacherEmpNu": "T378", "courseNames": ["Web Application Development"] }
        /// </example>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher SelectedTeacher = new Teacher();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT teachers.*, courses.courseName FROM courses INNER JOIN teachers ON teachers.teacherId = courses.teacherId WHERE teachers.teacherId = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string TeacherEmpNu = ResultSet["employeenumber"].ToString();

                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        string TeacherSalary = ResultSet["salary"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        if (SelectedTeacher.TeacherId == 0)
                        {
                            SelectedTeacher.TeacherFName = FirstName;
                            SelectedTeacher.TeacherLName = LastName;
                            SelectedTeacher.TeacherSalary = TeacherSalary;
                            SelectedTeacher.TeacherHireDate = TeacherHireDate;
                            SelectedTeacher.TeacherEmpNu = TeacherEmpNu;
                            SelectedTeacher.CourseNames = new List<string>();
                        }
                        SelectedTeacher.CourseNames.Add(CourseName);
                    }
                }
            }
            return SelectedTeacher;
        }

        /// <summary>
            /// Retrieves a list of courses assigned to a specific teacher by their teacher ID.
        /// </summary>
        /// <param name="id">The ID of the teacher whose courses are to be retrieved.</param>
        /// <returns>
            /// A list of course names taught by the specified teacher. If no courses are found, an empty list is returned.
        /// </returns>
        /// <remarks>
            /// This method queries the database to fetch the courses assigned to the teacher identified by the given ID.
        /// </remarks>
        /// <example>
            /// GET api/Teacher/GetCoursesByTeacher/1 -> ["Web Application Development"]
        /// </example>


        [HttpGet]
        [Route("GetCoursesByTeacher/{id}")]
        public List<string> GetCoursesByTeacher(int id)
        {
            List<string> courses = new List<string>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT CourseName FROM courses WHERE TeacherId = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        string courseName = ResultSet["CourseName"].ToString();
                        courses.Add(courseName);
                    }
                }
            }
            return courses;
        }
    }
}