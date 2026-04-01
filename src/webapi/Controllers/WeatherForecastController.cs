using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private static readonly string[] Summaries =
  [
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  ];

  private WeatherForecast[] WeatherForecasts;

  public WeatherForecastController()
  {
    WeatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
  }

  /// <summary>
  /// Return all weatherforecasts
  /// </summary>
  /// <returns>List of weatherforecasts</returns>
  [HttpGet(Name = "GetWeatherForecast")]
  public IEnumerable<WeatherForecast> Get()
  {
    return WeatherForecasts;
  }

  /// <summary>
  /// Return weatherforescast by position
  /// </summary>
  /// <param name="id">Weatherforecast index position</param>
  /// <returns>Weatherforecast</returns>
  [HttpGet]
  [Route("{id}")]
  public ActionResult<WeatherForecast> GetWeatherForecastByPosition(int id)
  {
    if (id < 0 || id >= WeatherForecasts.Length)
    {
      return NotFound();
    }
    return WeatherForecasts[id];
  }

  [HttpPut("{index}")]
  public IActionResult Put(int index, [FromBody] WeatherForecast weatherForecast)
  {
      if (index < 0 || index >= WeatherForecasts.Length)
      {
          return NotFound();
      }

      WeatherForecasts[index] = weatherForecast;
      return NoContent();
  }

  [HttpPatch("{index}")]
  public IActionResult Patch(int index, [FromBody] WeatherForecast weatherForecast)
  {
      if (index < 0 || index >= WeatherForecasts.Length)
      {
          return NotFound();
      }

      var existingForecast = WeatherForecasts[index];

      if (weatherForecast.Date != default)
      {
          existingForecast.Date = weatherForecast.Date;
      }

      if (weatherForecast.TemperatureC != default)
      {
          existingForecast.TemperatureC = weatherForecast.TemperatureC;
      }

      if (!string.IsNullOrEmpty(weatherForecast.Summary))
      {
          existingForecast.Summary = weatherForecast.Summary;
      }

      return NoContent();
  }

  [HttpDelete("{index}")]
  public IActionResult Delete(int index)
  {
      if (index < 0 || index >= WeatherForecasts.Length)
      {
          return NotFound();
      }

      WeatherForecasts = WeatherForecasts.Where((source, elementIndex) => elementIndex != index).ToArray();
      return NoContent();
  }
}
