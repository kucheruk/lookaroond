using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace lookaroond
{
    public class CheckSlackSignatureMiddleware
    {
        private readonly ILogger<CheckSlackSignatureMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly SlackRequestContainer _req;
        private readonly SlackRequestSignature _slackSignature;

        public CheckSlackSignatureMiddleware(RequestDelegate next,
            ILogger<CheckSlackSignatureMiddleware> logger,
            SlackRequestSignature slackSignature,
            SlackRequestContainer req)
        {
            _next = next;
            _logger = logger;
            _slackSignature = slackSignature;
            _req = req;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var bodyAsText = await context.ReadRequestBodyAsString();
            _req.Raw = bodyAsText;

            if (RequestNeedSignature(request))
            {
                if (SignatureKeyIsValid(bodyAsText, request))
                {
                    _logger.LogInformation("Got {Request} From Slack", bodyAsText);
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("{\"error\":\"signature_invalid\"}");
                }
            }
            else
            {
                await _next(context);
            }
        }

        private bool RequestNeedSignature(HttpRequest request)
        {
            return request.Path.Value != null && request.Path.Value.Contains("/api/slack/");
        }

        private bool SignatureKeyIsValid(string bodyAsText, HttpRequest request)
        {
            if (!request.Headers.TryGetValue("X-Slack-Request-Timestamp", out var timestamp))
            {
                return false;
            }

            return request.Headers.TryGetValue("X-Slack-Signature", out var signature)
                   && _slackSignature.Validate(bodyAsText, timestamp, signature);
        }
    }
}