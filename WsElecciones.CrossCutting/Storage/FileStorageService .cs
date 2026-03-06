using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Interface;
using Microsoft.AspNetCore.Http;

namespace WsElecciones.CrossCutting.Storage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _sharedPath;
        private readonly string _baseUrl;
        private readonly ILogger<FileStorageService> _logger;
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase) { ".png" };
       // private const long MaxFileSizeBytes = 2 * 1024 * 1024;

        public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
        {
            _logger = logger;
            _sharedPath = configuration["FileStorage:SharedPath"] ?? throw new InvalidOperationException("FileStorage:SharedPath no configurada.");
            _baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new InvalidOperationException("FileStorage:BaseUrl no configurada.");
        }

        public async void SaveAsync(IFormFile fileStream, string foldername, string fileName,CancellationToken cancellationToken = default)
        {
            //if (fileStream.Length > MaxFileSizeBytes)
            //    throw new InvalidOperationException("El archivo supera el tamaño máximo permitido de 2 MB.");

            var subFolder = Path.GetFileNameWithoutExtension(fileName); 
            var folderPath = Path.Combine(_sharedPath, foldername, subFolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, fileName);

            using var memoryStream = new MemoryStream();
            await using (var inputStream = fileStream.OpenReadStream())
            {
                if (inputStream.CanSeek)
                    inputStream.Seek(0, SeekOrigin.Begin);

                await inputStream.CopyToAsync(memoryStream, cancellationToken);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            await using var fileOutput = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true);

            await memoryStream.CopyToAsync(fileOutput, cancellationToken);
            await fileOutput.FlushAsync(cancellationToken);

            _logger.LogInformation("Foto guardada en ruta compartida: {Path}", fullPath);
        }

        //public Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(relativePath))
        //        return Task.CompletedTask;

        //    var fullPath = Path.Combine(_sharedPath, relativePath);

        //    if (File.Exists(fullPath))
        //    {
        //        File.Delete(fullPath);
        //        _logger.LogInformation("Foto eliminada de ruta compartida: {Path}", fullPath);
        //    }

        //    return Task.CompletedTask;
        //}
    }
}
