using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Auth;
using WsElecciones.Domain.Views.CuentaCorrienteCuotas;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Persistence.Repository
{
    public class AuthRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor): Repository<AppUser>(context), IAuthRepository
    {
        private const string SpLogin = "up_Iniciar_Sesion_v01";
        private const string SpRegistrar = "up_Add_Users_v01";
        private const string SqUpdate = "up_Add_Users_Personal_v01";



        public async Task<GetAuthView> GetByUsernameAsync(string correo, CancellationToken cancellationToken= default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateVarchar("@Correo",50, correo),
            };

            var mappers = new List<Func<IDataRecord, object>>
            {
                reader => new UserDto(
                    ConvertDbHelper.ToInt32(reader["IdUsuario"]),
                    ConvertDbHelper.ToInt32(reader["IdPersonal"]),
                    ConvertDbHelper.ToString(reader["ApePatPer"]),
                    ConvertDbHelper.ToString(reader["ApeMatPer"]),
                    ConvertDbHelper.ToString(reader["NomPer"]),
                    ConvertDbHelper.ToString(reader["TipDocPer"]),
                    ConvertDbHelper.ToString(reader["NumDocPer"]),
                    ConvertDbHelper.ToString(reader["FehNacPer"]),
                    ConvertDbHelper.ToString(reader["FotPer"]),
                    ConvertDbHelper.ToInt32(reader["IdPerfil"]),
                    ConvertDbHelper.ToString(reader["Perfil"]),
                    ConvertDbHelper.ToString(reader["Correo"]),
                    ConvertDbHelper.ToString(reader["ClaveHash"])
                ),

                reader => new MenuDto(
                    ConvertDbHelper.ToInt32(reader["IdPerfil"]),
                    ConvertDbHelper.ToString(reader["Perfil"]),
                    ConvertDbHelper.ToInt32(reader["IdMenu"]),
                    ConvertDbHelper.ToString(reader["Menu"]),
                    ConvertDbHelper.ToInt32(reader["IdSubMenu"]),
                    ConvertDbHelper.ToString(reader["NomVista"]),
                    ConvertDbHelper.ToString(reader["NomUrl"]),
                    ConvertDbHelper.ToString(reader["Icono"]),
                    ConvertDbHelper.ToString(reader["Tipo"]),
                    ConvertDbHelper.ToInt32(reader["Cantidad"])
                )
            };

            var results = await spExecutor.ExecuteMultipleReaderAsync(
                SpLogin,
                mappers,
                parameters,
                cancellationToken
            );

            var users = results[0].Cast<UserDto>().ToList().AsReadOnly();
            var menu = results[1].Cast<MenuDto>().ToList().AsReadOnly();

            return new GetAuthView(
                Token: null,
                Expiry: null,
                User: users,
                Menu: menu
            );

        }

        public async Task<IReadOnlyCollection<ResponseView>> RegistrarUserAsysc(string correo, string claveHash, string perfil, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateVarchar("@Correo",50, correo),
                SqlParameterFactory.CreateVarchar("@ClaveHash",60, claveHash),
                SqlParameterFactory.CreateVarchar("@Perfil",50, perfil),
            };

            var result = await spExecutor.ExecuteReaderAsync(
                SpRegistrar,
                reader => new ResponseView(
                    ConvertDbHelper.ToInt32(reader["Estado"]),
                    ConvertDbHelper.ToString(reader["Mensaje"])
                ),
                parameters,
                cancellationToken);

            return result;
        }

        public async Task<IReadOnlyCollection<ResponseView>> UpdateUserAsysc(UpdateUserView request, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdUsuario", request.IdUsuario),
                SqlParameterFactory.CreateInt("@IdPersonal", request.IdPersonal),
                SqlParameterFactory.CreateVarchar("@Nombre", 100, request.Nombre),
                SqlParameterFactory.CreateVarchar("@ApellidoPaterno", 100, request.ApellidoPaterno),
                SqlParameterFactory.CreateVarchar("@ApellidoMaterno", 100, request.ApellidoMaterno),
                SqlParameterFactory.CreateDateTime("@FechaNacimiento", request.FechaNacimiento),
                SqlParameterFactory.CreateVarchar("@TipoDocumento", 100, request.TipoDocumento),
                SqlParameterFactory.CreateVarchar("@NumeroDocumento", 20, request.NumeroDocumento),
                SqlParameterFactory.CreateVarchar("@Foto", 100, request.NombreFoto),
                SqlParameterFactory.CreateVarchar("@NombreCliente", 100, request.NombreCliente),
                SqlParameterFactory.CreateVarchar("@RazonSocial", 100, request.RazonSocial),
                SqlParameterFactory.CreateVarchar("@Ruc", 30, request.Ruc)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                SqUpdate,
                reader => new ResponseView(
                    ConvertDbHelper.ToInt32(reader["Estado"]),
                    ConvertDbHelper.ToString(reader["Mensaje"])
                ),
                parameters,
                cancellationToken);

            return result;
        }
    }
}
