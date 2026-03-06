using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using EFCore.BulkExtensions.SqlAdapters;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WsElecciones.Persistence.Repository;

public class ProgramacionCuentaCorrienteRepository : IProgramacionCuentaCorrienteRepository
{
    private const string CreateStoredProcedureName = "ctacte.Sp_Ins_Programacion_Cuenta_Corriente";
    private const string ListStoredProcedureName = "ctacte.Sp_Sel_Programacion_Cuenta_Corriente";
    private const string DetailStoredProcedureName = "ctacte.Sp_Sel_Programacion_Cuenta_Corriente_By_Id";
    private const string AlumnosFromProgramacionCtaCteStoredProcedureName = "ctacte.Sp_Sel_Alumnos_Programacion_Cuenta_Corriente";
    private const string AnularProgramacionCtaCteStoredProcedureName = "ctacte.Sp_Upd_Anular_Programacion_Cuenta_Corriente";
    private readonly CuentaCorrienteContext _context;

    public ProgramacionCuentaCorrienteRepository(CuentaCorrienteContext context)
    {
        _context = context;
    }

    public async Task<int> CreateProgramacionCuentaCorrienteAsync(CreateProgramacionCuentaCorriente entidad)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        await using var command = connection.CreateCommand();
        command.CommandText = CreateStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(CreateVarcharParam("@CodNivel", 50, entidad.CodNivel));
        command.Parameters.Add(CreateVarcharParam("@CodPeriodoAcademicoCtaCte", 50, entidad.CodPeriodoAcademicoCtaCte));
        command.Parameters.Add(CreateVarcharParam("@CodCompania", 50, Constants.CodCompania));
        command.Parameters.Add(CreateVarcharParam("@UsuarioCreacion", 50, Constants.CodUsuario));
        command.Parameters.Add(new SqlParameter("@FechaDeseada", SqlDbType.Date) { Value = entidad.FechaDeseada });
        command.Parameters.Add(CreateIntParam("@CodOperacionProgramadaCtaCte", entidad.CodOperacionProgramacionCtaCte));
        
        BindStructuredParameter(command, "@CodDepartamentoArr", "ctacte.TTCodDepartamentos", "CodDepartamento", entidad.CodDepartamentos);
        BindStructuredParameter(command, "@CodCampusArr", "ctacte.TTCodCampus", "CodCampus", entidad.CodCampus);
        BindStructuredParameter(command, "@FacultadCarreraArr", "ctacte.TTFacultadCarrera", entidad.FacultadCarreras);
        BindStructuredParameter(command, "@CodAlumnoArr", "ctacte.TTAlumnos", "CodAlumno", entidad.CodAlumnos);
        BindStructuredParameter(command, "@CodAlumnoArrExcluir", "ctacte.TTAlumnos", "CodAlumno", entidad.CodAlumnosExcluir);

        await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        if (await reader.ReadAsync().ConfigureAwait(false))
        {
            return reader.GetInt32(reader.GetOrdinal("CodProgramacionCtaCte"));
        }

        throw new InvalidOperationException("El procedimiento almacenado no retornó un identificador de programación.");
    }

    public async Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrientePagedResult> GetProgramacionCuentaCorrienteAsync(
        int page,
        int limit,
        string? codNivel = null,
        string? codPeriodoAcademicoCtaCte = null,
        int? codTipoOperacion = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = ListStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(CreateVarcharParam("@CodCompania", 50, Constants.CodCompania));
        command.Parameters.Add(CreateIntParam("@PageNumber", page));
        command.Parameters.Add(CreateIntParam("@PageSize", limit));
        command.Parameters.Add(CreateNullableVarcharParam("@CodNivel", 50, codNivel));
        command.Parameters.Add(CreateNullableVarcharParam("@CodPeriodoAcademicoCtaCte", 50, codPeriodoAcademicoCtaCte));
        command.Parameters.Add(CreateNullableIntParam("@CodTipoOperacion", codTipoOperacion));
        

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var items = new List<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteItem>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteItem(
                ConvertDbHelper.ToInt32(reader["Lote"]),
                ConvertDbHelper.ToString(reader["UsuarioSolicitante"]),
                ConvertDbHelper.ToDateTimeNull(reader["FechaDeseada"]),
                ConvertDbHelper.ToDateTimeNull(reader["FechaEjecucion"]),
                ConvertDbHelper.ToString(reader["EstadoEjecucion"]),
                ConvertDbHelper.ToString(reader["ColorWeb"]),
                ConvertDbHelper.ToInt32(reader["CantidadAlumnosProcesados"]),
                ConvertDbHelper.ToInt32(reader["CantidadAlumnosSatisfactorios"]),
                ConvertDbHelper.ToInt32(reader["AlumnosErrados"])
            ));
        }

        var totalRegistros = 0;
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false)
            && await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            totalRegistros = reader.GetInt32(reader.GetOrdinal("TotalRegistros"));
        }

        return new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrientePagedResult(
            items,
            totalRegistros,
            page,
            limit);
    }

    public async Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteDetail?> GetProgramacionCuentaCorrienteByIdAsync(
        int codProgramacionCtaCte,
        string codCompania,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codCompania))
        {
            throw new ArgumentException("El código de compañía es obligatorio.", nameof(codCompania));
        }

        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = DetailStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(CreateIntParam("@CodProgramacionCtaCte", codProgramacionCtaCte));
        command.Parameters.Add(CreateVarcharParam("@CodCompania", 50, codCompania.Trim()));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        var lote = ConvertDbHelper.ToInt32(reader["Lote"]);
        DateTime? fechaDeseada = ConvertDbHelper.ToDateTimeNull(reader["FechaDeseada"]);
        var codNivel = ConvertDbHelper.ToString(reader["CodNivel"]);
        var codPeriodoAcademicoCtaCte = ConvertDbHelper.ToString(reader["CodPeriodoAcademicoCtaCte"]); ;
        var codEstadoProgramacionCtaCte = ConvertDbHelper.ToInt32(reader["CodEstadoProgramacionCtaCte"]);
        var estadoProgramacionCtaCte = ConvertDbHelper.ToString(reader["EstadoProgramacionCtaCte"]);
        var colorWeb = ConvertDbHelper.ToString(reader["ColorWeb"]);
        var cantidadAlumnosProcesados = ConvertDbHelper.ToInt32(reader["CantidadAlumnosProcesados"]);
        var cantidadAlumnosErrados = ConvertDbHelper.ToInt32(reader["CantidadAlumnosErrados"]);
        var codOperationType = ConvertDbHelper.ToInt32(reader["CodOperationType"]);
        var operationType = ConvertDbHelper.ToString(reader["OperationType"]);

        var departamentos = new List<string>();
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false))
        {
            var codDepartamentoOrdinal = reader.GetOrdinal("CodDepartamento");
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (!reader.IsDBNull(codDepartamentoOrdinal))
                {
                    departamentos.Add(reader.GetString(codDepartamentoOrdinal));
                }
            }
        }

        var campus = new List<string>();
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false))
        {
            var codCampusOrdinal = reader.GetOrdinal("CodCampus");
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (!reader.IsDBNull(codCampusOrdinal))
                {
                    campus.Add(reader.GetString(codCampusOrdinal));
                }
            }
        }

        var facultades = new List<string>();
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false))
        {
            var codFacultadOrdinal = reader.GetOrdinal("CodFacultad");
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (!reader.IsDBNull(codFacultadOrdinal))
                {
                    facultades.Add(reader.GetString(codFacultadOrdinal));
                }
            }
        }

        var carreras = new List<string>();
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false))
        {
            var codCarreraOrdinal = reader.GetOrdinal("CodCarrera");
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (!reader.IsDBNull(codCarreraOrdinal))
                {
                    carreras.Add(reader.GetString(codCarreraOrdinal));
                }
            }
        }

        return new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteDetail(
            lote,
            fechaDeseada,
            codNivel,
            codPeriodoAcademicoCtaCte,
            codEstadoProgramacionCtaCte,
            estadoProgramacionCtaCte,
            colorWeb,
            cantidadAlumnosProcesados,
            cantidadAlumnosErrados,
            departamentos,
            campus,
            facultades,
            carreras,
            codOperationType,
            operationType);
    }

    public async Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoPagedResult>
        GetProgramacionCuentaCorrienteAlumnosAsync(
            int codProgramacionCtaCte,
            string codCompania,
            int page,
            int limit,
            bool estadoOk = true,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codCompania))
        {
            throw new ArgumentException("El código de compañía es obligatorio.", nameof(codCompania));
        }

        var connection = _context.Database.GetDbConnection();
        var shouldCloseConnection = connection.State != ConnectionState.Open;

        if (shouldCloseConnection)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        try
        {
            var (items, totalRegistros) = await ExecuteAlumnosStoredProcedureAsync(
                connection,
                codProgramacionCtaCte,
                codCompania,
                page,
                limit,
                estadoOk,
                cancellationToken).ConfigureAwait(false);

            return new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoPagedResult(
                items,
                totalRegistros,
                page,
                limit);
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    public async Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnosExportResult>
        GetProgramacionCuentaCorrienteAlumnosExportAsync(
            int codProgramacionCtaCte,
            string codCompania,
            int limit,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codCompania))
        {
            throw new ArgumentException("El código de compañía es obligatorio.", nameof(codCompania));
        }

        var connection = _context.Database.GetDbConnection();
        var shouldCloseConnection = connection.State != ConnectionState.Open;

        if (shouldCloseConnection)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        try
        {
            var (exitosos, _) = await ExecuteAlumnosStoredProcedureAsync(
                connection,
                codProgramacionCtaCte,
                codCompania,
                page: 1,
                limit,
                estadoOk: true,
                cancellationToken).ConfigureAwait(false);

            var (conErrores, _) = await ExecuteAlumnosStoredProcedureAsync(
                connection,
                codProgramacionCtaCte,
                codCompania,
                page: 1,
                limit,
                estadoOk: false,
                cancellationToken).ConfigureAwait(false);

            return new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnosExportResult(
                exitosos,
                conErrores);
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    public async Task<int> AnularProgramacionCuentaCorrienteAsync(
        int codProgramacionCtaCte,
        string codCompania,
        string codUsuario,
        CancellationToken cancellationToken = default)
    {
        if (codProgramacionCtaCte <= 0)
        {
            throw new ArgumentException("El identificador de la programación debe ser mayor a cero.", nameof(codProgramacionCtaCte));
        }

        if (string.IsNullOrWhiteSpace(codCompania))
        {
            throw new ArgumentException("El código de compañía es obligatorio.", nameof(codCompania));
        }

        if (string.IsNullOrWhiteSpace(codUsuario))
        {
            throw new ArgumentException("El código de usuario es obligatorio.", nameof(codUsuario));
        }

        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = AnularProgramacionCtaCteStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(CreateVarcharParam("@CodCompania", 50, codCompania.Trim()));
        command.Parameters.Add(CreateIntParam("@CodCuentaCorriente", codProgramacionCtaCte));
        command.Parameters.Add(CreateVarcharParam("@CodUsuario", 50, codUsuario.Trim()));

        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        if (result is null || result == DBNull.Value)
        {
            return 0;
        }

        return result switch
        {
            int intValue => intValue,
            long longValue => (int)longValue,
            short shortValue => shortValue,
            byte byteValue => byteValue,
            decimal decimalValue => (int)decimalValue,
            _ => int.TryParse(result.ToString(), out var parsed) ? parsed : 0
        };
    }

    private async Task<(List<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoItem> Items, int TotalRegistros)>
        ExecuteAlumnosStoredProcedureAsync(
            DbConnection connection,
            int codProgramacionCtaCte,
            string codCompania,
            int page,
            int limit,
            bool estadoOk,
            CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = AlumnosFromProgramacionCtaCteStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(CreateIntParam("@CodProgramacionCtaCte", codProgramacionCtaCte));
        command.Parameters.Add(CreateVarcharParam("@CodCompania", 50, codCompania.Trim()));
        command.Parameters.Add(CreateIntParam("@Page", page));
        command.Parameters.Add(CreateIntParam("@Limit", limit));
        command.Parameters.Add(new SqlParameter("@EstadoOK", SqlDbType.Bit) { Value = estadoOk });

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var items = new List<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoItem>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var codAlumno = ConvertDbHelper.ToString(reader["CodAlumno"]);
            var apellidosNombres = ConvertDbHelper.ToString(reader["ApellidosNombres"]);
            var carrera = ConvertDbHelper.ToString(reader["Carrera"]);
            var modalidad = ConvertDbHelper.ToString(reader["Modalidad"]);
            var campus = ConvertDbHelper.ToString(reader["Campus"]);
            var descripcionIncidente = ConvertDbHelper.ToString(reader["DescripcionIncidente"]);

            items.Add(new ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoItem(
                codAlumno,
                apellidosNombres,
                carrera,
                modalidad,
                campus,
                string.IsNullOrWhiteSpace(descripcionIncidente) ? null : descripcionIncidente.Trim()));
        }

        var totalRegistros = 0;
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false) &&
            await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var totalRegistrosOrdinal = reader.GetOrdinal("TotalRegistros");
            totalRegistros = reader.IsDBNull(totalRegistrosOrdinal)
                ? 0
                : reader.GetInt32(totalRegistrosOrdinal);
        }

        return (items, totalRegistros);
    }

    private static void BindStructuredParameter(
        DbCommand command,
        string parameterName,
        string typeName,
        string columnName,
        IReadOnlyCollection<string> values)
    {
        var table = new DataTable();
        table.Columns.Add(columnName, typeof(string));

        foreach (var value in values ?? System.Array.Empty<string>())
        {
            var sanitized = string.IsNullOrWhiteSpace(value)
                ? throw new InvalidOperationException($"Todos los valores de {columnName} deben contener un código válido.")
                : value.Trim();

            table.Rows.Add(sanitized);
        }

        var param = new SqlParameter(parameterName, table)
        {
            TypeName = typeName,
            SqlDbType = SqlDbType.Structured
        };
        command.Parameters.Add(param);
    }

    private static void BindStructuredParameter(
        DbCommand command,
        string parameterName,
        string typeName,
        IReadOnlyCollection<CreateProgramacionCuentaCorriente.FacultadCarrera> values)
    {
        var table = new DataTable();
        table.Columns.Add("CodFacultad", typeof(string));
        table.Columns.Add("CodCarrera", typeof(string));

        foreach (var pair in values ?? System.Array.Empty<CreateProgramacionCuentaCorriente.FacultadCarrera>())
        {
            var codFacultad = string.IsNullOrWhiteSpace(pair.CodFacultad)
                ? throw new InvalidOperationException("Todos los valores de CodFacultad deben contener un código válido.")
                : pair.CodFacultad.Trim();

            var codCarrera = string.IsNullOrWhiteSpace(pair.CodCarrera)
                ? (object)DBNull.Value
                : pair.CodCarrera!.Trim();

            table.Rows.Add(codFacultad, codCarrera);
        }

        var param = new SqlParameter(parameterName, table)
        {
            TypeName = typeName,
            SqlDbType = SqlDbType.Structured
        };
        command.Parameters.Add(param);
    }

    private static SqlParameter CreateVarcharParam(string name, int size, string value) =>
        new(name, SqlDbType.VarChar, size)
        {
            Value = value
        };

    private static SqlParameter CreateNullableVarcharParam(string name, int size, string? value) =>
        new(name, SqlDbType.VarChar, size)
        {
            Value = value ?? (object)DBNull.Value
        };

    private static SqlParameter CreateIntParam(string name, int value) =>
        new(name, SqlDbType.Int)
        {
            Value = value
        };
    private static SqlParameter CreateNullableIntParam(string name, int? value) =>
        new(name, SqlDbType.Int)
        {
            Value = value ?? (object)DBNull.Value
        };
}
