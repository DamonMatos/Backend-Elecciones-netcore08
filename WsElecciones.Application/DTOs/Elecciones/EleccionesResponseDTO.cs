using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente.GetProgramacionCuentaCorrienteAlumnosDTO;

namespace WsElecciones.Application.DTOs.Elecciones;

public sealed record EleccionesResponseDTO(
    int IdEleccion,
    int IdCliente,
    string RazonSocial,
    string RUC,
    string Nombre,
    string ColorBase,
    string UrlLogo,
    string NumDocPer,
    string FechaDifusion,
    string FechaInicio,
    string FechaFin,
    string FechaRegistro,
    bool PlanillaConfirmada,
    bool DifusionEnviada,
    int Estado
    );

public sealed record EleccionesPagedResponseDTO(
    IReadOnlyCollection<EleccionesResponseDTO> Items,
    int TotalRegistros,
    int Page,
    int Limit
    );
