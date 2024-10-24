using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class J1Controller : ControllerBase
    {

        /// <summary>
            /// Get final score by packages collisions and deliveries
        /// </summary>
        /// 
        /// <param name="collisions"> Number of collisions</param>
        /// <param name="deliveries"> Number of deliveries</param>
        /// 
        /// <return>Final score/return>
        /// 
        /// <example>
            /// POST     : /api/J1/Delivedroid
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: collisions=2&deliveries=5
            /// Output   : 730
            /// 
            /// POST     : /api/J1/Delivedroid
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: collisions=102deliveries=0
            /// Output   : -100
            /// 
            /// POST     : /api/J1/Delivedroid
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: collisions=3&deliveries=2
            /// Output   : 70
        /// </example>

        [HttpPost(template: "Delivedroid")]
        [Consumes("application/x-www-form-urlencoded")]
        public int Delivedroid([FromForm] int collisions, [FromForm] int deliveries)
        {
            int finalScore = (deliveries * 50) + (collisions * -10);

            if (deliveries > collisions)
            {
                finalScore = finalScore + 500;
            }

            return finalScore;
        }

        /// <summary>
        /// 2022 J1: Get the remaning cupcakes after distributing into the class
        /// </summary>
        /// 
        /// <param name="large"> Number of large boxes</param>
        /// <param name="small"> Number of small boxes</param>
        /// 
        /// <return>Remaining cupcakes</return>
        /// 
        /// <example>
            /// POST     : api/J1/CupcakesParty
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: large=2&small=5
            /// Output   : 3
            /// 
            /// POST     : api/J1/CupcakesParty
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: large=2&small=4
            /// Output   : 0
        /// </example>

        [HttpPost(template: "CupCakeParty")]
        [Consumes("application/x-www-form-urlencoded")]
        public int CupCakeParty([FromForm] int large, [FromForm] int small)
        {
            int totalCups     = (large * 8) + (small * 3);
            int remainingCups = totalCups - 28;

            return remainingCups;
        }

    }
}
