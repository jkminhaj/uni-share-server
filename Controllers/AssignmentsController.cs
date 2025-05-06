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

        // POST: api/assignments/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromBody] Assignment assignment)
        {
            if (assignment == null || string.IsNullOrEmpty(assignment.CourseId))
                return BadRequest("Invalid assignment data.");

            await _assignments.InsertOneAsync(assignment);

            // Update the course to push the new assignment ID
            var filter = Builders<Course>.Filter.Eq(c => c.Id, assignment.CourseId);
            var update = Builders<Course>.Update.Push(c => c.Assignments, assignment.Id);
            await _courses.UpdateOneAsync(filter, update);

            return Ok(new { message = "Assignment created successfully", assignment });
        }

        // GET: api/assignments/get_assignments/{courseId}
        [HttpGet("get_assignments/{courseId}")]
        public async Task<IActionResult> GetAssignments(string courseId)
        {
            var assignments = await _assignments.Find(a => a.CourseId == courseId).ToListAsync();
            return Ok(new { assignments });
        }

        // PUT: api/assignments/update_assignment/{assignmentId}
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

        // DELETE: api/assignments/delete/{courseId}/{assignmentId}
        [HttpDelete("delete/{courseId}/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(string courseId, string assignmentId)
        {
            var deleteResult = await _assignments.DeleteOneAsync(a => a.Id == assignmentId);

            if (deleteResult.DeletedCount == 0)
                return NotFound(new { error = "Assignment not found" });

            // Remove assignment reference from the course
            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, courseId);
            var courseUpdate = Builders<Course>.Update.Pull(c => c.Assignments, assignmentId);
            await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            return Ok(new { message = "Assignment deleted successfully" });
        }
    }
}
