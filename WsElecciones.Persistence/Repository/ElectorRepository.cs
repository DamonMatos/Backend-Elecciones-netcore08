using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Elector;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;

namespace WsElecciones.Persistence.Repository
{
    public class ElectorRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : IElectorRepository
    {
        public const string Sp_AddElector = "up_Add_Elector_v01";
        public async Task<ResponseView> CreateElectorAsync(CreateElectorView request, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdUsuario", request.IdUsuario),
                SqlParameterFactory.CreateInt("@IdEleccion", request.IdEleccion),
                SqlParameterFactory.CreateInt("@IdProceso", request.IdProceso),
                SqlParameterFactory.CreateNullableInt("@IdCandidato1", request.IdCandidato1),
                SqlParameterFactory.CreateNullableInt("@IdCandidato2", request.IdCandidato2),
                SqlParameterFactory.CreateNullableInt("@IdCandidato3", request.IdCandidato3),
                SqlParameterFactory.CreateNullableInt("@IdCandidato4", request.IdCandidato4),
                SqlParameterFactory.CreateNullableInt("@IdCandidato5", request.IdCandidato5),
                SqlParameterFactory.CreateNullableInt("@IdCandidato6", request.IdCandidato6),
                SqlParameterFactory.CreateOutput("@IdElectorOut", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Resultado", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Mensaje", SqlDbType.VarChar,250)
            };

            await spExecutor.ExecuteNonQueryAsync(
                Sp_AddElector,
                parameters,
                cancellationToken);

            return new ResponseView(
                Id: SqlParameterFactory.GetOutputValue<int>(parameters, "@IdElectorOut", 0),
                Estado: SqlParameterFactory.GetOutputValue<int>(parameters, "@Resultado", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parameters, "@Mensaje", string.Empty)
                );
        }
    }
}
