using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using UniShare.Models;
using UniShare.Services;
using System ;
namespace UniShare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabsController : ControllerBase
    {
        private readonly IMongoCollection<Lab> _labs;
        private readonly IMongoCollection<Course> _courses;

        public LabsController(MongoDbService mongoDbService)
        {
            _labs = mongoDbService.GetCollection<Lab>("labs");
            _courses = mongoDbService.GetCollection<Course>("courses");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLab([FromBody] Lab lab)
        {
            if (lab == null || string.IsNullOrEmpty(lab.CourseId))
                return BadRequest("Invalid lab data.");

            lab.Id = ObjectId.GenerateNewId().ToString();
            lab.CreatedAt = DateTime.UtcNow;

            await _labs.InsertOneAsync(lab);

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, lab.CourseId);
            var courseUpdate = Builders<Course>.Update.Push("labs", lab.Id);
            var updateResult = await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            if (updateResult.MatchedCount == 0)
                return NotFound($"Course with ID '{lab.CourseId}' not found.");

            return Ok(new
            {
                message = "Lab created successfully",
                lab
            });
        }

        [HttpGet("get_labs/{courseId}")]
        public async Task<IActionResult> GetLabs(string courseId)
        {
            var labs = await _labs.Find(l => l.CourseId == courseId).ToListAsync();
            return Ok(new { labs });
        }

        [HttpGet("get_lab/{labId}")]
        public async Task<IActionResult> GetLab(string labId)
        {
            var lab = await _labs.Find(l => l.Id == labId).FirstOrDefaultAsync();
            if (lab == null)
                return NotFound(new { error = "Lab not found" });

            return Ok(lab);
        }

        [HttpPut("update_lab/{labId}")]
        public async Task<IActionResult> UpdateLab(string labId, [FromBody] Lab updatedLab)
        {
            var filter = Builders<Lab>.Filter.Eq(l => l.Id, labId);
            var update = Builders<Lab>.Update
                .Set(l => l.LabName, updatedLab.LabName)
                .Set(l => l.LabNo, updatedLab.LabNo)
                .Set(l => l.Details, updatedLab.Details)
                .Set(l => l.Deadline, updatedLab.Deadline)
                .Set(l => l.Data, updatedLab.Data)
                .Set(l => l.LabUploader, updatedLab.LabUploader);

            var result = await _labs.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return NotFound(new { error = "Lab not found or no changes made." });

            return Ok(new { message = "Lab updated successfully" });
        }

        [HttpDelete("delete/{courseId}/{labId}")]
        public async Task<IActionResult> DeleteLab(string courseId, string labId)
        {
            var deleteResult = await _labs.DeleteOneAsync(l => l.Id == labId);

            if (deleteResult.DeletedCount == 0)
                return NotFound(new { error = "Lab not found" });

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, courseId);
            var courseUpdate = Builders<Course>.Update.Pull("labs", labId);
            await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            return Ok(new { message = "Lab deleted successfully" });
        }
    }
}
    