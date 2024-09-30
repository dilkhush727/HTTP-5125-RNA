using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q2Controller : ControllerBase
    {

        /// <summary>
        /// It shows a greeting message with the input name/value
        /// </summary>
        /// <param name="name">
        /// Name of the person
        /// </param>
        /// <returns>
        /// It returns a string that says "Hi" followed by the input name/value
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q2/greeting?name= Hi Gary!
        /// GET: localhost:7289/api/q2/greeting?name= Hi Renée!
        /// </example>

        [HttpGet(template: "greeting")]

        public string greeting(string name)
        {
            return ($"Hi {name}!");
        }

    }
}
