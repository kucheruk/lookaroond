using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace lookaroond
{
    public class SlackRequestContainer
    {
        public string Raw { get; set; }
        public JsonElement Json { get; set; }
        public Dictionary<string, string> Form { get; set; }
        public HttpResponse Response { get; set; }
    }
}