using System;

namespace lookaroond.DB
{
    public class AppConfig
    {
        public string MongoConnectionString { get; set; }
        public string MongoDbSuffix { get; set; }
    }

    public class SlackConfig
    {
        public string AccessToken { get; set; }
        public string SigningSecret { get; set; }

        public void Assert()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                throw new ApplicationException("no Slack AccessToken in configuration");
            }
            if (string.IsNullOrEmpty(SigningSecret))
            {
                throw new ApplicationException("no Slack SigningSecret in configuration");
            }
        }
    }
}