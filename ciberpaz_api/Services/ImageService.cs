using Humanizer;
using System.IO;
using System;

namespace ciberpaz_api.Services
{
    public class ImageService(IWebHostEnvironment env)
    {
        private readonly IWebHostEnvironment _env = env;

        /* Save Image */
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

        /* Update Image */
        public async Task<string?> UpdateImageAsync(IFormFile? file, string? currentFile, string folder)
        {
            // Si viene nueva imagen, reemplazarla
            if (file == null || file.Length == 0) return currentFile;

            // ----- ELIMINAR LA ANTERIOR -----
            if (!string.IsNullOrEmpty(currentFile))
            {
                // Sacar el nombre del archivo desde la URL guardada en BD
                var oldPath = Path.Combine(_env.WebRootPath, currentFile.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
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

        /* Delete Image */
        public void DeleteImage(string? fileName)
        {
        // ===== BORRAR IMAGEN =====
            if (!string.IsNullOrEmpty(fileName))
            {
                

                string filePath = Path.Combine(_env.WebRootPath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        
    }
}
