using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace lookaroond.Controllers
{
    public class SlackHttp
    {
        private static readonly Uri Base = new("https://slack.com");
        private readonly HttpClient _client;
        private readonly ILogger<SlackHttp> _logger;

        public SlackHttp(HttpClient client, ILogger<SlackHttp> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<SlackOauthResponse> OauthAccessAsync(string clientId, string clientSecret,
            string code)
        {
            var resp = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Base, $"/api/oauth.v2.access?client_id={clientId}&client_secret={clientSecret}&code={code}")
            });
            _logger.LogInformation("Send /api/oauth.v2.access {ClientId} {ClientSecret} {Code}", clientId, clientSecret, code);
            _logger.LogInformation("response {Body}", await resp.Content.ReadAsStringAsync());
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<SlackOauthResponse>();
            }

            _logger.LogError("Error authorizing {Body}", await resp.Content.ReadAsStringAsync());
            throw new OAuthFailedException();
        }
    }
}