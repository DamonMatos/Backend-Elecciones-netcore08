using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Views;

namespace WsElecciones.Domain.Interface;

public interface IPagoAsbancRepository : IRepository<PagoAsbanc>
{
    Task<ConsultaDeudaCuentaCorrienteView.ConsultaDeudaCuentaCorrienteResult> GetConsultaDeudaAsync(string? codNivel = null,string? codAlumno = null, string? codProducto = null, CancellationToken cancellationToken = default);
    Task<int> RevertirPagoAsync(int codDocumento,string codUsuario);
}
