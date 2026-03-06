using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Auth;

public sealed record UserDto(
    int IdUsuario,
    int IdPersonal,
    string ApePatPer,
    string ApeMatPer,
    string NomPer,
    string TipDocPer,
    string NumDocPer,
    string FehNacPer,
    string FotPer,
    int IdPerfil,
    string Perfil,
    string Correo,
    [property: JsonIgnore]
    string ClaveHash
);
