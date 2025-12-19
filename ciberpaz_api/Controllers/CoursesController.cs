using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using ciberpaz_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ciberpaz_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var items = await _context.Courses
                .Select(v => new CourseDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Image = v.Image,
                    Description = v.Description,
                    Link = v.Link,
                })
                .ToListAsync();

            return items;
        }

        // POST: api/Courses
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateCourse([FromForm] CourseUpdateCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Image, "images");

            var item = new Course
            {
                Title = dto.Title,
                Image = imagePath,
                Description = dto.Description,
                Link = dto.Link
            };

            _context.Courses.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateCourse(int id, [FromForm] CourseUpdateCreateDto dto)
        {
            var item = await _context.Courses
           .FirstOrDefaultAsync(v => v.Id == id);

            if (item == null)
                return NotFound();

            item.Title = dto.Title;
            item.Description = dto.Description;
            item.Link = dto.Link;

            item.Image = await _imageService.UpdateImageAsync(dto.Image, item.Image, "images");

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var item = await _context.Courses.FindAsync(id);
            if (item == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(item.Image);

            // ===== BORRAR REGISTRO =====
            _context.Courses.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
