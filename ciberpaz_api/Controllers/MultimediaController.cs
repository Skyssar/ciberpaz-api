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
    public class MultimediaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;

        public MultimediaController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;

        }

        // GET: api/Multimedia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MultimediaDto>>> GetMultimedias()
        {
            var views = await _context.Multimedias
                .Select(v => new MultimediaDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Type = v.Type,
                    Icon = v.Icon,
                    Link = v.Link,
                })
                .ToListAsync();

            return views;
        }

        // GET: api/Multimedia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MultimediaDto>> GetMultimedia(int id)
        {
            var media = await _context.Multimedias.FindAsync(id);

            if (media == null)
                return NotFound();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var dto = new MultimediaDto
            {
                Id = media.Id,
                Title = media.Title,
                Type = media.Type,
                Icon = $"{baseUrl}/{media.Icon}",
                Link = media.Link,
            };

            return dto;
        }

        // POST: api/Multimedia
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateMultimedia([FromForm] MultimediaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Icon, "icons");

            if (imagePath == null)
                return BadRequest("Debe enviar una imagen");

            var media = new Multimedia
            {
                Title = dto.Title,
                Icon = imagePath,
                Type = dto.Type,
                Link = dto.Link
            };

            _context.Multimedias.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        // PUT: api/ViewItems/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateMultimedia(int id, [FromForm] MultimediaUpdateDto dto)
        {
            var media = await _context.Multimedias
           .FirstOrDefaultAsync(v => v.Id == id);

            if (media == null)
                return NotFound();

            media.Title = dto.Title;
            media.Type = dto.Type;
            media.Link = dto.Link;

            media.Icon = await _imageService.UpdateImageAsync(dto.Icon, media.Icon, "icons");

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        // DELETE: api/Multimedia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMultimedia(int id)
        {
            var item = await _context.Multimedias.FindAsync(id);
            if (item == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(item.Icon);

            // ===== BORRAR REGISTRO =====
            _context.Multimedias.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
