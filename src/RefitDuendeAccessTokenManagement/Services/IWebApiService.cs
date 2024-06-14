using Refit;
using RefitDundenAuth.Services.Models;
namespace RefitDundenAuth.Services
{
    public interface IWebApiService
    {
        [Post("/api")]
        public Task<IEnumerable<WeatherForecast>> GetAsync();

        [Post("/WeatherForecast")]
        public Task<IEnumerable<WeatherForecast>> PostAsync();
    }
}
