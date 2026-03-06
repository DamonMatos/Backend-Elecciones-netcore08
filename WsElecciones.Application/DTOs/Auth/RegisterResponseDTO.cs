using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Auth;

public sealed record RegisterResponseDTO(
    bool Success,
    string? Message,
    string? UserId,
    string? Username,
    string? Role
    );
