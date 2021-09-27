# How to connect Solr with .NET5

This is a very basic sample to show Solr & .NET5 integration.

### Nuget Packages
- SolrNet.Core
- SolrNet.Microsoft.DependencyInjection

### Init Solr Service in Startup.cs

```csharp
var credentials = Encoding.ASCII.GetBytes("username:password");
var credentialsBase64 = Convert.ToBase64String(credentials);
services.AddSolrNet<WeatherForecast>("https://solr-server-ip:port/solr/name", options =>
{
  options.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialsBase64);
});
```

### Model

```csharp
public class WeatherForecast
{
  [SolrUniqueKey("id")]
  public string Id { get; set; }

  [SolrField("temperature")]
  public int Temperature { get; set; }

  [SolrField("summary")]
  public string Summary { get; set; }
}
```

### Api Controller
```csharp
private readonly ILogger<WeatherForecastController> _logger;
private readonly ISolrOperations<WeatherForecast> _solr;

public WeatherForecastController(ILogger<WeatherForecastController> logger, ISolrOperations<WeatherForecast> solr)
{ 
  _logger = logger;
  _solr = solr;
}
```

### Query

```csharp
[HttpGet]
public async Task<IActionResult> QueryTemperatureAsync([FromQuery]string parameter)
{
    // sample
    // parameter = "Freezing";
    _logger.LogInformation("Search: " + parameter);
    SolrQueryResults<WeatherForecast> results = await _solr.QueryAsync(new SolrQuery($"summary: {parameter}"));

    return Ok(results);
}
```

### Add

```csharp
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
```

### License

[MIT licensed](./LICENSE).
