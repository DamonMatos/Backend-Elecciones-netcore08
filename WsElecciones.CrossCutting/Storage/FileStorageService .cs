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
        private readonly IOptions<FileStorageConfig> _storageOptions;
        private readonly ILogger<FileStorageService> _logger;
        public FileStorageService(IOptions<FileStorageConfig> storageOptions, ILogger<FileStorageService> logger)
        {
            _logger = logger;
            _storageOptions = storageOptions;  
        }

        public async Task CreateCarpeta(string foldername,string idEleccion,string idProceso) 
        {
            var storage = _storageOptions.Value.Modules[foldername];
            var _sharedPath = storage.SharedPath;
            String Ruta = String.Format("{0}//{1}//{2}", _sharedPath.ToString(),idEleccion,idProceso);
            if(!Directory.Exists(Ruta)) 
            {
                Directory.CreateDirectory(Ruta);
            }        
        }

        public async Task DeleteCarpeta(string foldername, string idEleccion, string idProceso)
        {
            var storage = _storageOptions.Value.Modules[foldername];
            var _sharedPath = storage.SharedPath;
            String Ruta = String.Format("{0}//{1}//{2}", _sharedPath.ToString(), idEleccion, idProceso);
            if (Directory.Exists(Ruta))
            {
                Directory.Delete(Ruta,true);
            }
        }

        public async Task SaveAsync(string module, string foldername, string fileName, IFormFile fileStream, CancellationToken cancellationToken = default)
        {
            var storage = _storageOptions.Value.Modules[module];
            var _sharedPath = storage.SharedPath;

            var folderPath = Path.Combine(_sharedPath, foldername);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, fileName);

            await using var fileOutput = new FileStream(
                fullPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true);

            await fileStream.CopyToAsync(fileOutput, cancellationToken);
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
