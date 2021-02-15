using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HelloRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static ConnectionMultiplexer connection;
        private static String connectionString =
            "ReportHubRedisInstance.redis.cache.windows.net:6380,password=6DGfDR5uaTiJCYowyEvWaW3ETM0sqopvTS8qSvMh44Q=,ssl=True,abortConnect=False";
        private static IDatabase database;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            try
            {
                connection = ConnectionMultiplexer.ConnectAsync(connectionString).Result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message.ToString());
                throw;
            }

            database = connection.GetDatabase(0);
            database.StringSet("message", "Welcome to Redis!!");


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
