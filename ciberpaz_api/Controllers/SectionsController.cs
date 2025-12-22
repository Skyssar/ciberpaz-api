using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using ciberpaz_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ciberpaz_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;

        public SectionsController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: api/Sections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> GetSections()
        {
            return await _context.Sections.ToListAsync();
        }

        // GET: api/Sections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetSection(int id)
        {
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null)
                return NotFound();

            var dto = new SectionDto
            {
                Id = section.Id,
                Title = section.Title,
                Content = section.Content,
                Image = section.Image,
                Link = section.Link, 
                AppViewId = section.AppViewId
            };

            return dto;
        }

        // PUT: api/Sections/5
        [ApiKey]
        [HttpPatch("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateSection(int id, [FromForm] SectionUpdateDto dto)
        {
            var item = await _context.Sections
           .FirstOrDefaultAsync(v => v.Id == id);

            if (item == null)
                return NotFound();

            if (dto.Title != null)
                item.Title = dto.Title;

            if (dto.Content != null){
                item.Content = dto.Content;
            }

            if (dto.Link != null)
                item.Link = dto.Link;
            
            item.Image = await _imageService.UpdateImageAsync(dto.Image, item.Image, "images");

            // Guardar cambios
            await _context.SaveChangesAsync();

            var response = new SectionDto
            {
                Id = item.Id,
                Title = item.Title,
                Content = item.Content,
                Image = item.Image,
                Link = item.Link,
                AppViewId = item.AppViewId
            };

            return Ok(response);
        }
      
        // POST: api/Sections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateSection([FromForm] SectionCreateDto dto)
        {
            var view = await _context.AppViews.FindAsync(dto.AppViewId);
            if (view == null)
                return NotFound("View not found");

            var imagePath = await _imageService.SaveImageAsync(dto.Image, "images");

            var section = new Section
            {
                Title = dto.Title,
                Content = dto.Content,
                Image = imagePath,
                Link = dto.Link,
                AppViewId = dto.AppViewId
            };

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            var response = new SectionDto
            {
                Id = section.Id,
                Title = section.Title,
                Content = section.Content,
                Image = section.Image,
                Link = section.Link,
                AppViewId = section.AppViewId
            };

            return CreatedAtAction(nameof(GetSection), new { id = section.Id }, response);
        }

        // DELETE: api/Sections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SectionExists(int id)
        {
            return _context.Sections.Any(e => e.Id == id);
        }
    }
}
