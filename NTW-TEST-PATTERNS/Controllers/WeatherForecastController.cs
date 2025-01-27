using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Service.Services.IA;
using System.Text;

namespace NTW_TEST_PATTERNS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IOllamaService _ollamaService;

        public WeatherForecastController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(string prompt)
        {
            var ollamaResponse = await _ollamaService.GetOllamaResponseAsync(prompt, "llama3.1:8b");
            return Ok(ollamaResponse);
        }
    }
}
