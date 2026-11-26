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
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;

namespace WsElecciones.Persistence.Repository
{
    public class SelectRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor): Repository<AppSelect>(context),ISelectItemRepository 
    {
        private const string Sp_GetCatalogo = "up_get_eleccion_proceso_v01";

        public async Task<IReadOnlyCollection<SelectItemView>> GetByIdAsync(int Tipo,int Id, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateInt("@Tipo", Tipo),
                SqlParameterFactory.CreateInt("@Id", Id),
            };

            var result = await spExecutor.ExecuteReaderAsync(
                Sp_GetCatalogo,
                reader => new SelectItemView(
                    ConvertDbHelper.ToInt32(reader["Id"]),
                    ConvertDbHelper.ToString(reader["Valor"])
                ),
                parameters,
                cancellationToken);

            return result;


            //var mappers = new List<Func<IDataRecord, object>>
            //{
            //    reader => new SelectItemView(
            //        ConvertDbHelper.ToInt32(reader["Clave"]),
            //        ConvertDbHelper.ToString(reader["Valor"])
            //    ),

            //    //reader => new SelectItemView(
            //    //    ConvertDbHelper.ToInt32(reader["Clave"]),
            //    //    ConvertDbHelper.ToString(reader["Valor"])
            //    //)
            //};

            //var results = await spExecutor.ExecuteMultipleReaderAsync(
            //    Sp_Select,
            //    mappers,
            //    parameters,
            //    cancellationToken
            //);
            //    var Elecciones = results[0].Cast<SelectItemView>().ToList().AsReadOnly();
            //    //var Procesos   = results[1].Cast<SelectItemView>().ToList().AsReadOnly();

            //return new SelectItemView(Elecciones);
        }
    }
}
