using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Candidatos;

public sealed record CreateCandidatoDTO
{
    public int Accion {  get; set; }
    public int IdEleccion { get; set; }
    public int IdProceso { get; set; }
    public int IdCandidato { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Area { get; set; }
    public string? Localidad { get; set; }
    public string? UrlFile { get; set; }
    public IFormFile? Foto { get; set; }
    public int Estado { get; set; }
    public string? Descripcion { get; set; }
}
