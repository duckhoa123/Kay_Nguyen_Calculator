using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Calculator.Controllers
{
    [ApiController]
    public class CalculatorController : Controller
    {

        private readonly CalculatorService _calculatorService;

        public CalculatorController(
            CalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost("calculate/json")]
        public async Task<IActionResult> CalculateJson()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                try
                {
                    string input = await reader.ReadToEndAsync();
                    var result = _calculatorService.CalculateFromJson(input);
                    return Ok(new { Result = result });
                }
                catch (Exception)
                {
                    return StatusCode(500, "An error occurred while processing the JSON input.");
                }
        }


        [HttpPost("calculate/xml")]
        public async Task<IActionResult> CalculateXml()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                try
                {
                    string input = await reader.ReadToEndAsync();
                    var result = _calculatorService.CalculateFromXml(input);
                    return Ok(new { Result = result });
                }
                catch (Exception)
                {
                    return StatusCode(500, "An error occurred while processing the XML input.");
                }
            }
        }
    }
}
