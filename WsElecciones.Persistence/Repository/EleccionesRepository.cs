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
using WsElecciones.Domain.Views.Auth;
using WsElecciones.Domain.Views.Elecciones;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Persistence.Repository
{
    public class EleccionesRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : Repository<AppElecciones>(context), IEleccionesRepository
    {
        public const string Sp_GetElecciones = "up_Get_Elecciones_v01";
        public async Task<EleccionesPagedResult> GetEleccionesAsysc(int IdPersonal, int page,int limit, CancellationToken cancellationToken = default)
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

    }
}
