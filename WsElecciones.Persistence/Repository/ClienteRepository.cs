using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Cliente;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WsElecciones.Persistence.Repository
{
    public class ClienteRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppCliente>(context), IClienteRepository
    {
        public const string Sp_GetCliente = "";
        public const string Sp_GetByIdCliente = "up_get_Users_Cliente_v01";
        public async Task<IReadOnlyCollection<ClienteView>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdPersonal", id)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                Sp_GetCliente,
                reader => new ClienteView(
                    ConvertDbHelper.ToInt32(reader["IdCliente"]),
                    ConvertDbHelper.ToString(reader["NombreCliente"]),
                    ConvertDbHelper.ToInt32(reader["EstadoCliente"]),
                    ConvertDbHelper.ToString(reader["RazonSocial"]),
                    ConvertDbHelper.ToString(reader["Ruc"]),
                    ConvertDbHelper.ToInt32(reader["IdPersonal"])
                ),
                parameters,
                cancellationToken);

            return result;
        }

        public async Task<ClienteView> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdPersonal", id)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                Sp_GetByIdCliente,
                reader => new ClienteView(
                    ConvertDbHelper.ToInt32(reader["IdCliente"]),
                    ConvertDbHelper.ToString(reader["NombreCliente"]),
                    ConvertDbHelper.ToInt32(reader["EstadoCliente"]),
                    ConvertDbHelper.ToString(reader["RazonSocial"]),
                    ConvertDbHelper.ToString(reader["Ruc"]),
                    ConvertDbHelper.ToInt32(reader["IdPersonal"])
                ),
                parameters,
                cancellationToken);

            return result.FirstOrDefault();

        }
    }
}
