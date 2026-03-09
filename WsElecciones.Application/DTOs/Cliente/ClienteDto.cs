using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Cliente;

public sealed record ClienteDto(
    int IdCliente,
    string NombreCliente,
    int EstadoCliente,
    string RazonSocial,
    string Ruc,
    int IdPersonal
    );
