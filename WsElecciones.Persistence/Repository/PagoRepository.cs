using Azure;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Persistence.Repository;

    public class PagoRepository : IPagoRepository
    {
        private const string ListStoredProcedureName = "ctacte.Sp_Sel_Pago_Documento";
        private readonly CuentaCorrienteContext _context;

        public PagoRepository(CuentaCorrienteContext context)
        {
            _context = context;
        }

        public async Task<List<PagoView.PagoItem>> GetPagoAsync(int codDocumento, CancellationToken cancellationToken = default)
        {

            await using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }

            await using var command = connection.CreateCommand();
            command.CommandText = ListStoredProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CodDocumento", codDocumento));

            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

            var items = new List<PagoView.PagoItem>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                items.Add(new PagoView.PagoItem(
                     //ConvertDbHelper.ToInt32(reader["CodigoCancelacion"]),
                     ConvertDbHelper.ToInt32(reader["CodigoMedioCancelacion"]),
                     ConvertDbHelper.ToString(reader["MedioPago"]),
                     ConvertDbHelper.ToInt32(reader["CodigoMoneda"]),
                     ConvertDbHelper.ToString(reader["Moneda"]),
                     ConvertDbHelper.ToString(reader["CodigoBanco"]),
                     ConvertDbHelper.ToDecimal(reader["Importe"]),
                     ConvertDbHelper.ToString(reader["FechaCancelacion"]),
                     ConvertDbHelper.ToBoolean(reader["EsTotal"]),
                     ConvertDbHelper.ToString(reader["NumeroOperacionBancaria"]),
                     ConvertDbHelper.ToString(reader["CodCuentaBancarioContinental"]),
                     ConvertDbHelper.ToString(reader["Banco"]),
                     ConvertDbHelper.ToString(reader["CanalPago"]),
                     ConvertDbHelper.ToInt32(reader["NumeroDocumento"]),
                     ConvertDbHelper.ToString(reader["Serie"]),
                     ConvertDbHelper.ToInt32(reader["CodigoDocumento"])
                     
                ));
            }
            return items;

        }
    }

