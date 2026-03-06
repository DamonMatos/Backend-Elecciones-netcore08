using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Elecciones;

public sealed record EleccionesRequestDTO(
    int IdPersonal,
    int page = 1,
    int limit = 10
    );

