using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
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
using WsElecciones.Domain.Views.CuentasCorriente;
using WsElecciones.Domain.Views.Elecciones;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Persistence.Repository
{
    public class EleccionesRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppElecciones>(context), IEleccionesRepository
    {
        public const string Sp_GetElecciones   = "up_Get_Elecciones_v01";
        public const string Sp_AddElecciones   = "up_Add_Elecciones_v02";
        public const string Sp_GetByIdEleccion = "up_Get_EleccionesById_v01";
        public const string Sp_DeleteProceso = "up_delete_ProcesoById_v01";
        public const string Sp_GetProceso = "up_Get_Proceso";
        public const string Sp_GenerarDifusion = "Usp_GenerarDifusionSSMA";
        

        public async Task<ResponseView> CreateAsync(CreateEleccionesView request, CancellationToken cancellationToken = default)
        {
            var parametros = BuildParametrosElecciones(request).ToArray();

            await spExecutor.ExecuteNonQueryAsync(
                Sp_AddElecciones,
                parametros,
                cancellationToken);

            return new ResponseView(
                Id: SqlParameterFactory.GetOutputValue<int>(parametros, "@NuevoIdEleccion", 0),
                Estado: SqlParameterFactory.GetOutputValue<int>(parametros, "@PESTADO", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parametros, "@PRESULTADO", string.Empty)
                );
        }

        public async Task<ResponseView> DeleteProceso(int IdEleccion, int IdProceso, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdEleccion", IdEleccion),
                SqlParameterFactory.CreateInt("@IdProceso", IdProceso),
                SqlParameterFactory.CreateOutput("@Id", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Estado", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Mensaje", SqlDbType.VarChar,250)

            };

            await spExecutor.ExecuteNonQueryAsync(
                Sp_DeleteProceso,
                parameters,
                cancellationToken);

            return new ResponseView(
                Id: SqlParameterFactory.GetOutputValue<int>(parameters, "@Id", 0),
                Estado: SqlParameterFactory.GetOutputValue<int>(parameters, "@Estado", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parameters, "@Mensaje", string.Empty)
            );
        }

        public async Task<ResponseView> GenerarDifusion(int IdEleccion, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdEleccion", IdEleccion),
                SqlParameterFactory.CreateOutput("@Id", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Estado", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@Mensaje", SqlDbType.VarChar,250),
            };

            await spExecutor.ExecuteNonQueryAsync(
                Sp_GenerarDifusion,
                parameters,
                cancellationToken);

            return new ResponseView(
                Id: SqlParameterFactory.GetOutputValue<int>(parameters, "@Id", 0),
                Estado: SqlParameterFactory.GetOutputValue<int>(parameters, "@Estado", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parameters, "@Mensaje", string.Empty)
            );
        }

        public async Task<EleccionesPagedResult> GetAllAsync(int IdPersonal, int page,int limit, CancellationToken cancellationToken = default)
        {          
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdCliente", IdPersonal),
                SqlParameterFactory.CreateInt("@PageNumber", page),
                SqlParameterFactory.CreateInt("@PageSize", limit)
            };

            var mappers = new List<Func<IDataRecord, object>>
            {
                reader => new EleccionesView(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdCliente"]),
                    ConvertDbHelper.ToString(reader["RazonSocial"]),
                    ConvertDbHelper.ToString(reader["RUC"]),
                    ConvertDbHelper.ToString(reader["Nombre"]),
                    ConvertDbHelper.ToString(reader["ColorBase"]),
                    ConvertDbHelper.ToString(reader["UrlLogo"]),
                    ConvertDbHelper.ToString(reader["NumDocPer"]),
                    ConvertDbHelper.ToString(reader["FechaDifusion"]),
                    ConvertDbHelper.ToString(reader["FechaInicio"]),
                    ConvertDbHelper.ToString(reader["FechaFin"]),
                    ConvertDbHelper.ToString(reader["FechaRegistro"]),
                    ConvertDbHelper.ToBoolean(reader["PlanillaConfirmada"]),
                    ConvertDbHelper.ToBoolean(reader["DifusionEnviada"]),
                    ConvertDbHelper.ToInt32(reader["Estado"])
                ),

                reader => ConvertDbHelper.ToInt32(reader["TotalRegistros"])
            };

            var results = await spExecutor.ExecuteMultipleReaderAsync(
                Sp_GetElecciones,
                mappers,
                parameters,
                cancellationToken
            );

            var items = results[0].Cast<EleccionesView>().ToList().AsReadOnly();

            var totalRegistros = results[1].Cast<int>().FirstOrDefault();

            return new EleccionesPagedResult(
                Items: items,
                TotalRegistros:totalRegistros,
                Page:page,
                Limit:limit
            );
        }

        public async Task<IReadOnlyCollection<int>> GetAllProcesoAsync(int IdEleccion, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdEleccion", IdEleccion)
            };

            var result = await spExecutor.ExecuteReaderAsync<int>(
                Sp_GetProceso,
                reader => ConvertDbHelper.ToInt32(reader["IdProceso"]),
                parameters,
                cancellationToken
            );

            return result.ToList().AsReadOnly();
        }

        public async Task<EleccionView> GetByIdAsync(int IdCliente, int IdEleccion, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdCliente", IdCliente),
                SqlParameterFactory.CreateInt("@IdEleccion", IdEleccion)
            };

            var mappers = new List<Func<IDataRecord, object>>
            {
                reader => new EleccionesView(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdCliente"]),
                    ConvertDbHelper.ToString(reader["RazonSocial"]),
                    ConvertDbHelper.ToString(reader["RUC"]),
                    ConvertDbHelper.ToString(reader["Nombre"]),
                    ConvertDbHelper.ToString(reader["ColorBase"]),
                    ConvertDbHelper.ToString(reader["UrlLogo"]),
                    ConvertDbHelper.ToString(reader["NumDocPer"]),
                    ConvertDbHelper.ToString(reader["FechaDifusion"]),
                    ConvertDbHelper.ToString(reader["FechaInicio"]),
                    ConvertDbHelper.ToString(reader["FechaFin"]),
                    ConvertDbHelper.ToString(reader["FechaRegistro"]),
                    ConvertDbHelper.ToBoolean(reader["PlanillaConfirmada"]),
                    ConvertDbHelper.ToBoolean(reader["DifusionEnviada"]),
                    ConvertDbHelper.ToInt32(reader["Estado"])
                ),

                reader => new ProcesoView(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdProceso"]),
                    ConvertDbHelper.ToString(reader["Nombre"]),
                    ConvertDbHelper.ToInt32(reader["NumeroCandidatos"]),
                    ConvertDbHelper.ToBoolean(reader["VotacionObligatoria"]),
                    ConvertDbHelper.ToInt32(reader["Estado"])
                ),
            };

            var results = await spExecutor.ExecuteMultipleReaderAsync(
                Sp_GetByIdEleccion,
                mappers,
                parameters,
                cancellationToken
            );

            var eleccion = results[0].Cast<EleccionesView>().FirstOrDefault();
            var proceso = results[1].Cast<ProcesoView>().ToList().AsReadOnly();

            return new EleccionView(
                Eleccion: eleccion,
                Procesos: proceso
            );
        }

        private IEnumerable<SqlParameter> BuildParametrosElecciones(CreateEleccionesView entidad)
        {
            var tablaProceso = new DataTable();
            tablaProceso.Columns.Add("IdProceso", typeof(int));
            tablaProceso.Columns.Add("Nombre", typeof(string));
            tablaProceso.Columns.Add("NumeroCandidatos", typeof(int));
            tablaProceso.Columns.Add("VotacionObligatoria", typeof(bool));
            tablaProceso.Columns.Add("Estado", typeof(int));

            if (entidad.Procesos is not null)
            {
                foreach (var x in entidad.Procesos)
                {
                    tablaProceso.Rows.Add(x.IdProceso,x.Nombre,x.NumeroCandidato,x.VotacionObligatoria,x.Estado);
                }
            }

            return
            [
            SqlParameterFactory.CreateOutput("@NuevoIdEleccion", SqlDbType.Int),
            SqlParameterFactory.CreateInt("@IdCliente", entidad.IdCliente),
            SqlParameterFactory.CreateInt("@IdEleccion", entidad.IdEleccion),
            SqlParameterFactory.CreateVarchar("@Nombre", 200, entidad.Nombre),
            SqlParameterFactory.CreateVarchar("@ColorBase", 255, entidad.ColorBase),
            SqlParameterFactory.CreateDateTime("@FechaDifusion", entidad.FechaDifusion),
            SqlParameterFactory.CreateDateTime("@FechaInicio", entidad.FechaInicio),
            SqlParameterFactory.CreateDateTime("@FechaFin", entidad.FechaFin),
            SqlParameterFactory.CreateNullableBit("@PlanillaConfirmada", entidad.PlanillaConfirmada),
            SqlParameterFactory.CreateNullableBit("@DifusionEnviada", entidad.DifusionEnviada),
            SqlParameterFactory.CreateInt("@Estado", entidad.Estado),
            SqlParameterFactory.CreateStructured("@Procesos", "dbo.ProcesoType", tablaProceso),
            SqlParameterFactory.CreateOutput("@PESTADO", SqlDbType.Int),
            SqlParameterFactory.CreateOutput("@PRESULTADO", SqlDbType.VarChar,250),
            ];
        }
    }
}
