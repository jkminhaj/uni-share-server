using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UniShare.Models
{
    [BsonIgnoreExtraElements]
    public class Assignment
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] 
        [JsonIgnore]
        public string? Id { get; set; }

        [JsonPropertyName("_id")]  
        public string? _id => Id;

        [BsonElement("assignmentName")]
        public string? AssignmentName { get; set; }

        [BsonElement("assignmentNo")] 
        public int AssignmentNo { get; set; }

        [BsonElement("details")] 
        public string? Details { get; set; }

        
        [BsonElement("data")]
        public List<string> Data { get; set; } = new List<string>();

        [BsonElement("deadline")] 
        public DateTime Deadline { get; set; }

        [BsonElement("uploader")] 
        public Uploader? Uploader { get; set; }

        
        [BsonElement("course")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CourseId { get; set; }

        [BsonElement("createdAt")] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    
    public class Uploader
    {
        [BsonElement("name")] 
        public string? Name { get; set; }

        [BsonElement("email")] 
        public string? Email { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
