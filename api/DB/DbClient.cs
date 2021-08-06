using lookaroond.DB.Commands;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace lookaroond.DB
{
    public class DbClient
    {
        private readonly IOptions<AppConfig> _cfg;
        private MongoClient _mc;

        public DbClient(IOptions<AppConfig> cfg)
        {
            _cfg = cfg;
        }

        public IMongoCollection<Quiz> Quizes { get; set; }

        public IMongoCollection<QuizTemplate> QuizTemplates { get; set; }

        public IMongoCollection<QuestionTemplate> QuestionTemplates { get; set; }

        public IMongoCollection<Question> Questions { get; set; }


        public IMongoCollection<Response> Responses { get; set; }
        public IMongoCollection<SlackAccessToken> Access { get; set; }

        public void Start()
        {
            _mc = new MongoClient(_cfg.Value.MongoConnectionString);
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
            Responses = Collection<Response>("responses");
            Questions = Collection<Question>("questions");
            QuestionTemplates = Collection<QuestionTemplate>("questionTemplates");
            QuizTemplates = Collection<QuizTemplate>("quizTemplates");
            Quizes = Collection<Quiz>("quizes");
            Access = Collection<SlackAccessToken>("access");
            Access.Indexes.CreateOneAsync(new CreateIndexModel<SlackAccessToken>(Builders<SlackAccessToken>.IndexKeys.Ascending(a => a.Access.AppId)));
            Access.Indexes.CreateOneAsync(new CreateIndexModel<SlackAccessToken>(Builders<SlackAccessToken>.IndexKeys.Ascending(a => a.Access.Team.Id)));
        }

        public IMongoCollection<T> Collection<T>(string name)
        {
            return Db().GetCollection<T>(name);
        }

        public IMongoDatabase Db()
        {
            return _mc.GetDatabase("lookaroond" + _cfg.Value.MongoDbSuffix);
        }

        public void Stop()
        {
        }
    }
}