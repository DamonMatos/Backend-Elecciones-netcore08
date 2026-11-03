using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public sealed record SelectItemView(
        int Id,
        string Valor
    );

}
