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
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WsElecciones.Domain.Views.Candidatos.GetCandidatosView;
using static WsElecciones.Domain.Views.Colaboradores.GetColaboradoresView;

namespace WsElecciones.Persistence.Repository
{
    public class ColaboradorRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppColaborador>(context), IColaboradorRepository
    {
        public const string Sp_GetByIdEleccion = "USP_ListarColaboradorSSMA";
        public async Task<GetColaboradoresView.ColaboradoresPagedResult> GetColaboradoresAsync(int page, int limit, int idEleccion, int idProceso, CancellationToken cancellationToken = default)
        {
           var parameters = new[]
           {
                SqlParameterFactory.CreateInt("@NumeroPagina", page),
                SqlParameterFactory.CreateInt("@CantidadRegistros", limit),
                SqlParameterFactory.CreateInt("@IdEleccion", idEleccion),
                SqlParameterFactory.CreateInt("@IdProceso", idProceso),
           };

            var mappers = new List<Func<IDataRecord, object>>
            {
                reader => new ColadoradorItem(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdProceso"]),
                    ConvertDbHelper.ToString(reader["TipoDocumento"]),
                    ConvertDbHelper.ToString(reader["NumeroDocumento"]),
                    ConvertDbHelper.ToString(reader["Cargo"]),
                    ConvertDbHelper.ToString(reader["Sede"]),
                    ConvertDbHelper.ToString(reader["Nombre"]),
                    ConvertDbHelper.ToString(reader["Apellidopaterno"]),
                    ConvertDbHelper.ToString(reader["Apellidomaterno"]),
                    ConvertDbHelper.ToDateTime(reader["Fecharegistro"]),
                    ConvertDbHelper.ToString(reader["Emaildifusion"]),
                    ConvertDbHelper.ToInt32(reader["IdUsuario"]),
                    ConvertDbHelper.ToInt32(reader["Estado"])
                    ),

                reader => ConvertDbHelper.ToInt32(reader["TotalRegistros"]),

            };
            var result = await spExecutor.ExecuteMultipleReaderAsync(
                Sp_GetByIdEleccion,
                mappers,
                parameters,
                cancellationToken
                );
            
            var items = result[0].Cast<ColadoradorItem>().ToList().AsReadOnly();
            var total = result[1].Cast<int>().FirstOrDefault();

            return new ColaboradoresPagedResult(
                Items: items,
                TotalRegistros: total,
                Page: page,
                Limit: limit
            );
        }
    }
}
