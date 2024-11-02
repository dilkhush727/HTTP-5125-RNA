# Files from HTTP-5125
## Back End Web Development 1
## Course Code: HTTP 5125

### Academic Year: 2025-2026

In this course, I will explore server-side web development using the C# programming language and learn to implement techniques for building data-driven websites that utilize various external data sources.

# links
https://learn.microsoft.com/en-us/dotnet/csharp/

# Image
https://github.com/dilkhush727/HTTP-5125-RNA/blob/main/back-end-development.jpg?raw=true


> **Note**: This repository includes key files and resources to aid your learning in back-end development. A solid understanding of databases and APIs will improve your ability to implement data-driven techniques effectively.



# Example code


    [Route("api/[controller]")]
    [ApiController]
    public class J1Controller : ControllerBase
    {

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
