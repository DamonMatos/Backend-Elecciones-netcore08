using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public sealed record ActualizacionMasivoView
    (
       int CodigoEjecucion,
       string Estado,
       string Resultado
    );

    public sealed record ActualizacionMasivoItem
        (
            string codAlumno,
            string nombreAlumno,
            string codCampus,
            string nombreCampus,
            string codFacultad,
            string codCarrera,
            string nombreCarrera,
            string codDepartamento,
            string nombreDepartamento,
            string estado,
            string nombreIncidente
        );

    public sealed record ActualizacionMasivoViewPagedResult(
        IReadOnlyCollection<ActualizacionMasivoItem> Items,
        int TotalRegistros,
        int Page,
        int Limit);
}
