using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UniShare.Models
{
    [BsonIgnoreExtraElements]
    public class Note
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }

        [JsonPropertyName("_id")]
        public string? _id => Id;

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("data")]
        public List<string> Data { get; set; } = new List<string>();

        [JsonPropertyName("uploader")]
        [BsonElement("uploader")]
        public NoteUploader? NoteUploader { get; set; }

        [BsonElement("course")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CourseId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public class NoteUploader
    {
        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
