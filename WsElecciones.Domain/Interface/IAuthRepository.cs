using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Auth;

namespace WsElecciones.Domain.Interface
{
    public interface IAuthRepository
    {
        Task<GetAuthView> GetByUsernameAsync(string correo, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ResponseView>> RegistrarUserAsysc(string username, string claveHash, string perfil,CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ResponseView>> UpdateUserAsysc(UpdateUserView request, CancellationToken cancellationToken = default);
    }
}
