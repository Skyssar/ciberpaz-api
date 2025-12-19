using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using ciberpaz_api.Services;
using Humanizer;
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
    public class AppViewsController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Views
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppViewListDto>>> GetViews()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var views = await _context.AppViews
                .Select(v => new AppViewListDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Image = $"{baseUrl}/{v.Image}",
                    Route = v.Route,
                })
                .ToListAsync();

            return views;
        }

        // GET: api/Views/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppViewDto>> GetView(int id)
        {
            var view = await _context.AppViews
                .Include(v => v.Sections)
                .Include(v => v.Paragraphs)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (view == null)
                return NotFound();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var dto = new AppViewDto
            {
                Id = view.Id,
                Title = view.Title,
                Image = $"{baseUrl}/{view.Image}",
                Route = view.Route,
                Sections = view.Sections?.Select(s => new SectionDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    Image = $"{baseUrl}/{s.Image}",
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
        public async Task<IActionResult> Update(int id, [FromForm] AppViewUpdateDto dto)
        {
            var view = await _context.AppViews.FindAsync(id);
            if (view == null)
                return NotFound();

            // Actualizar el título si viene
            if (!string.IsNullOrEmpty(dto.Title))
                view.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Route))
                view.Route = dto.Route;

            var newImagePath = await _imageService.UpdateImageAsync(dto.Image, view.Image, "images");

            view.Image = newImagePath;
            
            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(view);
        }

        // POST: api/Views
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateView([FromForm] AppViewCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Image, "images");

            if (imagePath == null)
                return BadRequest("Debe enviar una imagen");

            var view = new AppView
            {
                Title = dto.Title,
                Image = imagePath,
                Route = dto.Route,
            };

            _context.AppViews.Add(view);
            await _context.SaveChangesAsync();

            return Ok(view);
        }

        // DELETE: api/Views/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteView(int id)
        {
            var view = await _context.AppViews.FindAsync(id);
            if (view == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(view.Image);

            // ===== BORRAR REGISTRO =====
            _context.AppViews.Remove(view);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
