using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q4Controller : ControllerBase
    {
        /// <summary>
        /// Receives an HTTP POST request and displays a message.
        /// </summary>
        /// <returns>
        /// It returns a message "Who's there?"
        /// </returns>
        /// <example>
        /// POST: localhost:7289/api/q4/knockknock -> Who's there?
        /// </example>

        [HttpPost (template: "knockknock")]

        public string knockknock()
        {
            return "Who's there?";
        }


        /// <summary>
        /// Receives an HTTP POST request and displays a message.
        /// </summary>
        /// <returns>
        /// It returns a message "Hi, how can I help you?"
        /// </returns>
        /// <example>
        /// POST: localhost:7289/api/q4/example -> "Hi, how can I help you?"
        /// </example>

        [HttpPost(template: "example")]
        public string example()
        {
            return "Hi, how can I help you?";
        }
    }
}
