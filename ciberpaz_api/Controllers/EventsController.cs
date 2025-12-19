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
    public class EventsController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventsByTypeDto>>> GetEventsGroupedByType()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var result = await _context.Events
            .AsNoTracking()
            .GroupBy(e => e.Type)
            .Select(g => new EventsByTypeDto
            {
                Type = g.Key,
                Events = g.Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Type = e.Type,
                    Image = $"{baseUrl}/{e.Image}",
                    Link = e.Link
                }).ToList()
            })
            .OrderBy(g => g.Type)
            .ToListAsync();

             return Ok(result);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var item = await _context.Events.FindAsync(id);

            if (item == null)
                return NotFound();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var dto = new EventDto
            {
                Id = item.Id,
                Title = item.Title,
                Type = item.Type,
                Image = $"{baseUrl}/{item.Image}",
                Date = item.Date,
                Link = item.Link,
            };

            return dto;
        }

        // POST: api/Events
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateEvent([FromForm] EventCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Image, "images");

            var item = new Event
            {
                Title = dto.Title,
                Image = imagePath,
                Type = dto.Type,
                Date = dto.Date,
                Link = dto.Link
            };

            _context.Events.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateEvent(int id, [FromForm] EventUpdateDto dto)
        {
            var item = await _context.Events
           .FirstOrDefaultAsync(v => v.Id == id);

            if (item == null)
                return NotFound();

            item.Title = dto.Title;
            item.Type = dto.Type;
            item.Link = dto.Link;

            if (dto.Date != null)
                item.Date = dto.Date ?? new DateTime();


            item.Image = await _imageService.UpdateImageAsync(dto.Image, item.Image, "images");

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var item = await _context.Events.FindAsync(id);
            if (item == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(item.Image);

            // ===== BORRAR REGISTRO =====
            _context.Events.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
