using System;

namespace lookaroond
{
    public class SlackConfig
    {
        public string SigningSecret { get; set; }
        public string AppId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string VerificationToken { get; set; }
        public string[] Admins { get; set; }
        public string[] Scopes { get; set; }

        public void Assert()
        {
            if (string.IsNullOrEmpty(SigningSecret))
            {
                throw new ApplicationException("no Slack SigningSecret in configuration");
            }
        }
    }
}