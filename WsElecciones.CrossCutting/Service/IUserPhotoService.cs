using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.CrossCutting.Service
{
    public interface IUserPhotoService
    {
        string GetPhotoUrl(string? fotPer, string? numDocPer);
    }
}
