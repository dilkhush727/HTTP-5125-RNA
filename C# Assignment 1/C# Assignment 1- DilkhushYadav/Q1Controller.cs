using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q1Controller : ControllerBase
    {

        /// <summary>
        /// It shows welcome message with the GET request
        /// </summary>
        /// <returns>
        /// It returns HTTP response with a body that displays "welcome to 5125!"
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q1/welcome -> "Welcome to 5125!"
        /// </example>

        [HttpGet(template: "Welcome")]

        public String Welcome()
        {
            return "Welcome to 5125!";
        }


        /// <summary>
        /// It shows welcome message with the GET request
        /// </summary>
        /// <returns>
        /// It returns HTTP response with a body that displays "Hello, my name is Dilkhush Yadav"
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q1/welcome -> "Hello, my name is Dilkhush Yadav"
        /// </example>
        /// 
        [HttpGet(template: "example")]
        public string example()
        {
            return ("Hello, my name is Dilkhush Yadav");
        }

    }
}
