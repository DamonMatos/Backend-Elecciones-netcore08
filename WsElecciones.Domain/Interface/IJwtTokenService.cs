using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views.Auth;

namespace WsElecciones.Domain.Interface
{
    public interface IJwtTokenService
    {
        (string Token, DateTime Expiry) GenerateToken(UserDto user);
    }
}
