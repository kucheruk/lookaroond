using System;
using MongoDB.Bson.Serialization.Attributes;

namespace lookaroond.DB
{
    public class QuizTemplate
    {
        [BsonId] public string Id { get; set; }

        public string AuthorId { get; set; }

        public string TeamId { get; set; }

        public string Subject { get; set; }

        public DateTime CreatedTime { get; set; }

        public int Version { get; set; }

        public bool Ordered { get; set; }
    }
}