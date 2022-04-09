using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using webapi.DBO;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly ApplicationContext _context;

        public WeatherForecastController(ApplicationContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        [HttpGet("create")]
        public WeatherForecast Create()
        {
            var rng = new Random();
            var forecast =
                Enumerable.Range(1, 10).Select(index => new WeatherForecast
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).First();

            _context.Add(forecast);
            _context.SaveChanges();

            return forecast;
        }

        [HttpGet("get/{id}")]
        public WeatherForecast Get(Guid id)
        {
            return _context.Forecasts.FirstOrDefault(it => it.Id == id);
        }
    }
}
