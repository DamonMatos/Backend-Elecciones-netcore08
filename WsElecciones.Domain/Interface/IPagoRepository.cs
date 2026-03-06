using WsElecciones.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Interface
{
    public interface IPagoRepository
    {
        Task<List<PagoView.PagoItem>> GetPagoAsync(
        int codDocumento,
        CancellationToken cancellationToken = default);


    }
}
