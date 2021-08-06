using System.Text.Json;
using lookaroond.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace lookaroond
{
    public class SlackEventChallengeHandler : ISlackEventHandler
    {
        private readonly ILogger<SlackEventChallengeHandler> _logger;
        private JsonElement _challenge;

        public SlackEventChallengeHandler(ILogger<SlackEventChallengeHandler> logger)
        {
            _logger = logger;
        }

        public bool Match(JsonElement e)
        {
            if (e.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            if (!e.TryGetProperty("type", out var type))
            {
                return false;
            }

            if (type.GetString() != "url_verification")
            {
                return false;
            }

            if (!e.TryGetProperty("challenge", out var challenge))
            {
                return false;
            }

            _challenge = challenge;
            _logger.LogInformation("Got Event Subscription challenge!");
            return true;
        }

        public IActionResult Handle(JsonElement e)
        {
            return new OkObjectResult(new {challenge = _challenge.GetString()});
        }
    }
}