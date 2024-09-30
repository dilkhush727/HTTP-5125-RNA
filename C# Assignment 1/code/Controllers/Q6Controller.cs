using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q6Controller : ControllerBase
    {
        /// <summary>
        /// It returns the total area of a hexagon with side length S.
        /// </summary>
        /// <param name="side">Length of the hexagon side</param>
        /// <returns>
        /// It retuns the total area of the hexagon
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q6/hexagon?side=1 -> 2.598076211353316
        /// GET: localhost:7289/api/q6/hexagon?side=1.5 -> 5.845671475544961
        /// GET: localhost:7289/api/q6/hexagon?side=20 -> 1039.2304845413264
        /// </example>

        [HttpGet (template: "hexagon")]

        public double hexagon(double side)
        {
            double totalArea = (3 * Math.Sqrt(3) / 2) * Math.Pow(side, 2);
            return totalArea;
        }
    }
}
