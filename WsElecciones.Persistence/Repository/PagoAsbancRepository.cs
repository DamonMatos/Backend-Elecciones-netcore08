using Azure;
using Azure.Core;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace WsElecciones.Persistence.Repository;

public class PagoAsbancRepository : Repository<PagoAsbanc>, IPagoAsbancRepository
{
    private const string ListaDeudaStoredProcedureName = "ctacte.Sp_Sel_Cuenta_Corriente_Deuda";
    private const string ReviertePagoStoredProcedureName = "facturacion.Sp_Revierte_Importe_Cancelado";
    private readonly CuentaCorrienteContext _context;

    public PagoAsbancRepository(CuentaCorrienteContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ConsultaDeudaCuentaCorrienteView.ConsultaDeudaCuentaCorrienteResult> GetConsultaDeudaAsync(string? codNivel, string? codAlumno,string? codProducto, CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = ListaDeudaStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@CodNivel", codNivel));
        command.Parameters.Add(new SqlParameter("@CodAlumno", codAlumno));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        var items = new List<ConsultaDeudaCuentaCorrienteView.ConsultaDeudaCuentaCorrienteItem>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new ConsultaDeudaCuentaCorrienteView.ConsultaDeudaCuentaCorrienteItem(
                ConvertDbHelper.ToString(reader["NumDocumento"]),
                ConvertDbHelper.ToString(codProducto),
                ConvertDbHelper.ToString(reader["DescDocumento"]),
                ConvertDbHelper.ToDateTimeNull(reader["FechaVencimiento"]),
                ConvertDbHelper.ToDateTimeNull(reader["FechaEmision"]),
                ConvertDbHelper.ToDoubleNull(reader["Deuda"]),
                ConvertDbHelper.ToDoubleNull(reader["Mora"]),
                ConvertDbHelper.ToDoubleNull(reader["GastosAdm"]),
                ConvertDbHelper.ToDoubleNull(reader["PagoMinimo"]),
                ConvertDbHelper.ToString(reader["Periodo"]),
                ConvertDbHelper.ToInt32Null(reader["Anio"]),
                ConvertDbHelper.ToString(reader["Cuota"]),
                ConvertDbHelper.ToString(reader["MonedaDoc"])
            ));
        }
        return new ConsultaDeudaCuentaCorrienteView.ConsultaDeudaCuentaCorrienteResult(items);
    }

    public async Task<int> RevertirPagoAsync(int codDocumento, string codUsuario)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
        await using var command = connection.CreateCommand();
        command.CommandText = ReviertePagoStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@CodDocumento", codDocumento));
        command.Parameters.Add(new SqlParameter("@Usuario", codUsuario));
        var resultado = await command.ExecuteNonQueryAsync();
        return resultado;
    }
}

