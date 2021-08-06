using System.Text.Json.Serialization;

namespace lookaroond.Controllers
{
    public class SlackOauthResponse
    {
        [JsonPropertyName("ok")] public bool Ok { get; set; }

        [JsonPropertyName("access_token")] public string AccessToken { get; set; }

        [JsonPropertyName("token_type")] public string TokenType { get; set; }

        [JsonPropertyName("scope")] public string Scope { get; set; }

        [JsonPropertyName("bot_user_id")] public string BotUserId { get; set; }

        [JsonPropertyName("app_id")] public string AppId { get; set; }

        [JsonPropertyName("team")] public SlackTeamInfoResponse Team { get; set; }

        [JsonPropertyName("enterprise")] public SlackEnterpriseResponse Enterprise { get; set; }

        [JsonPropertyName("authed_user")] public SlackAuthedUser AuthedUser { get; set; }
    }
}