using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class J2Controller : ControllerBase
    {

        /// <summary>
        /// Get the total spiciness according to the added spices
        /// </summary>
        /// 
        /// <param name="ingridents"> Contains all the spices</param>
        /// 
        /// <return>Total spiciness with corresponding SHU values</return>
        /// 
        /// <example>
            /// GET   : /api/J2/ChiliPepper=Poblano,Cayenne,Thai,Poblano
            /// Output: 118000
            /// 
            /// GET   : /api/J2/ChiliPepper=Habanero,Habanero,Habanero,Habanero,Habanero
            /// Output: 625000
            /// 
            /// GET   : /api/J2/ChiliPepper=Poblano,Mirasol,Serrano,Cayenne,Thai,Habanero,Serrano
            /// Output: 278500
        /// </example>

        [HttpGet(template: "ChiliPepper={ingridents}")]
        public int ChiliPepper(string ingridents)
        {
            Dictionary < string, int > peppers = new()
            {
               { "Poblano",1500 },
               { "Mirasol",6000 },
               { "Serrano",15500 },
               { "Cayenne",40000 },
               { "Thai",75000 },
               { "Habanero",125000 }
            };

            string[] peppersList = ingridents.Split(',');
            int totalSpiciness   = 0;

            foreach (var pepper in peppersList)
            {
                if (peppers.ContainsKey(pepper))
                {
                    totalSpiciness += peppers[pepper];
                }
            }

            return totalSpiciness;
        }

        /// <summary>
        /// 2017 J2: Calculating a shifty sum, which is the sum of a number and the numbers we get by shifting
        /// </summary>
        /// 
        /// <param name="N"> Starting or base number</param>
        /// <param name="K"> Shifting number</param>
        /// 
        /// <returns>Shifty Sum of N by K</returns>

        /// <example>
            /// POST     : /api/J2/ShiftySum
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: N=12&K=3
            /// Output   : 13332
            /// 
            /// POST     : /api/J2/ShiftySum
            /// Headers  : Content-Type: application/x-www-form-urlencoded
            /// Post data: N=12&K=1
            /// Output   : 132
        /// </example>

        [HttpPost(template: "ShiftySum")]
        [Consumes("application/x-www-form-urlencoded")]
        public int ShiftySum([FromForm] int N, [FromForm] int K)
        {
            int startingNumber = N;
            int shiftingNumber = K;

            int count = 0;

            for (int i = 0; i <= shiftingNumber; i++)
            {
                count += startingNumber;
                startingNumber *= 10;
            }

            return count;
        }

    }
}