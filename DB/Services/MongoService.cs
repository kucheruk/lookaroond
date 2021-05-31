using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace lookaroond.DB.Services
{
    public class MongoService : IHostedService
    {
        private readonly DbClient _db;
        private readonly ILogger<MongoService> _logger;

        public MongoService(DbClient db, ILogger<MongoService> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _db.Start();
                sw.Stop();
                _logger.LogInformation("Mongo initialized ({Time}ms)", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing mongoDb connection");
                throw;
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _db.Stop();
            return Task.CompletedTask;
        }
    }
}