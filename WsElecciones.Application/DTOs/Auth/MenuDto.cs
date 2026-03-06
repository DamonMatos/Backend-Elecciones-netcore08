using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Auth;

public sealed record MenuDto(
    int IdPerfil,
    string Perfil,
    int IdMenu,
    string Menu,
    int IdSubMenu,
    string NomVista,
    string NomUrl,
    string Icono,
    string Tipo,
    int Cantidad
    );
