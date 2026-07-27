using Azure.Core;
using Microsoft.Data.SqlClient;
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
using WsElecciones.Domain.Views.Cliente;
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Domain.Views.Elecciones;
using WsElecciones.Domain.Views.Usuario;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WsElecciones.Domain.Views.Candidatos.GetCandidatosView;
using static WsElecciones.Domain.Views.Colaboradores.GetColaboradoresView;

namespace WsElecciones.Persistence.Repository
{
    public class ColaboradorRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppColaborador>(context), IColaboradorRepository
    {
        public const string Sp_GetByIdEleccion   = "USP_ListarColaboradorSSMA";
        public const string Sp_CreateColaborador = "Usp_CreateColaboradoresSSMA_Optimized";
        public const string Sp_DeleteColaborador = "Usp_EliminarColaboradorSSMA";
        public const string Sp_UpdateUsuario     = "Usp_UpdateUsuarioSSMA_Optimized";
        public const string Sp_GetColaboradoresMasivo = "Usp_getColaborador_Correo";

        public async Task<ResponseView> CrearColaboradoresAsync(CrearListaColaborador request, CancellationToken cancellationToken = default)
        {
            var parametros = BuildParametrosColaboradores(request).ToArray();
            await spExecutor.ExecuteNonQueryAsync(
                Sp_CreateColaborador,
                parametros,
                cancellationToken);

            return new ResponseView(
                Id: 0,
                Estado: SqlParameterFactory.GetOutputValue<int>(parametros, "@PESTADO", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parametros, "@PRESULTADO", string.Empty)
                );

        }

        public async Task<ResponseView> DeleteColaboradorAsync(DeleteColaboradorView request, CancellationToken cancellationToken = default)
        {
           var parameters = new[]
           {
                SqlParameterFactory.CreateInt("@IdEleccion", request.idEleccion),
                SqlParameterFactory.CreateInt("@IdProceso", request.idProceso),
                SqlParameterFactory.CreateVarchar("@TipoDocumento",255, request.tipoDocumento),
                SqlParameterFactory.CreateVarchar("@NumeroDocumento",15, request.numeroDocumento),
           };

            await spExecutor.ExecuteNonQueryAsync(
                    Sp_DeleteColaborador,
                    parameters,
                    cancellationToken);

            return new ResponseView(
                Id: 0,
                Estado: SqlParameterFactory.GetOutputValue<int>(parameters, "@Estado", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parameters, "@Mensaje", string.Empty)
            );

        }

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

        public async Task<IReadOnlyCollection<ColaboradoresEnvioMasivoCorreo>> GetColaboradoresEnvioMasivoAsync(int idEleccion, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@IdEleccion", idEleccion)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                Sp_GetColaboradoresMasivo,
                reader => new ColaboradoresEnvioMasivoCorreo(
                    ConvertDbHelper.ToString(reader["Correo"]),
                    ConvertDbHelper.ToString(reader["Clave"]),
                    ConvertDbHelper.ToString(reader["NombrePerfil"])
                ),
                parameters,
                cancellationToken);

            return result;

        }

        public async Task<ResponseView> UpdateUsuarioAsync(UpdateUsuarioView.UpdateUsuariosListRequest usuarios, CancellationToken cancellationToken = default)
        {
            var parametros = BuildParametrosUsuarios(usuarios).ToArray();
            await spExecutor.ExecuteNonQueryAsync(
                Sp_UpdateUsuario,
                parametros,
                cancellationToken);

            return new ResponseView(
                Id: 0,
                Estado: SqlParameterFactory.GetOutputValue<int>(parametros, "@PESTADO", 0),
                Mensaje: SqlParameterFactory.GetOutputValue<string>(parametros, "@PRESULTADO", string.Empty)
            );

        }

        private IEnumerable<SqlParameter> BuildParametrosUsuarios(UpdateUsuarioView.UpdateUsuariosListRequest request)
        {
            var tablausuario = new DataTable();
            tablausuario.Columns.Add("Correo", typeof(string));
            tablausuario.Columns.Add("ClaveHash", typeof(string));
            if (request.Usuarios is not null)
            {
                foreach (var x in request.Usuarios)
                {
                    tablausuario.Rows.Add(x.Correo,
                                          x.ClaveHash
                                          );
                }
            }
            return [
                SqlParameterFactory.CreateStructured("@Usuarios", "dbo.TablaUsuarioType", tablausuario),
                SqlParameterFactory.CreateOutput("@PESTADO", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@PRESULTADO", SqlDbType.VarChar,255)
                ];
        }

        private IEnumerable<SqlParameter> BuildParametrosColaboradores(CrearListaColaborador entidad)
        {
            var tablacolaboradores = new DataTable();
            tablacolaboradores.Columns.Add("IdEleccion", typeof(int));
            tablacolaboradores.Columns.Add("IdProceso", typeof(int));
            tablacolaboradores.Columns.Add("Tipodocumento", typeof(string));
            tablacolaboradores.Columns.Add("NumeroDocumento", typeof(string));
            tablacolaboradores.Columns.Add("Cargo", typeof(string));
            tablacolaboradores.Columns.Add("Sede", typeof(string));
            tablacolaboradores.Columns.Add("Nombre", typeof(string));
            tablacolaboradores.Columns.Add("Apellidopaterno", typeof(string));
            tablacolaboradores.Columns.Add("Apellidomaterno", typeof(string));
            tablacolaboradores.Columns.Add("Emaildifusion", typeof(string));
            tablacolaboradores.Columns.Add("Estado", typeof(int));

            if (entidad is not null)
            {
                foreach (var x in entidad.Colaboradores)
                {
                    tablacolaboradores.Rows.Add(x.IdEleccion,
                                                x.IdProceso,
                                                x.Tipodocumento,
                                                x.NumeroDocumento,
                                                x.Cargo,
                                                x.Sede,
                                                x.Nombre,
                                                x.Apellidopaterno,
                                                x.Apellidomaterno,
                                                x.Emaildifusion,
                                                x.Estado
                                                );
                }
            }
            return [
                SqlParameterFactory.CreateStructured("@Colaboradores", "dbo.TablaColaboradorSSMAType", tablacolaboradores),
                SqlParameterFactory.CreateOutput("@PESTADO", SqlDbType.Int),
                SqlParameterFactory.CreateOutput("@PRESULTADO", SqlDbType.VarChar,255)
                ];
        }

    }
}
