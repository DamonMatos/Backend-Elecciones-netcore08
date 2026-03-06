using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Auth;
    public sealed record  LoginRequestDTO(
        string Correo,
        string Clave
    );
    

