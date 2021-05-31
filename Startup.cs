using lookaroond.DB;
using lookaroond.DB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using SlackNet.AspNetCore;

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
            Configuration.Bind(_slackConfig);
            _slackConfig.Assert();
            services.AddControllers();
            services.Configure<AppConfig>(Configuration);
            services.AddSingleton<DbClient>();
            services.AddSingleton<IHostedService, MongoService>();
            services.AddSlackNet(c =>
            {
                c.UseApiToken(_slackConfig.AccessToken);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "lookaroond", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "lookaroond v1"));
            }
            app.UseSerilogRequestLogging();
            app.UseSlackNet(c => c.UseSigningSecret(_slackConfig.SigningSecret));
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
