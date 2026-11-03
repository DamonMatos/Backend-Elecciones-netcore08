using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting.Storage;

namespace WsElecciones.CrossCutting.Service
{
    public class UserPhotoService(IOptions<FileStorageConfig> storageOptions) : IUserPhotoService
    {
        public string GetPhotoUrl(string? fotPer, string? numDocPer)
        {
            var storage = storageOptions.Value.Modules["Personal"];
            var baseUrl = storage.BaseUrl;

            if (string.IsNullOrWhiteSpace(fotPer))
            {
                return $"{baseUrl}/user-default.png";
            }

            return $"{baseUrl}/{numDocPer?.Trim()}/{fotPer}";
        }
    }
}

