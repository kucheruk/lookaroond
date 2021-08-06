using System;
using MongoDB.Bson.Serialization.Attributes;

namespace lookaroond.DB
{
    [BsonIgnoreExtraElements]
    public class Quiz
    {
        [BsonId] public string Id { get; set; }

        public string AuthorId { get; set; }

        public string TeamId { get; set; }

        public string Subject { get; set; }

        public int Version { get; set; }

        public DateTime CreatedTime { get; set; }

        public string[] ForUsers { get; set; }
    }
}