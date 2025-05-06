using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UniShare.Models
{
    [BsonIgnoreExtraElements]  // Ignores extra elements that may be present in the BSON but not in the C# model
    public class Assignment
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]  // Stores ObjectId as string for easier handling
        [JsonIgnore]  // Ignores this property during JSON serialization
        public string? Id { get; set; }

        [JsonPropertyName("_id")]  // Maps the _id field in the MongoDB document to the C# Id field
        public string? _id => Id;

        [BsonElement("assignmentName")]  // Maps to assignmentName field in MongoDB document
        public string? AssignmentName { get; set; }

        [BsonElement("assignmentNo")]  // Maps to assignmentNo field in MongoDB document
        public int AssignmentNo { get; set; }

        [BsonElement("details")]  // Maps to details field in MongoDB document
        public string? Details { get; set; }

        // List of assignment data (e.g., thumbnail and view links)
        [BsonElement("data")]
        public List<string> Data { get; set; } = new List<string>();

        [BsonElement("deadline")]  // Maps to deadline field in MongoDB document
        public DateTime Deadline { get; set; }

        [BsonElement("uploader")]  // Maps to uploader field in MongoDB document
        public Uploader? Uploader { get; set; }

        [BsonElement("course")]  // References the course this assignment belongs to (ObjectId)
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CourseId { get; set; }

        [BsonElement("createdAt")]  // Date when the assignment was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Represents the uploader's information
    public class Uploader
    {
        [BsonElement("name")]  // Name of the uploader
        public string? Name { get; set; }

        [BsonElement("email")]  // Email of the uploader
        public string? Email { get; set; }

        [BsonElement("image")]  // Profile image of the uploader (optional)
        public string? Image { get; set; }
    }

    // Represents the assignment data (e.g., files associated with the assignment)
    public class AssignmentData
    {
        [BsonElement("thumbnailLink")]  // Link to the thumbnail image
        public string? ThumbnailLink { get; set; }

        [BsonElement("viewLink")]  // Link to view the full assignment file
        public string? ViewLink { get; set; }
    }
}
