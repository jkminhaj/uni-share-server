using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using UniShare.Models;
using UniShare.Services ;

namespace UniShare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IMongoCollection<Course> _courses;

        public CoursesController(MongoDbService mongoDbService)
        {
            _courses = mongoDbService.GetCollection<Course>("courses");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest("Invalid course data.");
            }

            await _courses.InsertOneAsync(course);
            return Ok(new { message = "Course created successfully", course });
        }

        [HttpGet("get_course/{id}")]
        public async Task<IActionResult> GetCourse(string id)
        {
            var course = await _courses.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound(new { error = "Course not found" });
            }

            return Ok(course);
        }
        
        [HttpGet("get_all_courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courses.Find(_ => true).ToListAsync();
            return Ok(new { courses });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var deleteResult = await _courses.DeleteOneAsync(c => c.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                return NotFound(new { error = "Course not found" });
            }

            return Ok(new { message = "Course deleted successfully" });
        }

        [HttpPost("add_notice/{id}")]
        public async Task<IActionResult> AddNotice(string id, [FromBody] Notice notice)
        {
            if (notice == null || string.IsNullOrEmpty(notice.Email) || string.IsNullOrEmpty(notice.NoticeText))
            {
                return BadRequest(new { error = "Notice must have email and text" });
            }

            var course = await _courses.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound(new { error = "Course not found" });
            }

            notice.Date = DateTime.UtcNow;
            course.Notices.Add(notice);

            await _courses.ReplaceOneAsync(c => c.Id == id, course);

            return Ok(new { message = "Notice added successfully", course });
        }
    }
}
