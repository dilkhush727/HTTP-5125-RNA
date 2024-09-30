using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q7Controller : ControllerBase
    {

        // GET: localhost:7289/api/q7/timemachine?days=1
        // GET: localhost:7289/api/q7/timemachine?days=-1

        /// <summary>
        /// It returns the current date more or less by the given number of days, format is YYYY-MM-DD
        /// </summary>
        /// <param name="days">Number of days to increase or decrease current date</param>
        /// <returns>
        /// It returns the date in YYYY-MM-DD format
        /// </returns>
        /// <example>
        /// GET: localhost:7289/api/q7/timemachine?days=1 -> 2024-10-01
        /// GET: localhost:7289/api/q7/timemachine?days=-1 -> 2024-09-29
        /// </example>

        [HttpGet (template: "timemachine")]

        public string timemachine(int days)
        {
            DateTime dateNow = DateTime.Today;
            DateTime totalDays = dateNow.AddDays(days);
            return totalDays.ToString("yyyy-MM-dd");
        }
    }
}
