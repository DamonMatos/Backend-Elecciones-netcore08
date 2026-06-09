using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WsElecciones.Domain.Interface
{
    public interface IFileStorageService
    {
        void SaveAsync(string module, string foldername, string fileName, IFormFile fileStream, CancellationToken cancellationToken = default);
        //void SaveAsync(IFormFile fileStream, string foldername, string fileName, CancellationToken cancellationToken = default);
        void CreateCarpeta(string foldername, string idEleccion, string idProceso);
        void DeleteCarpeta(string foldername, string idEleccion, string idProceso);

    }
}
