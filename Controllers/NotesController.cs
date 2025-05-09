using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using UniShare.Models;
using UniShare.Services;

namespace UniShare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IMongoCollection<Note> _notes;
        private readonly IMongoCollection<Course> _courses;

        public NotesController(MongoDbService mongoDbService)
        {
            _notes = mongoDbService.GetCollection<Note>("notes");
            _courses = mongoDbService.GetCollection<Course>("courses");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            if (note == null || string.IsNullOrEmpty(note.CourseId))
                return BadRequest("Invalid note data.");

            note.Id = ObjectId.GenerateNewId().ToString();
            note.CreatedAt = DateTime.UtcNow;

            await _notes.InsertOneAsync(note);

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, note.CourseId);
            var courseUpdate = Builders<Course>.Update.Push(c => c.Notes, note.Id);
            var updateResult = await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            if (updateResult.ModifiedCount == 0)
                return NotFound($"Course with ID '{note.CourseId}' not found.");

            return Ok(new
            {
                message = "Note created successfully",
                note
            });
        }

        [HttpGet("get_notes/{courseId}")]
        public async Task<IActionResult> GetNotes(string courseId)
        {
            var notes = await _notes.Find(n => n.CourseId == courseId).ToListAsync();
            return Ok(new { notes });
        }

        [HttpGet("get_note/{noteId}")]
        public async Task<IActionResult> GetNote(string noteId)
        {
            var note = await _notes.Find(n => n.Id == noteId).FirstOrDefaultAsync();
            if (note == null)
                return NotFound(new { error = "Note not found" });

            return Ok(note);
        }

        [HttpPut("update_note/{noteId}")]
        public async Task<IActionResult> UpdateNote(string noteId, [FromBody] Note updatedData)
        {
            var filter = Builders<Note>.Filter.Eq(n => n.Id, noteId);
            var update = Builders<Note>.Update
                .Set(n => n.Title, updatedData.Title)
                .Set(n => n.Description, updatedData.Description)
                .Set(n => n.Data, updatedData.Data)
                .Set(n => n.Uploader, updatedData.Uploader);

            var result = await _notes.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
                return NotFound(new { error = "Note not found or no changes made." });

            return Ok(new { message = "Note updated successfully" });
        }

        [HttpDelete("delete/{courseId}/{noteId}")]
        public async Task<IActionResult> DeleteNote(string courseId, string noteId)
        {
            var deleteResult = await _notes.DeleteOneAsync(n => n.Id == noteId);

            if (deleteResult.DeletedCount == 0)
                return NotFound(new { error = "Note not found" });

            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, courseId);
            var courseUpdate = Builders<Course>.Update.Pull(c => c.Notes, noteId);
            await _courses.UpdateOneAsync(courseFilter, courseUpdate);

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}
