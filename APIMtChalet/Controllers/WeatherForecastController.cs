using APIMtChalet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMtChalet.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase {

        //Dbcontext, tak powinno być?
        private readonly MtChaletDBContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MtChaletDBContext context) {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // Test Get, 01012022 = 2
        [HttpGet("{test}")]
        public IEnumerable<Reservations> Get2(string test) {

            string dateStr = test[0].ToString();
            dateStr = dateStr + test[1] + "/" + test[2] + test[3] + "/" + test[4] + test[5] + test[6] + test[7];
            DateTime date = DateTime.Parse(dateStr);
            var reservations = _context.Reservations.Where(s => s.StartingDate >= date && s.EndingDate <= date).ToArray();

            return reservations;
        }
    }
}
