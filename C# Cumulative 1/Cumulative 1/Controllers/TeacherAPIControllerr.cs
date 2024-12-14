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
        /// Retrieves a list of teachers along with their assigned courses, with an optional filter based on hire date range.
        /// </summary>
        /// <param name="StartDate">The start of the hire date range (optional).</param>
        /// <param name="EndDate">The end of the hire date range (optional).</param>
        /// <returns>
        /// A list of teachers, including details such as ID, first name, last name, hire date, salary, employee number, and their associated course names.
        /// </returns>
        /// <remarks>
        /// This method queries the database to retrieve teacher and course information. It can optionally filter the results by a specified hire date range.
        /// If no date range is provided, all teachers and their associated courses will be returned.
        /// </remarks>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"teacherId": 1, "teacherFName": "Alexander", "teacherLName": "Bennett", "teacherHireDate": "2016-08-05T00:00:00", "teacherSalary": "55.30", "teacherEmpNu": "T378", "courseNames": ["Web Application Development"]}, ...]
        /// GET api/Teacher/ListTeachers?StartDate=2016-01-01&EndDate=2018-01-01 -> [{"teacherId": 1, "teacherFName": "Alexander", "teacherLName": "Bennett", "teacherHireDate": "2016-08-05T00:00:00", "teacherSalary": "55.30", "teacherEmpNu": "T378", "courseNames": ["Web Application Development"]}, {"teacherId": 6, "teacherFName": "Thomas", "teacherLName": "Hawkins", "teacherHireDate": "2016-08-10T00:00:00", "teacherSalary": "54.45", "teacherEmpNu": "T393", "courseNames": ["Career Connections"]}]
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


                string query = "SELECT * FROM teachers LEFT JOIN courses ON teachers.teacherid = courses.teacherid";


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
                        decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
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

                            };
                        }
                    }

                    Teachers.AddRange(teacherDict.Values);
                }
            }

            return Teachers;
        }

        /// <summary>
        /// Retrieves the details of a specific teacher, including the courses they teach, based on their unique teacher ID.
        /// </summary>
        /// <param name="id">The ID of the teacher whose details are to be fetched.</param>
        /// <returns>
        /// An object containing the teacher's details, including first name, last name, employee number, hire date, salary, 
        /// and a list of the courses they are assigned to teach.
        /// </returns>
        /// <remarks>
        /// This method queries the database to retrieve the teacher's information along with the courses they teach.
        /// If the teacher is found, their details and associated course names will be returned.
        /// </remarks>
        /// <example>
        /// GET api/Teacher/FindTeacher/1 -> {"teacherId":1,"teacherFName":"Alexander","teacherLName":"Bennett","teacherHireDate":"2016-08-05T00:00:00","teacherSalary":"55.30","teacherEmpNu":"T378","courseNames":["Web Application Development"]} 
        /// </example>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher selectedTeacher = null;

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM teachers WHERE teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.Read())  // Only if there is a result
                    {
                        selectedTeacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                            TeacherFName = ResultSet["teacherfname"].ToString(),
                            TeacherLName = ResultSet["teacherlname"].ToString(),
                            TeacherEmpNu = ResultSet["employeenumber"].ToString(),
                            TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                            TeacherSalary = Convert.ToDecimal(ResultSet["salary"])
                        };
                    }
                }
            }

            return selectedTeacher;  // Will return null if not found.
        }

        /// <summary>
        /// Retrieves a list of courses assigned to a specific teacher, identified by their teacher ID.
        /// </summary>
        /// <param name="id">The ID of the teacher whose courses are to be fetched.</param>
        /// <returns>
        /// A list of course names taught by the specified teacher.
        /// </returns>
        /// <remarks>
        /// This method queries the database to retrieve all courses associated with the teacher's ID.
        /// If the teacher is not assigned any courses, an empty list will be returned.
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
                Command.CommandText = "SELECT CourseName,teacherid FROM courses WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        string courseName = ResultSet["CourseName"].ToString();
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);


                        courses.Add(courseName);
                    }
                }
            }

            return courses;
        }

        /// <summary>
        /// Adds a new teacher record to the database.
        /// </summary>
        /// <param name="TeacherData">The teacher object containing the details of the teacher to be added.</param>
        /// <example>
        /// POST: api/Teacher/AddTeacher
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///   "teacherId": 20,
        ///   "teacherFName": "Rohit",
        ///   "teacherLName": "Kumar",
        ///   "teacherHireDate": "2024-11-29T04:13:56.436Z",
        ///   "teacherSalary": 80,
        ///   "teacherEmpNu": "N0178"
        /// } -> 20
        /// </example>
        /// <returns>
        /// The ID of the newly added teacher.
        /// </returns>
        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher Teacherdata)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "INSERT INTO teachers(teacherfname,teacherlname,hiredate,employeenumber,salary) VALUES(@teacherfname,@teacherlname,@hiredate,@TeacherEmpNu,@salary)";
                Command.Parameters.AddWithValue("@teacherfname", Teacherdata.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", Teacherdata.TeacherLName);
                Command.Parameters.AddWithValue("@hiredate", Teacherdata.TeacherHireDate);
                Command.Parameters.AddWithValue("@TeacherEmpNu", Teacherdata.TeacherEmpNu);
                Command.Parameters.AddWithValue("@salary", Teacherdata.TeacherSalary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Removes a teacher from the database using their unique ID.
        /// </summary>
        /// <param name="TeacherId">The ID of the teacher to be deleted.</param>
        /// <example>
        /// DELETE: api/Teacher/DeleteTeacher/1
        /// -> 1
        /// </example>
        /// <returns>
        /// The number of rows affected by the operation (1 if the deletion is successful, 0 if no rows were deleted).
        /// </returns>
        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Updates an existing teacher record in the database. The updated teacher data is provided in the request body, 
        /// and the teacher ID is specified in the request query.
        /// </summary>
        /// <param name="TeacherData">The teacher object containing the updated details.</param>
        /// <param name="TeacherId">The unique ID (primary key) of the teacher to be updated.</param>
        /// <example>
        /// PUT: api/Teacher/UpdateTeacher/4
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "TeacherFName": "Christine",
        ///     "TeacherLName": "Brittle",
        ///     "TeacherHireDate": "2020-08-15",
        ///     "TeacherEmpNu": "T98765",
        ///     "TeacherSalary": 55000
        /// } -> 
        /// {
        ///     "TeacherId": 4,
        ///     "TeacherFName": "Christine",
        ///     "TeacherLName": "Brittle",
        ///     "TeacherHireDate": "2020-08-15",
        ///     "TeacherEmpNu": "T98765",
        ///     "TeacherSalary": 55000
        /// }
        /// </example>
        /// <returns>
        /// The updated teacher object with the new details.
        /// </returns>
        [HttpPut(template: "UpdateTeacher/{TeacherId}")]
        public Teacher UpdateTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "UPDATE teachers  set teacherfname=@teacherfname,teacherlname=@teacherlname,hiredate=@hiredate,employeenumber=@teacherempnum,salary=@salary WHERE teacherid=@id;";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLName);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.TeacherHireDate);
                Command.Parameters.AddWithValue("@teacherempnum", TeacherData.TeacherEmpNu);
                Command.Parameters.AddWithValue("@salary", TeacherData.TeacherSalary);

                Command.Parameters.AddWithValue("@id", TeacherId);

                Command.ExecuteNonQuery();
            }

            return FindTeacher(TeacherId);
        }
    }
}