using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UniShare.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("courseName")]
        public string CourseName { get; set; } = string.Empty;

        [BsonElement("section")]
        public string Section { get; set; } = string.Empty;

        [BsonElement("semester")]
        public int Semester { get; set; }

        [BsonElement("courseCode")]
        public string CourseCode { get; set; } = "Not Provided";

        [BsonElement("faculty")]
        public string Faculty { get; set; } = string.Empty;

        [BsonElement("assignments")]
        public List<string> Assignments { get; set; } = new List<string>();

        [BsonElement("labs")]
        public List<string> Labs { get; set; } = new List<string>();

        [BsonElement("notes")]
        public List<string> Notes { get; set; } = new List<string>();

        [BsonElement("notices")]
        public List<Notice> Notices { get; set; } = new List<Notice>();
    }

    public class Notice
    {
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("notice")]
        public string NoticeText { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
