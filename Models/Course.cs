using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UniShare.Models
{
    [BsonIgnoreExtraElements]
    public class Course
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [JsonPropertyName("_id")]
        public string? _id => Id;

        [BsonElement("courseName")]
        public string? CourseName { get; set; }

        [BsonElement("section")]
        public string? Section { get; set; }

        [BsonElement("semester")]
        public int Semester { get; set; }

        [BsonElement("courseCode")]
        public string? CourseCode { get; set; } = "Not Provided";

        [BsonElement("faculty")]
        public string? Faculty { get; set; }

        [BsonElement("assignments")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Assignments { get; set; } = new List<string>();

        [BsonElement("labs")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Labs { get; set; } = new List<string>();

        [BsonElement("notes")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Notes { get; set; } = new List<string>();

        [BsonElement("notices")]
        public List<Notice> Notices { get; set; } = new List<Notice>();
    }

    public class Notice
    {
        [BsonElement("email")]
        public string? Email { get; set; }

        [JsonPropertyName("notice")]    
        [BsonElement("notice")]
        public string? NoticeText { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
