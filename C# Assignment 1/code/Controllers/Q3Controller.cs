using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q3Controller : ControllerBase
    {
        /// <summary>
        /// It calculates the cube of a given number
        /// </summary>
        /// <param name="num">
        /// Number that we want a cube value for
        /// </param>
        /// <returns>
        /// It returns the cube of the input number
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q3/cube/4 -> 64
        /// GET: localhost:7289/api/q3/cube/-4 -> -64
        /// GET: localhost:7289/api/q3/cube/10 -> 1000
        /// </example>

        [HttpGet(template: "cube/{num}")]

        public int cube(int num)
        {
            int numCube = num * num * num;

            return numCube;
        }

    }
}
