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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ciberpaz_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Volunteers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VolunteerDto>>> GetVolunteers()
        {
            var items = await _context.Volunteers
                .Select(v => new VolunteerDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Email = v.Email,
                    Phone = v.Phone
                })
                .ToListAsync();

            return items;
        }

        // POST: api/Volunteers
        [HttpPost]
        public async Task<ActionResult> CreateVolunteer([FromBody] VolunteerUpdateCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = new Volunteer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
            };

            _context.Volunteers.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // PUT: api/Volunteers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVolunteer(int id, [FromBody] VolunteerUpdateCreateDto dto)
        {
            var item = await _context.Volunteers
           .FirstOrDefaultAsync(v => v.Id == id);

            if (item == null)
                return NotFound();

            item.Name = dto.Name;
            item.Email = dto.Email;
            item.Phone = dto.Phone;

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/Volunteers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVolunteer(int id)
        {
            var item = await _context.Volunteers.FindAsync(id);
            if (item == null)
                return NotFound();

            // ===== BORRAR REGISTRO =====
            _context.Volunteers.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
