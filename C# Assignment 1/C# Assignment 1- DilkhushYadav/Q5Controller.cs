using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q5Controller : ControllerBase
    {
        /// <summary>
        /// It returns a input secret number by POST request
        /// </summary>
        /// <param name="num">
        /// The secret number
        /// </param>
        /// <returns>
        /// It returns a secret number 
        /// </returns>
        /// <example>
        /// POST: localhost:7289/api/q5/secret
        /// Content-Type: application/json
        /// REQUEST BODY: 5
        /// -> Shh.. the secret is 5
        /// POST: localhost:7289/api/q5/secret
        /// Content-Type: application/json
        /// REQUEST BODY: -200
        /// -> Shh.. the secret is -200
        /// </example>

        [HttpPost (template: "secret")]

        public string secret([FromBody] int num)
        {
            return $"Shh.. the secret is {num}";
        }
    }
}
