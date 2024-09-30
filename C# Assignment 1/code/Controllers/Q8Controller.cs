using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q8Controller : ControllerBase
    {
        /// <summary>
        /// It returns the checkout summary for an order with POST request.
        /// </summary>
        /// <param name="Small"> Number of Small plushies ordered </param>
        /// <param name="Large"> Number of large plushies ordered </param>
        /// <returns>
        /// An HTTP response as a string including Subtotal, Tax, and Total
        /// </returns>
        /// <example>
        /// POST localhost:7289/api/q8/squashfellows 
        /// Content-Type: application/x-www-form-urlencoded
        /// REQUEST BODY: Small=1&Large=1
        /// -> 1 Small @ $25.50 = $25.50; 1 Large @ $45.50 = $45.50; Subtotal = $71.00; Tax = $9.23 HST; Total = $80.23
        /// POST localhost:7289/api/q8/squashfellows
        /// Content-Type: application/x-www-form-urlencoded
        /// REQUEST BODY: Small=2&Large=1
        /// -> 2 Small @ $25.50 = $51.00; 1 Large @ $45.50 = $45.50; Subtotal = $96.50; Tax = $12.54 HST; Total = $109.04
        /// POST localhost:7289/api/q8/squashfellows 
        /// Content-Type: application/x-www-form-urlencoded
        /// REQUEST BODY: Small=100&Large=100
        /// -> 100 Small @ $25.50 = $2,550.00; 100 Large @ $45.50 = $4,550.00; Subtotal = $7,100.00; Tax = $923.00 HST; Total = $8,023.00
        /// </example>

        [HttpPost (template: "squashfellows")]

        public string squashfellows([FromForm] int Small, [FromForm] int Large)
        {
            decimal priceSmall = 25.50m;
            decimal priceLarge = 45.50m;
            decimal HSTRate    = 0.13m;

            decimal priceSmallTotal = Small * priceSmall;
            decimal priceLargeTotal = Large * priceLarge;

            decimal subTotal = priceSmallTotal + priceLargeTotal;

            decimal Tax = Math.Round(subTotal * HSTRate, 2);

            decimal Total = Tax + subTotal;

            string resSummary = $"{Small} Small @ {priceSmall.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))} = {priceSmallTotal.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))}; " +
                              $"{Large} Large @ {priceLarge.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))} = {priceLargeTotal.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))}; " +
                              $"subTotal = {subTotal.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))}; " +
                              $"Tax = {Tax.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))} HST; " +
                              $"Total = {Total.ToString("C2", CultureInfo.CreateSpecificCulture("en-CA"))}";

            return resSummary;
        }
    }
}
