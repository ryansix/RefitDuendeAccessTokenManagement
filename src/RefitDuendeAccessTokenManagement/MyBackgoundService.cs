
using RefitDundenAuth.Services;
using System.Text.Json;
namespace RefitDundenAuth
{
    public class MyBackgoundService : IHostedService
    {
        private readonly IWebApiService _webApiService;
        private readonly ILogger _logger;
        public MyBackgoundService(IWebApiService webApiService,
            ILogger<MyBackgoundService> logger)
        {
            _webApiService = webApiService;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var res = await _webApiService.PostAsync();
                        _logger.LogInformation(JsonSerializer.Serialize(res));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }, cancellationToken);
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
