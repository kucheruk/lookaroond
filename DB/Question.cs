using System;
using MongoDB.Bson.Serialization.Attributes;

namespace lookaroond.DB
{
    [BsonIgnoreExtraElements]
    public class Question
    {
        [BsonId]
        public string Id { get; set; }
        
        public string TeamId { get; set; }
        
        public string QuestionTemplateId { get; set; }
        
        public string AuthorId { get; set; }
        
        public string UserId { get; set; }
        
        public string QuizId { get; set; }
        
        public DateTime CreatedTime { get; set; }
        
        public string Text { get; set; }
        
        public  ResponseType ResponseType {get;set;}
        
        public string[] Options { get; set; }
        
        public QuestionState State { get; set; }
    }
}