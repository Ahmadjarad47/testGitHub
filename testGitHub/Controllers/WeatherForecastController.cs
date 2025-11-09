using Microsoft.AspNetCore.Mvc;

namespace TestGitHub.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Cold", "Cool", "Mild", "Warm", "Hot"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // ===== GET v1.0 =====
        [HttpGet("osama")]
        [MapToApiVersion("1.0")]
        public IEnumerable<WeatherForecast> GetV1()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }

        // ===== GET v2.0 =====
        [HttpGet("forecast")]
        [MapToApiVersion("2.0")]
        public IEnumerable<WeatherForecast> GetV2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                ExtraInfo = "Version 2 enhanced data"
            });
        }

        // ===== Test GET v1.0 =====
        [HttpGet("test")]
        [MapToApiVersion("1.0")]
        public IActionResult TestV1()
        {
            return Ok(new { message = "Test endpoint v1.0", timestamp = DateTime.Now });
        }

        // ===== Test GET v2.0 =====
        [HttpGet("test")]
        [MapToApiVersion("2.0")]
        public IActionResult TestV2()
        {
            return Ok(new { message = "Test endpoint v2.0", timestamp = DateTime.Now, features = new[] { "v2 feature" } });
        }

        // ===== POST v1.0 =====
        [HttpPost("test")]
        [MapToApiVersion("1.0")]
        public IActionResult TestPostV1([FromBody] TestRequest request)
        {
            return Ok(new { message = "Received v1.0", data = request, timestamp = DateTime.Now });
        }

        // ===== POST v2.0 =====
        [HttpPost("test")]
        [MapToApiVersion("2.0")]
        public IActionResult TestPostV2([FromBody] TestRequest request)
        {
            return Ok(new { message = "Received v2.0", data = request, timestamp = DateTime.Now, processedAt = DateTime.UtcNow });
        }
    }

    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public string? ExtraInfo { get; set; }
    }

    public class TestRequest
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
    }
}
