using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ciberpaz_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MenuItemsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/MenuItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            return await _context.MenuItems.ToListAsync();
        }

        // GET: api/MenuItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return menuItem;
        }

        // PUT: api/MenuItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MenuItemUpdateDto dto)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            // Actualizar el título si viene
            if (!string.IsNullOrEmpty(dto.Title))
                menuItem.Title = dto.Title;

            // Si viene nueva imagen, reemplazarla
            if (dto.Icon != null && dto.Icon.Length > 0)
            {
                // ----- ELIMINAR LA ANTERIOR -----
                if (!string.IsNullOrEmpty(menuItem.Icon))
                {
                    // Sacar el nombre del archivo desde la URL guardada en BD
                    string oldFileName = Path.GetFileName(menuItem.Icon);

                    // Ruta física del viejo archivo
                    string oldFilePath = Path.Combine(_env.WebRootPath, "icons", oldFileName);

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                // ----- GUARDAR LA NUEVA -----
                string iconsFolder = Path.Combine(_env.WebRootPath, "icons");
                if (!Directory.Exists(iconsFolder))
                    Directory.CreateDirectory(iconsFolder);

                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Icon.FileName);
                string newFilePath = Path.Combine(iconsFolder, newFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await dto.Icon.CopyToAsync(stream);
                }

                // Construir la nueva URL pública
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                string newFileUrl = $"{baseUrl}/icons/{newFileName}";

                menuItem.Icon = newFileUrl;
            }

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(menuItem);
        }

        // POST: api/MenuItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MenuItemCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar archivo
            if (dto.Icon == null || dto.Icon.Length == 0)
                return BadRequest("Debe enviar una imagen");

            // Asegurar carpeta /wwwroot/icons
            string iconsFolder = Path.Combine(_env.WebRootPath, "icons");
            if (!Directory.Exists(iconsFolder))
                Directory.CreateDirectory(iconsFolder);

            // Generar nombre único
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Icon.FileName);

            // Ruta física donde se guardará
            string filePath = Path.Combine(iconsFolder, fileName);

            // Guardar archivo físicamente
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Icon.CopyToAsync(stream);
            }

            // Construir URL pública para guardar en BD
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string fileUrl = $"{baseUrl}/icons/{fileName}";

            // Crear entidad
            var menuItem = new MenuItem
            {
                Title = dto.Title,
                Icon = fileUrl  // AQUÍ se guarda la URL absoluta
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            // Retornar el resultado creado
            return CreatedAtAction("GetMenuItem", new { id = menuItem.Id }, menuItem);
        }

        // DELETE: api/MenuItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            // ===== BORRAR IMAGEN =====
            if (!string.IsNullOrEmpty(menuItem.Icon))
            {
                // menuItem.Icon = "https:dominio../icons/imagen.png"
                string fileName = Path.GetFileName(menuItem.Icon); // imagen.png

                string filePath = Path.Combine(_env.WebRootPath, "icons", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // ===== BORRAR REGISTRO =====
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
