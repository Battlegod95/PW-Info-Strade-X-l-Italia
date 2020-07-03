using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CallSignalR
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<ClockHub, IClock> _clockHub;

        public Worker(ILogger<Worker> logger, IHubContext<ClockHub, IClock> clockHub)
        {
            _logger = logger;
            _clockHub = clockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                await _clockHub.Clients.All.ShowTime(DateTime.Now);
                await Task.Delay(1000);
            }
        }
    }
    public partial class ClockHubClient : IClock, IHostedService
    {
        private readonly ILogger<ClockHubClient> _logger;
        private HubConnection _connection;

        public ClockHubClient(ILogger<ClockHubClient> logger)
        {
            _logger = logger;

            _connection = new HubConnectionBuilder()
                .WithUrl(Strings.HubUrl)
                .Build();

            _connection.On<DateTime>(Strings.Events.TimeSent, ShowTime);
        }

        public Task ShowTime(DateTime currentTime)
        {
            _logger.LogInformation("{CurrentTime}", currentTime.ToShortTimeString());

            return Task.CompletedTask;
        }
    }

}
