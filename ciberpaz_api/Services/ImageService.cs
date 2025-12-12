using Humanizer;
using System.IO;
using System;

namespace ciberpaz_api.Services
{
    public class ImageService(IWebHostEnvironment env)
    {
        private readonly IWebHostEnvironment _env = env;

        public async Task<string?> SaveImageAsync(IFormFile? file, string folder)
        {
            // Validar archivo
            if (file == null) return null;

            // Asegurar carpeta /wwwroot/icons
            string fileFolder = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);

            // Generar nombre único
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // Ruta física donde se guardará
            string path = Path.Combine(fileFolder, fileName);

            // Guardar archivo físicamente
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folder}/{fileName}";
        }

        public async Task<string?> UpdateImageAsync(IFormFile? file, string? currentFile, string folder)
        {
            // Si viene nueva imagen, reemplazarla
            if (file == null || file.Length == 0) return null;

            // ----- ELIMINAR LA ANTERIOR -----
            if (!string.IsNullOrEmpty(currentFile))
            {
                // Sacar el nombre del archivo desde la URL guardada en BD
                string oldFileName = Path.GetFileName(currentFile);

                // Ruta física del viejo archivo
                string oldFilePath = Path.Combine(_env.WebRootPath, folder, oldFileName);

                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }

            // ----- GUARDAR LA NUEVA -----
            string imgFolder = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(imgFolder))
                Directory.CreateDirectory(imgFolder);

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string newFilePath = Path.Combine(imgFolder, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folder}/{newFileName}";
        }

        public void DeleteImage(string? file, string folder)
        {
        // ===== BORRAR IMAGEN =====
            if (!string.IsNullOrEmpty(file))
            {
                // "https:dominio../icons/imagen.png"
                string fileName = Path.GetFileName(file); // imagen.png

                string filePath = Path.Combine(_env.WebRootPath, folder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        
    }
}
