using System;
using MongoDB.Bson.Serialization.Attributes;

namespace lookaroond.DB
{
    public class QuestionTemplate
    {
        [BsonId]
        public string Id { get; set; }
        
        public string TeamId { get; set; }
        
        public string AuthorId { get; set; }
        
        public string UserId { get; set; }
        
        public string QuizTemplateId { get; set; }
        
        public DateTime CreatedTime { get; set; }
        
        public string Text { get; set; }
        
        public int Order { get; set; }
        
        public  ResponseType ResponseType {get;set;}
        
        public string[] Options { get; set; }

    }
}