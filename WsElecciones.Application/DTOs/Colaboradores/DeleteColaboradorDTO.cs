using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Colaboradores
{
    public sealed record class DeleteColaboradorDTO(
        int idEleccion, 
        int idProceso, 
        string tipoDocumento, 
        string numeroDocumento
    );
}
