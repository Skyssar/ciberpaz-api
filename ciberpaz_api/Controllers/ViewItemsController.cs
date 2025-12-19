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
    public class ViewItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;

        public ViewItemsController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;

        }

        // GET: api/ViewItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViewItemDto>> GetViewItem(int id)
        {
            var viewItem = await _context.ViewItems.FindAsync(id);

            if (viewItem == null)
                return NotFound();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var dto = new ViewItemDto
            {
                Id = viewItem.Id,
                Title = viewItem.Title,
                Icon = $"{baseUrl}/{viewItem.Icon}",
                WebUrl = viewItem.WebUrl,
                Embed = viewItem.Embed,
                Order = viewItem.Order,
                Route = viewItem.Route
            };

            return dto;
        }

        // POST: api/ViewItems
        [ApiKey]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ViewItemDto>> CreateViewItem([FromForm] ViewItemCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImageAsync(dto.Icon, "icons");

            if (imagePath == null)
                return BadRequest("Debe enviar una imagen");

            var viewItem = new ViewItem
            {
                Title = dto.Title,
                Icon = imagePath,
                WebUrl = dto.WebUrl,
                Embed = dto.Embed,
                Order = dto.Order,
                Route = dto.Route

            };

            _context.ViewItems.Add(viewItem);
            await _context.SaveChangesAsync();

            return Ok(viewItem);
        }

        // PUT: api/ViewItems/5
        [ApiKey]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateViewItem(int id, [FromForm] ViewItemUpdateDto dto)
        {
            var item = await _context.ViewItems
           .FirstOrDefaultAsync(v => v.Id == id);

            if (item == null)
                return NotFound();

            item.Title = dto.Title;
            item.Route = dto.Route;
            item.WebUrl = dto.WebUrl;
            item.Embed = dto.Embed;
            item.Order = dto.Order;

            item.Icon = await _imageService.UpdateImageAsync(dto.Icon, item.Icon, "icons");

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/ViewItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteViewItem(int id)
        {
            var item = await _context.ViewItems.FindAsync(id);
            if (item == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            _imageService.DeleteImage(item.Icon);

            // ===== BORRAR REGISTRO =====
            _context.ViewItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
