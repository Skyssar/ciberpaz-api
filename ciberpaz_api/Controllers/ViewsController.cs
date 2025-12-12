using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using ciberpaz_api.Services;
using Humanizer;
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
    public class ViewsController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Views
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewListDto>>> GetViews()
        {
            var views = await _context.Views
                .Select(v => new ViewListDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Image = v.Image
                })
                .ToListAsync();

            return views;
        }

        // GET: api/Views/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViewDto>> GetView(int id)
        {
            var view = await _context.Views
                .Include(v => v.Sections)
                .Include(v => v.Paragraphs)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (view == null)
                return NotFound();

            var dto = new ViewDto
            {
                Id = view.Id,
                Title = view.Title,
                Image = view.Image,
                Sections = view.Sections?.Select(s => new SectionDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    Image = s.Image,
                    Link = s.Link
                }).ToList(),
                Paragraphs = view.Paragraphs?.Select(p => new ParagraphDto
                {
                    Id = p.Id,
                    Type = p.Type,
                    Content = p.Content
                }).ToList()
            };

            return dto;
        }

        // PUT: api/Views/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ViewUpdateDto dto)
        {
            var view = await _context.Views.FindAsync(id);
            if (view == null)
                return NotFound();

            // Actualizar el título si viene
            if (!string.IsNullOrEmpty(dto.Title))
                view.Title = dto.Title;

            var newImagePath = await _imageService.UpdateImageAsync(dto.Image, view.Image, "images");

            if (newImagePath == null)
                return BadRequest("Debe enviar una imagen");

            // Construir la nueva URL pública
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string newFileUrl = $"{baseUrl}/{newImagePath}";

            view.Image = newFileUrl;
            
            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(view);
        }

        // POST: api/Views
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateView([FromForm] ViewCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Image, "images");

            if (imagePath == null)
                return BadRequest("Debe enviar una imagen");

            // Construir URL pública para guardar en BD
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string fileUrl = $"{baseUrl}/{imagePath}";

            var view = new View
            {
                Title = dto.Title,
                Image = fileUrl
            };

            _context.Views.Add(view);
            await _context.SaveChangesAsync();

            return Ok(view);
        }

        // DELETE: api/Views/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteView(int id)
        {
            var view = await _context.Views.FindAsync(id);
            if (view == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(view.Image, "images");

            // ===== BORRAR REGISTRO =====
            _context.Views.Remove(view);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
