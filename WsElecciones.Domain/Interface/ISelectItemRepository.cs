using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Elecciones;

namespace WsElecciones.Domain.Interface
{
    public interface ISelectItemRepository
    {
        Task<IReadOnlyCollection<SelectItemView>> GetByIdAsync(int Tipo,int IdCliente, CancellationToken cancellationToken = default);
    }
}
