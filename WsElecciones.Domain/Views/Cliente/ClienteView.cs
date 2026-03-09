using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Cliente;

    public sealed record class ClienteView(
        int IdCliente,
        string NombreCliente,
        int EstadoCliente,
        string RazonSocial,
        string Ruc,
        int IdPersonal
    );
