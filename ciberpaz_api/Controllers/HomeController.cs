using ciberpaz_api.Context;
using ciberpaz_api.DTOs;
using ciberpaz_api.Models;
using ciberpaz_api.Services;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class HomeController(AppDbContext context, ImageService imageService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        // GET: api/Home
        [HttpGet]
        public async Task<ActionResult<HomeDto>> GetHome()
        {
            var home = await _context.Homes
                .Include(h => h.ViewItems)
                .FirstOrDefaultAsync();

            if (home == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            string? ToAbsolute(string? path)
                => string.IsNullOrWhiteSpace(path) ? null : $"{baseUrl}/{path}";

            var dto = new HomeDto
            {
                Id = home.Id,
                Title = home.Title,
                GovIcon = ToAbsolute(home.GovIcon),
                AppIcon = ToAbsolute(home.AppIcon),
                ViewItems = [.. home.ViewItems
                    .OrderBy(v => v.Order)
                    .Select(v => new ViewItemDto
                    {
                        Id = v.Id,
                        Title = v.Title,
                        Icon = ToAbsolute(v.Icon),
                        Route = v.Route,
                        WebUrl = v.WebUrl,
                        Embed = v.Embed,
                        Order = v.Order
                    })]
            };

            return Ok(dto);
        }

        // POST: api/Home
        [ApiKey]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<HomeDto>> CreateHome([FromForm] HomeCreateDto homeDto)
        {
            // Singleton: solo un Home permitido
            if (await _context.Homes.AnyAsync())
                return BadRequest("Home ya existe");

            var govIcon = await _imageService.SaveImageAsync(homeDto.GovIcon, "images");
            var appIcon = await _imageService.SaveImageAsync(homeDto.AppIcon, "images");

            var home = new Home
            {
                Title = homeDto.Title,
                GovIcon = govIcon,
                AppIcon = appIcon,
            };

            _context.Homes.Add(home);
            await _context.SaveChangesAsync();

            return Ok(home);
        }

        // PUT: api/Home
        [ApiKey]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<HomeDto>> UpdateHome([FromForm] HomePatchDto dto)
        {
            var home = await _context.Homes.FirstOrDefaultAsync();
            if (home == null)
                return NotFound();

            if (dto.Title is not null)
                home.Title = dto.Title;

            home.GovIcon = await _imageService.UpdateImageAsync(dto.GovIcon, home.GovIcon, "images");
            home.AppIcon = await _imageService.UpdateImageAsync(dto.AppIcon, home.AppIcon, "images");

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(home);
        }

    }
}
