    using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using UniShare.Models;
using UniShare.Services;
using MongoDB.Bson;

namespace UniShare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMongoCollection<Assignment> _assignments;
        private readonly IMongoCollection<Course> _courses;

        public AssignmentsController(MongoDbService mongoDbService)
        {
            _assignments = mongoDbService.GetCollection<Assignment>("assignments");
            _courses = mongoDbService.GetCollection<Course>("courses");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromBody] Assignment assignment)
        {
            if (assignment == null || string.IsNullOrEmpty(assignment.CourseId))
                return BadRequest("Invalid assignment data.");

            assignment.Id = ObjectId.GenerateNewId().ToString();
            assignment.CreatedAt = DateTime.UtcNow;

            await _assignments.InsertOneAsync(assignment);

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, assignment.CourseId);
            var courseUpdate = Builders<Course>.Update.Push(c => c.Assignments, assignment.Id);
            var updateResult = await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            if (updateResult.ModifiedCount == 0)
                return NotFound($"Course with ID '{assignment.CourseId}' not found.");

            return Ok(new
            {
                message = "Assignment created successfully",
                assignment
            });
        }

        [HttpGet("get_assignments/{courseId}")]
        public async Task<IActionResult> GetAssignments(string courseId)
        {
            var assignments = await _assignments.Find(a => a.CourseId == courseId).ToListAsync();
            return Ok(new { assignments });
        }

        [HttpPut("update_assignment/{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment(string assignmentId, [FromBody] Assignment updatedData)
        {
            var filter = Builders<Assignment>.Filter.Eq(a => a.Id, assignmentId);
            var update = Builders<Assignment>.Update
                .Set(a => a.AssignmentName, updatedData.AssignmentName)
                .Set(a => a.AssignmentNo, updatedData.AssignmentNo)
                .Set(a => a.Details, updatedData.Details)
                .Set(a => a.Deadline, updatedData.Deadline)
                .Set(a => a.Uploader, updatedData.Uploader)
                .Set(a => a.Data, updatedData.Data);

            var result = await _assignments.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return NotFound(new { error = "Assignment not found or no changes made." });

            return Ok(new { message = "Assignment updated successfully" });
        }

        [HttpGet("get_assignment/{assignmentId}")]
        public async Task<IActionResult> GetAssignment(string assignmentId)
        {
            var assignment = await _assignments.Find(a => a.Id == assignmentId).FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound(new { error = "Assignment not found" });

            return Ok(assignment);
        }

        [HttpDelete("delete/{courseId}/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(string courseId, string assignmentId)
        {
            var deleteResult = await _assignments.DeleteOneAsync(a => a.Id == assignmentId);

            if (deleteResult.DeletedCount == 0)
                return NotFound(new { error = "Assignment not found" });

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, courseId);
            var courseUpdate = Builders<Course>.Update.Pull(c => c.Assignments, assignmentId);
            await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            return Ok(new { message = "Assignment deleted successfully" });
        }
    }
}
