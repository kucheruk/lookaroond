using lookaroond.Controllers;
using lookaroond.DB;
using lookaroond.DB.Commands;
using lookaroond.DB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace lookaroond
{
    public class Startup
    {
        private SlackConfig _slackConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _slackConfig = new SlackConfig();
            Configuration.GetSection("Slack").Bind(_slackConfig);
            _slackConfig.Assert();
            services.AddControllers();
            services.Configure<AppConfig>(Configuration);
            services.Configure<SlackConfig>(Configuration.GetSection("Slack"));
            services.AddSingleton<DbClient>();
            services.AddTransient<ISlackEventHandler, SlackEventChallengeHandler>();
            services.AddScoped<SlackRequestContainer>();
            services.AddSingleton<SlackRequestSignature>();
            services.AddSingleton<StoreAccessTokenCommand>();
            services.AddSingleton<IHostedService, MongoService>();
            services.AddHttpClient<SlackHttp>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            app.UseDeveloperExceptionPage();
            // }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<CheckSlackSignatureMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}