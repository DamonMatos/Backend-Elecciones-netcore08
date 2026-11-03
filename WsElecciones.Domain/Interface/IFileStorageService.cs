using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WsElecciones.Domain.Interface
{
    public interface IFileStorageService
    {
        Task SaveAsync(string module, string? foldername, string fileName, IFormFile fileStream, CancellationToken cancellationToken = default);
        Task CreateCarpeta(string foldername, string idEleccion, string idProceso);
        Task DeleteCarpeta(string foldername, string idEleccion, string idProceso);

    }
}
