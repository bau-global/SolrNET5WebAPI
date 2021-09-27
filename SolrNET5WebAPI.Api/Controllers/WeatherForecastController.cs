using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// solr
using SolrNet;
// team
using SolrNET5WebAPI.Api.Models;

namespace SolrNET5WebAPI.Api.Controllers
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
        private readonly ISolrOperations<WeatherForecast> _solr;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISolrOperations<WeatherForecast> solr)
        {
            _logger = logger;
            _solr = solr;
        }

        [HttpGet]
        public async Task<IActionResult> QueryTemperatureAsync([FromQuery]string parameter)
        {
            // sample
            // parameter = "Freezing";
            _logger.LogInformation("Search: " + parameter);
            SolrQueryResults<WeatherForecast> results = await _solr.QueryAsync(new SolrQuery($"summary: {parameter}"));

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> AddTemperatureAsync([FromQuery]WeatherForecast weather)
        {
            _logger.LogInformation("Add: " + weather.Summary);

            // sample
            //Random rng = new();
            //WeatherForecast weather = new()
            //{
            //    Id = "1",
            //    Summary = Summaries[rng.Next(Summaries.Length)],
            //    Temperature = 10
            //};

            await _solr.AddAsync(weather);
            await _solr.CommitAsync();

            return Ok(weather);
        }
    }
}