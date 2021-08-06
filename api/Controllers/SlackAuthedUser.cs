using System.Text.Json.Serialization;

namespace lookaroond.Controllers
{
    public class SlackAuthedUser
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("scope")] public string Scope { get; set; }

        [JsonPropertyName("access_token")] public string AccessToken { get; set; }

        [JsonPropertyName("token_type")] public string TokenType { get; set; }
    }
}