using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Auth;

public sealed record LoginResponseDTO(
    string? Token,
    DateTime? Expiry,
    IReadOnlyCollection<UserDto> User,
    IReadOnlyCollection<MenuDto> Menu
    );

