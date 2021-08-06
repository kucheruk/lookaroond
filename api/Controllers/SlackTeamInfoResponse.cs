using System.Text.Json.Serialization;

namespace lookaroond.Controllers
{
    public class SlackTeamInfoResponse
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("id")] public string Id { get; set; }
    }
}