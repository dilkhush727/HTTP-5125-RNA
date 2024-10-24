using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class J3Controller : ControllerBase
    {

        /// <summary>
            /// 2021 J3: Decodes secret inputs and returns the number of steps with direction
        /// </summary>
        /// 
        /// <param name="instructions">A list of 5 digit strings</param>
        /// 
        /// <return>A decoded string (direction and steps)</return>
        /// 
        /// <example>
            /// POST     : /api/J3/secretInstructions
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: instructions=57234,00907,34100,99999
            /// Output   : Right: 234
            /// Output   : Right: 907
            /// Output   : Left: 100
        /// </example>

        [HttpPost(template: "secretInstructions")]
        [Consumes("application/x-www-form-urlencoded")]
        public string secretInstructions([FromForm] string instructions)
        {
            string[] instructionList = instructions.Split(',');
            string results           = "";
            string lastDirection     = "";

            foreach (var i in instructionList)
            {
                if (i == "99999")
                {
                    break;
                }

                string direction;
                int first2digits  = int.Parse(i.Substring(0, 2));
                int Last3Digits   = int.Parse(i.Substring(2, 3));
                int sumOFfirsttwo = (first2digits / 10) + (first2digits % 10);

                if (sumOFfirsttwo == 0)
                {
                    direction = lastDirection;
                }
                else if (sumOFfirsttwo % 2 == 0)
                {
                    direction = "Right: ";
                }
                else
                {
                    direction = "Left: ";
                }

                lastDirection = direction;
                results      += $"{direction} {Last3Digits}\n";
            }

            return results.TrimEnd('\n');
        }

    }
}