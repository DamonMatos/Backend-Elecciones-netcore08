using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WsElecciones.Domain.Interface
{
    public interface IFileStorageService
    {
        void SaveAsync(IFormFile fileStream, string foldername, string fileName, CancellationToken cancellationToken = default);
       // Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
    }
}
