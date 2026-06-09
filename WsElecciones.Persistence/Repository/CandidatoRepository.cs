using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Algorithm;
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
using WsElecciones.Domain.Views.Candidatos;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static WsElecciones.Domain.Views.Candidatos.GetCandidatosView;

namespace WsElecciones.Persistence.Repository
{
    public class CandidatoRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppCandidato>(context), ICandidatoRepository
    {
        public const string Sp_GetByIdEleccion = "USP_ListarCandidatoSSMA";
        public const string Sp_MantenEleccion  = "up_mantenimiento_CandidatoSSMA";

        public async Task<ResponseView> CreateCandidatosAsync(int Accion, CandidatoItem entidad, CancellationToken cancellationToken = default)
        {
            var parametros = new[]
            {
                SqlParameterFactory.CreateInt("@Accion", Accion),
                SqlParameterFactory.CreateInt("@IdEleccion", entidad.IdEleccion),
                SqlParameterFactory.CreateInt("@IdProceso", entidad.IdProceso),
                SqlParameterFactory.CreateOutput("@IdCandidato", SqlDbType.Int),
                SqlParameterFactory.CreateVarchar("@TipoDocumento",50, entidad.TipoDocumento),
                SqlParameterFactory.CreateVarchar("@NumeroDocumento",20, entidad.NumeroDocumento),
                SqlParameterFactory.CreateVarchar("@NombreCompleto", 150, entidad.NombreCompleto),
                SqlParameterFactory.CreateVarchar("@Area", 100, entidad.Area),
                SqlParameterFactory.CreateVarchar("@Localidad",100, entidad.Localidad),
                SqlParameterFactory.CreateVarchar("@UrlFile",255, entidad.UrlFile),
                SqlParameterFactory.CreateInt("@Estado", entidad.Estado),
                SqlParameterFactory.CreateVarchar("@Descripcion",int.MaxValue, entidad.Descripcion),
                SqlParameterFactory.CreateOutput("@EstadoRetorno", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@MensajeRetorno",SqlDbType.VarChar,250)
            };

            await spExecutor.ExecuteNonQueryAsync(
                Sp_MantenEleccion,
                parametros,
                cancellationToken);

            return new ResponseView(
                Id     : SqlParameterFactory.GetOutputValue<int>(parametros, "@IdCandidato", 0),
                Estado : SqlParameterFactory.GetOutputValue<int>(parametros, "@EstadoRetorno", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parametros, "@MensajeRetorno", string.Empty)
            );
        }

        public async Task<GetCandidatosView.CandidatoPagedResult> GetCandidatosAsync(int page, int limit, int idEleccion, int idProceso, CancellationToken cancellationToken = default)
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
                reader => new CandidatoItem(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdProceso"]),
                    ConvertDbHelper.ToInt32(reader["IdCandidato"]),
                    ConvertDbHelper.ToString(reader["TipoDocumento"]),
                    ConvertDbHelper.ToString(reader["NumeroDocumento"]),
                    ConvertDbHelper.ToString(reader["NombreCompleto"]),
                    ConvertDbHelper.ToString(reader["Area"]),
                    ConvertDbHelper.ToString(reader["Localidad"]),
                    ConvertDbHelper.ToString(reader["UrlFile"]),
                    ConvertDbHelper.ToInt32(reader["Estado"]),
                    ConvertDbHelper.ToString(reader["Descripcion"])
                    ),
                reader => ConvertDbHelper.ToInt32(reader["TotalRegistros"])
            };

            var result = await spExecutor.ExecuteMultipleReaderAsync(
                Sp_GetByIdEleccion, 
                mappers, 
                parameters, 
                cancellationToken
                );

            var items = result[0].Cast<CandidatoItem>().ToList().AsReadOnly();
            var total = result[1].Cast<int>().FirstOrDefault();

            return new CandidatoPagedResult(
                Items: items,
                TotalRegistros: total,
                Page:page,
                Limit:limit
            );
        }
    }
}
