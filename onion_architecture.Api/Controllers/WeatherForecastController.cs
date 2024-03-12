using Microsoft.AspNetCore.Mvc;
using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.Helper;
using onion_architecture.Application.IService;

namespace onion_architecture.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public IActionResult Create(CreateUser user)
        {
            return Ok(_service.Create(user));
        }
        [HttpGet("User")]
        public IActionResult GetItem(int page=1,int size=10,string? keyword="")
        {
            return Ok(_service.Items(page,size,keyword));
        }
        [HttpDelete("{id}")]
        public IActionResult Create(long id)
        {
            return Ok(_service.Delete(id));
        }
    }
}