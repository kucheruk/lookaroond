using System.Collections.Generic;
using System.Text.Json;
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
        private readonly IOptions<SlackConfig> _config;
        private readonly IEnumerable<ISlackEventHandler> _handlers;
        private readonly ILogger<SlackApiController> _logger;
        private readonly SlackHttp _slackHttp;
        private readonly StoreAccessTokenCommand _store;

        public SlackApiController(ILogger<SlackApiController> logger,
            IOptions<SlackConfig> config,
            IOptions<AppConfig> appConfig,
            IEnumerable<ISlackEventHandler> handlers,
            StoreAccessTokenCommand store,
            SlackHttp slackHttp)
        {
            _logger = logger;
            _config = config;
            _appConfig = appConfig;
            _handlers = handlers;
            _store = store;
            _slackHttp = slackHttp;
        }

        [HttpPost]
        [Route("api/slack/event")]
        public IActionResult GotEvent([FromBody] JsonElement j)
        {
            if (j.ValueKind == JsonValueKind.Undefined)
            {
                _logger.LogInformation("got event with {j}", j.ToString());
                return Ok();
            }

            foreach (var handler in _handlers)
            {
                if (handler.Match(j))
                {
                    return handler.Handle(j);
                }
            }

            return Ok();
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
        [Route("api/registered-to")]
        public IActionResult OauthRedirect([FromQuery] string id, [FromQuery] string name)
        {
            _logger.LogInformation("Registered Slack Bot User! {Id} {Team}", id, name);
            return Ok(new {success = true, id, name});
        }

        [HttpGet]
        [Route("api/redirect-uri")]
        public async Task<IActionResult> OauthRedirect([FromQuery] string code)
        {
            _logger.LogInformation("Got Redirect! {Code}", code);
            var res = await _slackHttp.OauthAccessAsync(_config.Value.ClientId, _config.Value.ClientSecret, code);
            if (res.Ok)
            {
                _logger.LogInformation("Registered! {Slack}", JsonSerializer.Serialize(res));
                await _store.StoreToken(res);
                return Redirect($"/api/registered-to/?id={res.Team.Id}&name={res.Team.Name}");
            }

            return BadRequest(res);
        }
    }
}