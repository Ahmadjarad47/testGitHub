using Microsoft.AspNetCore.Mvc;

namespace testGitHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "asdasd",  "Warm", "Bas-------------------swlmy", "Hotss", "Sweltering", "asdasd"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeather-Forecast")]
        public IEnumerable<WeatherForecast> Gets()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { 
                message = "Test endpoint is working!", 
                timestamp = DateTime.Now,
                status = "success"
            });
        }

        [HttpPost("test")]
        public IActionResult TestPost([FromBody] TestRequest request)
        {
            return Ok(new
            {
                message = "POST test endpoint received your data!",
                receivedData = request,
                timestamp = DateTime.Now,
                status = "success"
            });
        }
    }

    public class TestRequest
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
    }
}
