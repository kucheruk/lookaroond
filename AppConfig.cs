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
    }
}