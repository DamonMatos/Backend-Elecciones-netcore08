using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Interface;

namespace WsElecciones.CrossCutting.Storage
{
    public class FileStorageService : IFileStorageService
    {
        //private readonly string _sharedPath;
        private readonly IOptions<FileStorageConfig> _strageOptions;
        //private readonly string _baseUrl;
        private readonly ILogger<FileStorageService> _logger;
       // private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase) { ".png",".jpg" };
       // private const long MaxFileSizeBytes = 2 * 1024 * 1024;

        public FileStorageService(IOptions<FileStorageConfig> storageOptions, ILogger<FileStorageService> logger)
        {
            _logger = logger;
            _strageOptions = storageOptions;  
        }

        public void CreateCarpeta(string foldername,string idEleccion,string idProceso) 
        {
            var storage = _strageOptions.Value.Modules[foldername];
            var _sharedPath = storage.SharedPath;
            String Ruta = String.Format("{0}//{1}//{2}", _sharedPath.ToString(),idEleccion,idProceso);
            if(!Directory.Exists(Ruta)) 
            {
                Directory.CreateDirectory(Ruta);
            }        
        }

        public void DeleteCarpeta(string foldername, string idEleccion, string idProceso)
        {
            var storage = _strageOptions.Value.Modules[foldername];
            var _sharedPath = storage.SharedPath;
            String Ruta = String.Format("{0}//{1}//{2}", _sharedPath.ToString(), idEleccion, idProceso);
            if (Directory.Exists(Ruta))
            {
                Directory.Delete(Ruta,true);
            }
        }

        public async void SaveAsync(string module, string foldername, string fileName, IFormFile fileStream, CancellationToken cancellationToken = default)
        {
            var storage = _strageOptions.Value.Modules[module];
            var _sharedPath = storage.SharedPath;

            //var subFolder = Path.GetFileNameWithoutExtension(fileName); 
            //var folderPath = Path.Combine(_sharedPath, subFolder);
            var folderPath = Path.Combine(_sharedPath, foldername);

            //Crear la carpeta
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //Crear el Archivo File 
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
