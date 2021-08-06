using System.Threading.Tasks;
using lookaroond.DB.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace lookaroond.Controllers
{
    [ApiController]
    public class SlackApiController : ControllerBase
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly StoreAccessTokenCommand _store;
        private readonly IOptions<SlackConfig> _config;
        private readonly ILogger<SlackApiController> _logger;
        private readonly SlackHttp _slackHttp;

        public SlackApiController(ILogger<SlackApiController> logger, 
            IOptions<SlackConfig> config,
            IOptions<AppConfig> appConfig,
            StoreAccessTokenCommand store, 
            SlackHttp slackHttp)
        {
            _logger = logger;
            _config = config;
            _appConfig = appConfig;
            _store = store;
            _slackHttp = slackHttp;
        }

        [HttpGet]
        [Route("api/install-redirect")]
        public IActionResult Install()
        {
            var scopes = string.Join(",", _config.Value.Scopes);
            var s = $"https://slack.com/oauth/v2/authorize?scope={scopes}&client_id={_config.Value.ClientId}&redirect_uri={_appConfig.Value.Url}/api/redirect-uri";
            _logger.LogInformation("Install Redirect {Url}", s);
            return Redirect(s);
        }

        [HttpGet]
        [Route("api/redirect-uri")]
        public async Task<IActionResult> OauthRedirect([FromQuery] string code)
        {
            _logger.LogInformation("Got Redirect! {Code}", code);
            var res = await _slackHttp.OauthAccessAsync(_config.Value.ClientId, _config.Value.ClientSecret, code);
            await _store.StoreToken(res);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}