using System;
using Microsoft.AspNetCore.Http;

namespace WsElecciones.Application.DTOs.Auth;
    public sealed record UpdateRequestDTO(
        int IdUsuario,
        int IdPersonal,
        string? Nombre,
        string? ApellidoPaterno,
        string? ApellidoMaterno,
        DateTime? FechaNacimiento,
        string? TipoDocumento,
        string? NumeroDocumento,
        string NombreFoto,
        string? NombreCliente,
        string RazonSocial,
        string Ruc,
        IFormFile? Foto
        );
