using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Application.DTOs.Elecciones;

    public sealed record EleccionesRequestDTO(
        int IdPersonal,
        int Page = 1,
        int Limit = 10
        );

    public sealed record CreateEleccionesTemporalDTO
    {
        public int IdEleccion { get; init; }
        public int IdCliente { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public string ColorBase { get; init; } = string.Empty;
        public DateTime FechaInicio { get; init; }
        public DateTime FechaFin { get; init; }
        public DateTime? FechaDifusion { get; init; }
        public bool PlanillaConfirmada { get; init; }
        public bool DifusionEnviada { get; init; }
        public int Estado { get; init; }
        public IFormFile? Logo { get; init; }
        public string Procesos { get; init; } = "[]";
    }

    public sealed record CreateEleccionesDTO(
        int IdEleccion,
        int IdCliente,
        string Nombre,
        string ColorBase,
        DateTime FechaInicio,
        DateTime FechaFin,
        bool PlanillaConfirmada,
        bool DifusionEnviada,
        int Estado,
        IReadOnlyCollection<CreateProcesoDTO> Procesos,
        DateTime? FechaDifusion = null
        );

    public sealed record CreateProcesoDTO(
        int IdEleccion,
        int IdProceso,
        string Nombre,
        int NumeroCandidato,
        bool VotacionObligatoria, 
        int Estado
        );
