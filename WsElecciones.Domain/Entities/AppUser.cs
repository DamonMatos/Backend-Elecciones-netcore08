using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Entities;
    public class AppUser
    {
        public int IdUsuario { get;  set; }
        public int IdPersonal { get; set; }
        public string? ApePatPer { get; set; }
        public string? ApeMatPer { get;  set; }
        public string? NomPer { get; set; }
        public string? TipDocPer { get; set; }
        public string? NumDocPer { get; set; }
        public string? FehNacPer { get; set; }
        public string? FotPer { get; set; }
        public int IdPerfil { get; set; }
        public string? Perfil { get; set; }
        public string? Correo { get; set; }
        public string? ClaveHash { get; private set; }

    //public Guid Id { get; private set; } = Guid.NewGuid();
    //public string Username { get; private set; } = string.Empty;
    //public string Email { get; private set; } = string.Empty;
    //public string PasswordHash { get; private set; } = string.Empty;
    //public string Role { get; private set; } = UserRoles.User;
    //public bool IsActive { get; private set; } = true;
    //public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    //public DateTime? LastLogin { get; private set; }

    //private AppUser() { }

    //    public static AppUser Create(string username, string email, string passwordHash, string role) // string role = UserRoles.User)
    //      => new()
    //      {
    //          NomPer = username.Trim().ToLowerInvariant(),
    //          Correo = email.Trim().ToLowerInvariant(),
    //          ClaveHash = passwordHash,
    //          Perfil = role
    //      };

        //public void RegisterLogin() => LastLogin = DateTime.UtcNow;

        //public void Deactivate() => IsActive = false;

    }

    public static class UserRoles
    {
        public const string Admin = "Administrador" ;
        public const string Empresa = "Empresa";
       // public const string Correo = "Admin";
       // public const string ReadOnly = "ReadOnly";
    }
