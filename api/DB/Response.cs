using System;
using MongoDB.Bson.Serialization.Attributes;

namespace lookaroond.DB
{
    [BsonIgnoreExtraElements]
    public class Response
    {
        [BsonId]
        public string Id { get; set; }
        
        public string TeamId { get; set; }
        
        public string QuizId { get; set; }
        
        public string QuestionId { get; set; }
        
        public string UserId { get; set; }
        
        public int? Number { get; set; }
        
        public string Text { get; set; }
        
        public DateTime ResponseTime { get; set; }
        
        
    }
}