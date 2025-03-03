using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ApiGatewayDocusaurusQA.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var internalToken = "chostim-security"; // Replace with logic to generate or retrieve your internal token
        var externalToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", externalToken);

        // Validate the token with GitLab
        var response = await client.GetAsync("https://gitlab.com/oauth/token/info");

        if (response.IsSuccessStatusCode)
        {
            var concatenatedToken = $"{externalToken} {internalToken}";

            // Print the concatenated token
            Console.WriteLine($"Concatenated Token: {concatenatedToken}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", concatenatedToken);

            // Make the request to the respective endpoint
            // var endpointResponse = await client.GetAsync("https://your-endpoint-url");
            // endpointResponse.EnsureSuccessStatusCode();

            // var weatherForecasts = await endpointResponse.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();

            return Ok("Token validado correctamente");
        }
        else
        {
            return Unauthorized();
        }
    }
}