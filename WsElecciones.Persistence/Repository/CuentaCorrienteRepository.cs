using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain;
using WsElecciones.Domain.Entitites;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.CuentasCorriente;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WsElecciones.Persistence.Repository;

public class CuentaCorrienteRepository : Repository<CuentaCorriente>, ICuentaCorrienteRepository
{
    private const string SpGeneraCuentaCorriente = "[ctacte].[Sp_Genera_Cuenta_Corriente]";
    private const string SpSelResultadoGeneraCuentaCorriente = "[ctacte].[Sp_Sel_Resultado_Genera_Cuenta_Corriente]";
    private const string CuentaCorrienteByIdStoredProcedureName = "[ctacte].[Sp_Sel_Cuenta_Corriente_By_Id]";
    private const string EliminaCuentaCorrienteStoredProcedureName = "[ctacte].[Sp_Del_CuentaCorriente]";
    private const string ReiniciaCuentaCorrienteStoredProcedureName = "[ctacte].[Sp_Reinicia_CuentaCorriente]";
    private const string SpActualizaCuentaCorriente = "ctacte.Sp_Actualiza_Individual_CtaCte";

    private const string ActualizaMasivaStoredProcedureName = "ctacte.Sp_Act_Masiva_Fecha_Vencimiento";
    private const string ListaMasivaStoredProcedureName = "ctacte.Sp_Sel_Resultado_Act_Masiva_Fecha_Vencimiento";

    private readonly IStoredProcedureExecutor spExecutor;

    public CuentaCorrienteRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor) : base(context)
    {
        this.spExecutor = spExecutor;
    }
    public async Task<CuentaCorrienteView.CuentaCorriente> CreateCuentaCorrienteAsync(CreateCuentaCorrienteView entidad, CancellationToken cancellationToken = default)
    {
        var parametros = BuildParametrosCreateCuentaCorriente(entidad);
        var result = await spExecutor.ExecuteScalarAsync<int>(
                SpGeneraCuentaCorriente,
                parametros,
                cancellationToken);
        var codCuentaCorriente = parametros.FirstOrDefault(x => x.ParameterName == "@pcod_cuenta_corriente")?.Value;
        var estado = parametros.FirstOrDefault(x => x.ParameterName == "@pestado")?.Value.ToString()!;
        var resultado = parametros.FirstOrDefault(x => x.ParameterName == "@presultado")?.Value?.ToString()!;
        if(estado == "1" && codCuentaCorriente is not null)
        {
            //leemos el resultado de la ejecución
            var data = await spExecutor.ExecuteMultipleReaderAsync(SpSelResultadoGeneraCuentaCorriente,
                [
                    reader => new CuentaCorrienteView.CuentaCorrienteCuota(
                        ConvertDbHelper.ToLong(reader["CodCuentaCorrienteCuota"]),
                        ConvertDbHelper.ToInt32(reader["NumeroCuota"]),
                        ConvertDbHelper.ToInt32(reader["CodConcepto"]),
                        ConvertDbHelper.ToString(reader["Concepto"]),
                        ConvertDbHelper.ToInt32(reader["Cantidad"]),
                        ConvertDbHelper.ToString(reader["Moneda"]),
                        ConvertDbHelper.ToDecimal(reader["TasaImporte"]),
                        ConvertDbHelper.ToDecimalNull(reader["TasaImpuesto"]),
                        ConvertDbHelper.ToDateTimeNull(reader["FechaVencimiento"]),
                        ConvertDbHelper.ToDateTimeNull(reader["FechaProrroga"]),
                            []),
                    reader => new CuentaCorrienteView.CuentaCorrienteBeneficio(
                        ConvertDbHelper.ToLong(reader["CodCuentaCorrienteCuota"]),
                        ConvertDbHelper.ToInt32(reader["CodBeneficio"]),
                        ConvertDbHelper.ToString(reader["Beneficio"]),
                        ConvertDbHelper.ToDecimal(reader["Importe"]),
                        ConvertDbHelper.ToString(reader["Origen"]),
                        ConvertDbHelper.ToBoolean(reader["Mandatorio"]),
                        ConvertDbHelper.ToInt32(reader["CodConcepto"])
                        )
                ]
                , [SqlParameterFactory.CreateInt("@pcod_cuenta_corriente", int.Parse(codCuentaCorriente.ToString()!))]
                , cancellationToken);
            var cuotas = data.ElementAtOrDefault(0)?.Cast<CuentaCorrienteView.CuentaCorrienteCuota>().ToList();
            var beneficios = data.ElementAtOrDefault(1)?.Cast<CuentaCorrienteView.CuentaCorrienteBeneficio>();
            if (cuotas is not null && beneficios is not null)
            {
                foreach (var cuentaCorrienteCuota in cuotas)
                {
                    foreach (var cuentaCorrienteBeneficio in beneficios.Where(b =>
                                 b.CodCuentaCorrienteCuota == cuentaCorrienteCuota.CodCuentaCorrienteCuota && b.CodConcepto == cuentaCorrienteCuota.CodConcepto))
                    {
                        cuentaCorrienteCuota.Beneficios.Add(cuentaCorrienteBeneficio);
                    }
                }
            }
            return new CuentaCorrienteView.CuentaCorriente(
                long.Parse(codCuentaCorriente!.ToString()!),
                [],
                cuotas ?? [],
                resultado
            );

        }
        return new CuentaCorrienteView.CuentaCorriente(
                null,
                [resultado],
                [],
                resultado
            );

    }

    public async Task<CuentaCorrienteView.CuentaCorriente> UpdateCuentaCorrienteAsync(UpdateCuentaCorrienteView entidad, CancellationToken cancellationToken = default)
    {
        var parametros = BuildParametrosUpdateCuentaCorriente(entidad);
        var result = await spExecutor.ExecuteScalarAsync<int>(
                SpGeneraCuentaCorriente,
                parametros,
                cancellationToken);
        var codCuentaCorriente = parametros.FirstOrDefault(x => x.ParameterName == "@pcod_cuenta_corriente")?.Value;
        var estado = parametros.FirstOrDefault(x => x.ParameterName == "@pestado")?.Value.ToString()!;
        var resultado = parametros.FirstOrDefault(x => x.ParameterName == "@presultado")?.Value?.ToString()!;
        if (estado == "1" && codCuentaCorriente is not null)
        {
            //leemos el resultado de la ejecución
            var data = await spExecutor.ExecuteMultipleReaderAsync(SpSelResultadoGeneraCuentaCorriente,
                [
                    reader => new CuentaCorrienteView.CuentaCorrienteCuota(
                        ConvertDbHelper.ToLong(reader["CodCuentaCorrienteCuota"]),
                        ConvertDbHelper.ToInt32(reader["NumeroCuota"]),
                        ConvertDbHelper.ToInt32(reader["CodConcepto"]),
                        ConvertDbHelper.ToString(reader["Concepto"]),
                        ConvertDbHelper.ToInt32(reader["Cantidad"]),
                        ConvertDbHelper.ToString(reader["Moneda"]),
                        ConvertDbHelper.ToDecimal(reader["TasaImporte"]),
                        ConvertDbHelper.ToDecimalNull(reader["TasaImpuesto"]),
                        ConvertDbHelper.ToDateTimeNull(reader["FechaVencimiento"]),
                        ConvertDbHelper.ToDateTimeNull(reader["FechaProrroga"]),
                            []),
                    reader => new CuentaCorrienteView.CuentaCorrienteBeneficio(
                        ConvertDbHelper.ToLong(reader["CodCuentaCorrienteCuota"]),
                        ConvertDbHelper.ToInt32(reader["CodBeneficio"]),
                        ConvertDbHelper.ToString(reader["Beneficio"]),
                        ConvertDbHelper.ToDecimal(reader["Importe"]),
                        ConvertDbHelper.ToString(reader["Origen"]),
                        ConvertDbHelper.ToBoolean(reader["Mandatorio"]),
                        ConvertDbHelper.ToInt32(reader["CodConcepto"])
                        )
                ]
                , [SqlParameterFactory.CreateInt("@pcod_cuenta_corriente", int.Parse(codCuentaCorriente.ToString()!))]
                , cancellationToken);
            var cuotas = data.ElementAtOrDefault(0)?.Cast<CuentaCorrienteView.CuentaCorrienteCuota>().ToList();
            var beneficios = data.ElementAtOrDefault(1)?.Cast<CuentaCorrienteView.CuentaCorrienteBeneficio>();
            if (cuotas is not null && beneficios is not null)
            {
                foreach (var cuentaCorrienteCuota in cuotas)
                {
                    foreach (var cuentaCorrienteBeneficio in beneficios.Where(b =>
                                 b.CodCuentaCorrienteCuota == cuentaCorrienteCuota.CodCuentaCorrienteCuota && b.CodConcepto == cuentaCorrienteCuota.CodConcepto))
                    {
                        cuentaCorrienteCuota.Beneficios.Add(cuentaCorrienteBeneficio);
                    }
                }
            }
            return new CuentaCorrienteView.CuentaCorriente(
                long.Parse(codCuentaCorriente!.ToString()!),
                [],
                cuotas ?? [],
                resultado
            );

        }
        return new CuentaCorrienteView.CuentaCorriente(
                null,
                [resultado],
                [],
                resultado
            );

    }

    public async Task<PagedView<GetCuentasCorrienteView>> GetCuentasCorrienteByFilterAsync(
        int page, int limit,
        string codNivel,
        string codPeriodoAcademico,
        string codCompania,
        string? nombreAlumno = null,
        string? empresa = null,
         CancellationToken cancellationToken = default)
    {
        
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = "[ctacte].[Sp_Sel_Cuenta_corriente]";
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@CodCompania", 60, codCompania.Trim()));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@CodNivel", 50, codNivel.Trim()));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@CodPeriodoAcademico", 50, codPeriodoAcademico?.Trim()));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@Alumno", 250, nombreAlumno));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@Empresa", 250, empresa));
        command.Parameters.Add(SqlParameterFactory.CreateInt("@Page", page));
        command.Parameters.Add(SqlParameterFactory.CreateInt("@Limit", limit));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var items = new List<GetCuentasCorrienteView>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new GetCuentasCorrienteView(
                ConvertDbHelper.ToInt32(reader["cod_cuenta_corriente"]),
                ConvertDbHelper.ToString(reader["CodigoCompania"]),
                ConvertDbHelper.ToString(reader["CodigoNivel"]),
                ConvertDbHelper.ToString(reader["CodigoAlumno"]),
                ConvertDbHelper.ToString(reader["DatosAlumno"]),
                ConvertDbHelper.ToString(reader["PeriodoAcademico"]),
                ConvertDbHelper.ToString(reader["CodigoPeriodoAcademico"]),
                ConvertDbHelper.ToString(reader["CodigoCarrera"]),
                ConvertDbHelper.ToString(reader["Carrera"]),
                ConvertDbHelper.ToString(reader["CodigoDepartamento"]),
                ConvertDbHelper.ToString(reader["Departamento"]),
                ConvertDbHelper.ToString(reader["CodigoCampus"]),
                ConvertDbHelper.ToString(reader["Campus"]),
                ConvertDbHelper.ToString(reader["CodigoCategoriaPago"]),
                ConvertDbHelper.ToString(reader["CodigoTasaCuotas"]),
                ConvertDbHelper.ToString(reader["TipoDocumentoFinanciero"]),
                ConvertDbHelper.ToString(reader["TipoAlumno"]),
                ConvertDbHelper.ToString(reader["CodigoEmpresa"]),
                ConvertDbHelper.ToString(reader["DatosEmpresa"]),
                null,null, null, null, null, null, null, null, null, null,
                //ConvertDbHelper.ToString(reader["IdSis1"]),
                //ConvertDbHelper.ToString(reader["DescripcionSis1"]),
                //ConvertDbHelper.ToString(reader["IdSis2"]),
                //ConvertDbHelper.ToString(reader["DescripcionSis2"]),
                //ConvertDbHelper.ToString(reader["IdSis3"]),
                //ConvertDbHelper.ToString(reader["DescripcionSis3"]),
                //ConvertDbHelper.ToString(reader["IdSis4"]),
                //ConvertDbHelper.ToString(reader["DescripcionSis4"]),
                //ConvertDbHelper.ToString(reader["IdSis5"]),
                //ConvertDbHelper.ToString(reader["DescripcionSis5"]),
                ConvertDbHelper.ToString(reader["Vigente"]),
                ConvertDbHelper.ToString(reader["CodigoPersona"]),
                ConvertDbHelper.ToString(reader["CodigoMoneda"]),
                ConvertDbHelper.ToDecimal(reader["ImporteTotal"]),
                ConvertDbHelper.ToDecimal(reader["Impuesto"]),
                ConvertDbHelper.ToDecimal(reader["TotalPagado"]),
                ConvertDbHelper.ToDecimal(reader["TotalAPagar"]),
                ConvertDbHelper.ToDecimal(reader["Saldo"]),
                ConvertDbHelper.ToDecimal(reader["BeneficiosTotales"])
            ));
        }

        var totalRegistros = 0;
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false) &&
            await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            totalRegistros = ConvertDbHelper.ToInt32(reader["TotalRegistros"]);
        }

        return new PagedView<GetCuentasCorrienteView>(
            items,
            totalRegistros,
            page,
            limit);
    }
   
    public async Task<CuentaCorrienteView.CuentaCorrienteDetail?> GetCuentaCorrienteByIdAsync(
        string codCompania,
        int codCuentaCorriente,
        CancellationToken cancellationToken = default)
    {
       
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = CuentaCorrienteByIdStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@CodCompania", 50, codCompania.Trim()));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@CodCuentaCorriente", 50, codCuentaCorriente.ToString()));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        return new CuentaCorrienteView.CuentaCorrienteDetail(
            ConvertDbHelper.ToString(reader["Alumno"]),
            ConvertDbHelper.ToString(reader["CodCuentaCorriente"]),
            ConvertDbHelper.ToString(reader["Vigente"]),
            ConvertDbHelper.ToString(reader["Nivel"]),
            ConvertDbHelper.ToString(reader["Carrera"]),
            ConvertDbHelper.ToString(reader["Modalidad"]),
            ConvertDbHelper.ToString(reader["PeriodoAcademico"]),
            ConvertDbHelper.ToString(reader["Campus"]),
            ConvertDbHelper.ToString(reader["Escala"]),
            ConvertDbHelper.ToInt32Null(reader["CantidadCreditos"]),
            ConvertDbHelper.ToInt32Null(reader["CantidadCursos"]),
            ConvertDbHelper.ToString(reader["CategoriaPago"]),
            ConvertDbHelper.ToDecimalNull(reader["PrecioCargaAcademica"]),
            ConvertDbHelper.ToString(reader["TipoDocumento"]),
            ConvertDbHelper.ToInt32(reader["CodTipoDocumento"]),
            ConvertDbHelper.ToString(reader["Empresa"]),
            ConvertDbHelper.ToString(reader["CodEmpresa"]),
            ConvertDbHelper.ToInt32Null(reader["CantidadCuotas"]));
    }

    public async Task<DeleteCuentaCorrienteResultView?> DeleteCuentaCorrienteAsync(
        int codCuentaCorriente,
        string motivoEliminacion,
        string usuario,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = EliminaCuentaCorrienteStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        // TODO:Asumiendo que no tienen matricula, igual el SP verifica eso
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@ptiene_matricula_activa", 2, "NO"));
        command.Parameters.Add(SqlParameterFactory.CreateNullableInt("@pcod_cuenta_corriente", codCuentaCorriente));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@pmotivo_eliminacion", 4000, motivoEliminacion));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@pusuario", 100, usuario));
        // TODO: Preguntar por este parámetro, en el codigo solo verifica que sea IN
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@porigen", 2, "IN"));

        var pTipoAlumnoApto = new SqlParameter("@ptipo_alumno_apto", SqlDbType.Char, 2)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pTipoAlumnoApto);
        var pElimino = new SqlParameter("@pelimino", SqlDbType.Char, 2)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pElimino);
        var pEstado = new SqlParameter("@pestado", SqlDbType.Char, 2)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pEstado);
        var pResultado = new SqlParameter("@presultado", SqlDbType.VarChar, 500)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pResultado);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
       
        return new DeleteCuentaCorrienteResultView(
            pTipoAlumnoApto.Value?.ToString() ?? "0",
            pElimino.Value?.ToString() ?? string.Empty,
            pEstado.Value?.ToString() ?? "0",
            pResultado.Value?.ToString() ?? string.Empty
        );
    }

    public async Task<ReiniciaCuentaCorrienteResultView?> ReiniciaCuentaCorrienteAsync(
        int codCuentaCorriente,
        int numeroCuotaDesde,
        string motivoReinicio,
        string usuario,
        CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = ReiniciaCuentaCorrienteStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@ptiene_matricula_activa", 2,"NO"));
        command.Parameters.Add(SqlParameterFactory.CreateInt("@pcod_cuenta_corriente", codCuentaCorriente));
        command.Parameters.Add(SqlParameterFactory.CreateInt("@pnumero_cuota_desde", numeroCuotaDesde));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@pmotivo_reinicio",500, motivoReinicio));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@porigen", 2, "IN"));
        command.Parameters.Add(SqlParameterFactory.CreateNullableVarchar("@pusuario",50, usuario));

        var pTipoAlumnoApto = SqlParameterFactory.CreateOutput("@ptipo_alumno_apto", SqlDbType.Char, 2);
        command.Parameters.Add(pTipoAlumnoApto);
        var pReinicio = SqlParameterFactory.CreateOutput("@preinicio", SqlDbType.Char, 2);
        command.Parameters.Add(pReinicio);
        var pEstado = SqlParameterFactory.CreateOutput("@pestado", SqlDbType.Char, 2);
        command.Parameters.Add(pEstado);
        var pResultado = SqlParameterFactory.CreateOutput("@presultado", SqlDbType.VarChar, 500);
        command.Parameters.Add(pResultado);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);


        return new ReiniciaCuentaCorrienteResultView(
            pTipoAlumnoApto.Value?.ToString() ?? "0",
            pReinicio.Value?.ToString() ?? string.Empty,
            pEstado.Value?.ToString() ?? "0",
            pResultado.Value?.ToString() ?? string.Empty
        );
    }
    public async Task<UpdCuentaCorrienteAtributoFinancierosView> UpdCuentaCorrienteAtributoFinancierosAsync(UpdCuentaCorrienteAtributoFinancieros cuentaCorrienteAtributoFinancieros, CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);


        await using var command = connection.CreateCommand();
        command.CommandText = SpActualizaCuentaCorriente;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@PCOD_CUENTA_CORRIENTE", cuentaCorrienteAtributoFinancieros.CodCuentaCorriente));
        command.Parameters.Add(new SqlParameter("@PCOD_TIPO_DOCUMENTO_FINANCIERO", cuentaCorrienteAtributoFinancieros.CodTipoDocumentoFinanciero));
        command.Parameters.Add(new SqlParameter("@PCANTIDAD_CUOTAS", cuentaCorrienteAtributoFinancieros.CantidadCuotas));
        command.Parameters.Add(new SqlParameter("@PCOD_EMPRESA", cuentaCorrienteAtributoFinancieros.CodEmpresa));
        command.Parameters.Add(new SqlParameter("@PRAZON_SOCIAL", cuentaCorrienteAtributoFinancieros.RazonSocial));
        command.Parameters.Add(new SqlParameter("@PUSUARIO", cuentaCorrienteAtributoFinancieros.Usuario));
        var pEstado = new SqlParameter("@PESTADO", SqlDbType.Char, 1)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pEstado);
        var pResultado = new SqlParameter("@PRESULTADO", SqlDbType.VarChar, 500)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pResultado);
        await command.ExecuteNonQueryAsync(cancellationToken);
        return new UpdCuentaCorrienteAtributoFinancierosView(
            pEstado.Value?.ToString() ?? "0",
            pResultado.Value?.ToString() ?? string.Empty
        );
    }

    #region "Metodos Privados"
    private IEnumerable<SqlParameter> BuildParametrosCreateCuentaCorriente(CreateCuentaCorrienteView entidad)
    {
        var otrosCargosTable = new DataTable();
        otrosCargosTable.Columns.Add("CodConcepto", typeof(string));
        if (entidad.OtrosCargos is not null)
        {
            foreach (var codConcepto in entidad.OtrosCargos)
            {
                if (codConcepto > 0)
                    otrosCargosTable.Rows.Add(codConcepto);
            }
        }

        return
        [
            SqlParameterFactory.CreateOutput("@pcod_cuenta_corriente", SqlDbType.Int),
            SqlParameterFactory.CreateOutput("@pcod_cuenta_corriente_cuota", SqlDbType.Int),
            SqlParameterFactory.CreateVarchar("@pcod_compania", 50, entidad.CodCompania),
            SqlParameterFactory.CreateVarchar("@pcod_nivel", 50, entidad.CodNivel),
            SqlParameterFactory.CreateVarchar("@pcod_campus", 50, entidad.CodCampus),
            SqlParameterFactory.CreateVarchar("@pcod_departamento", 50, entidad.CodDepartamento),
            SqlParameterFactory.CreateVarchar("@pcod_periodo_academico", 50, entidad.CodPeriodoAcademico),
            SqlParameterFactory.CreateVarchar("@pcod_carrera", 50, entidad.CodCarrera),
            SqlParameterFactory.CreateVarchar("@pcod_facultad", 50, entidad.CodFacultad),
            SqlParameterFactory.CreateVarchar("@pcod_alumno", 50, entidad.CodAlumno),
            SqlParameterFactory.CreateNullableVarchar("@pcod_colegio", 50, entidad.CodColegio),
            SqlParameterFactory.CreateVarchar("@ptipo_alumno", 2, entidad.TipoAlumno),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis1", 50, entidad.IdSis1),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis2", 50, entidad.IdSis2),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis3", 50, entidad.IdSis3),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis4", 50, entidad.IdSis4),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis5", 50, entidad.IdSis5),
            SqlParameterFactory.CreateNullableInt("@pcod_beneficio", entidad.CodBeneficio),
            SqlParameterFactory.CreateVarchar("@pdatos_personales_alumno", 250, entidad.DatosPersonalesAlumno),
            SqlParameterFactory.CreateVarchar("@pdescripcion_carrera", 250, entidad.DescripcionCarrera),
            SqlParameterFactory.CreateVarchar("@pdescripcion_departamento", 250, entidad.DescripcionDepartamento),
            SqlParameterFactory.CreateVarchar("@pdescripcion_campus", 250, entidad.DescripcionCampus),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis1", 250, entidad.DescripcionIdSis1),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis2", 250, entidad.DescripcionIdSis2),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis3", 250, entidad.DescripcionIdSis3),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis4", 250, entidad.DescripcionIdSis4),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis5", 250, entidad.DescripcionIdSis5),
            SqlParameterFactory.CreateBigInt("@pcod_persona", entidad.CodPersona),
            SqlParameterFactory.CreateStructured("@otrosconceptos", "ctacte.TTOtrosCargos", otrosCargosTable),
            SqlParameterFactory.CreateDateTime("@pfecha_inicio", entidad.FechaInicioClases.Date),
            SqlParameterFactory.CreateVarchar("@pusuario", 50, Constants.CodUsuario),
            SqlParameterFactory.CreateChar("@ptipo_operacion", 1, 'I'),
            SqlParameterFactory.CreateNullableInt("@pcantidad_creditos", entidad.CreditosMatriculados?.CantidadCreditos),
            SqlParameterFactory.CreateNullableInt("@pcantidad_cursos", entidad.CreditosMatriculados?.CantidadCursos),
            SqlParameterFactory.CreateNullableVarchar("@pid_modulo", 50, entidad.CreditosMatriculados?.IdModulo),
            SqlParameterFactory.CreateNullableVarchar("@pcod_empresa_parametro", 50, entidad.CreditosMatriculados?.CodEmpresa),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_empresa_parametro", 250, entidad.CreditosMatriculados?.DatosEmpresa),
            SqlParameterFactory.CreateNullableInt("@pcod_tasa_cuotas_parametro", entidad.CreditosMatriculados?.CodTasaCuotas),
            SqlParameterFactory.CreateOutput("@pestado", SqlDbType.Char, 1),
            SqlParameterFactory.CreateOutput("@presultado", SqlDbType.VarChar, 500)
        ];
    }
    private IEnumerable<SqlParameter> BuildParametrosUpdateCuentaCorriente(UpdateCuentaCorrienteView entidad)
    {
        var otrosCargosTable = new DataTable();
        otrosCargosTable.Columns.Add("CodConcepto", typeof(string));
        if (entidad.OtrosCargos is not null)
        {
            foreach (var codConcepto in entidad.OtrosCargos)
            {
                if (codConcepto > 0)
                    otrosCargosTable.Rows.Add(codConcepto);
            }
        }

        return
        [
            SqlParameterFactory.CreateOutput("@pcod_cuenta_corriente", SqlDbType.Int, entidad.CodCuentaCorriente),
            SqlParameterFactory.CreateOutput("@pcod_cuenta_corriente_cuota", SqlDbType.Int),
            SqlParameterFactory.CreateVarchar("@pcod_compania", 50, entidad.CodCompania),
            SqlParameterFactory.CreateVarchar("@pcod_nivel", 50, entidad.CodNivel),
            SqlParameterFactory.CreateVarchar("@pcod_campus", 50, entidad.CodCampus),
            SqlParameterFactory.CreateVarchar("@pcod_departamento", 50, entidad.CodDepartamento),
            SqlParameterFactory.CreateVarchar("@pcod_periodo_academico", 50, entidad.CodPeriodoAcademico),
            SqlParameterFactory.CreateVarchar("@pcod_carrera", 50, entidad.CodCarrera),
            SqlParameterFactory.CreateVarchar("@pcod_facultad", 50, entidad.CodFacultad),
            SqlParameterFactory.CreateVarchar("@pcod_alumno", 50, entidad.CodAlumno),
            SqlParameterFactory.CreateNullableVarchar("@pcod_colegio", 50, entidad.CodColegio),
            SqlParameterFactory.CreateVarchar("@ptipo_alumno", 2, entidad.TipoAlumno),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis1", 50, entidad.IdSis1),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis2", 50, entidad.IdSis2),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis3", 50, entidad.IdSis3),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis4", 50, entidad.IdSis4),
            SqlParameterFactory.CreateNullableVarchar("@pid_sis5", 50, entidad.IdSis5),
            SqlParameterFactory.CreateNullableInt("@pcod_beneficio", entidad.CodBeneficio),
            SqlParameterFactory.CreateVarchar("@pdatos_personales_alumno", 250, entidad.DatosPersonalesAlumno),
            SqlParameterFactory.CreateVarchar("@pdescripcion_carrera", 250, entidad.DescripcionCarrera),
            SqlParameterFactory.CreateVarchar("@pdescripcion_departamento", 250, entidad.DescripcionDepartamento),
            SqlParameterFactory.CreateVarchar("@pdescripcion_campus", 250, entidad.DescripcionCampus),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis1", 250, entidad.DescripcionIdSis1),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis2", 250, entidad.DescripcionIdSis2),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis3", 250, entidad.DescripcionIdSis3),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis4", 250, entidad.DescripcionIdSis4),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_idsis5", 250, entidad.DescripcionIdSis5),
            SqlParameterFactory.CreateBigInt("@pcod_persona", entidad.CodPersona),
            SqlParameterFactory.CreateStructured("@otrosconceptos", "ctacte.TTOtrosCargos", otrosCargosTable),
            SqlParameterFactory.CreateDateTime("@pfecha_inicio", entidad.FechaInicioClases.Date),
            SqlParameterFactory.CreateVarchar("@pusuario", 50, Constants.CodUsuario),
            SqlParameterFactory.CreateChar("@ptipo_operacion", 1, 'U'),
            SqlParameterFactory.CreateNullableInt("@pcantidad_creditos", entidad.CreditosMatriculados?.CantidadCreditos),
            SqlParameterFactory.CreateNullableInt("@pcantidad_cursos", entidad.CreditosMatriculados?.CantidadCursos),
            SqlParameterFactory.CreateNullableVarchar("@pid_modulo", 50, entidad.CreditosMatriculados?.IdModulo),
            SqlParameterFactory.CreateNullableVarchar("@pcod_empresa_parametro", 50, entidad.CreditosMatriculados?.CodEmpresa),
            SqlParameterFactory.CreateNullableVarchar("@pdescripcion_empresa_parametro", 250, entidad.CreditosMatriculados?.DatosEmpresa),
            SqlParameterFactory.CreateNullableInt("@pcod_tasa_cuotas_parametro", entidad.CreditosMatriculados?.CodTasaCuotas),
            SqlParameterFactory.CreateOutput("@pestado", SqlDbType.Char, 1),
            SqlParameterFactory.CreateOutput("@presultado", SqlDbType.VarChar, 500)
        ];
    }

    public async Task<ActualizacionMasivoView> UpdMasivoAsync(ActualizacionMasivo actualizacionMasivo, CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);


        await using var command = connection.CreateCommand();
        command.CommandText = ActualizaMasivaStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@PCOD_COMPANIA", actualizacionMasivo.codCompania));
        command.Parameters.Add(new SqlParameter("@PCOD_NIVEL", actualizacionMasivo.codigoNivel));
        command.Parameters.Add(new SqlParameter("@PCOD_CARRERA", actualizacionMasivo.codigoCarrera));
        command.Parameters.Add(new SqlParameter("@PCOD_PERIODO_ACADEMICO", actualizacionMasivo.codigoPeriodo));
        command.Parameters.Add(new SqlParameter("@PFECHA_INICIO", actualizacionMasivo.fechaInicio));
        command.Parameters.Add(new SqlParameter("@PDIAS_POSTERGACION", actualizacionMasivo.diasPostergacion));
        command.Parameters.Add(new SqlParameter("@PNUMERO_CUOTA_DESDE", actualizacionMasivo.cuotaDesde));
        command.Parameters.Add(new SqlParameter("@PUSUARIO", actualizacionMasivo.Usuario));
         
        var pCodEjecucion = new SqlParameter("@PCOD_EJECUCION", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pCodEjecucion);

        var pEstado = new SqlParameter("@PESTADO", SqlDbType.Char, 1)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pEstado);
        var pResultado = new SqlParameter("@PRESULTADO", SqlDbType.VarChar, 500)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(pResultado);
        await command.ExecuteNonQueryAsync(cancellationToken);
        return new ActualizacionMasivoView(
            pCodEjecucion.Value != DBNull.Value? Convert.ToInt32(pCodEjecucion.Value): 0,
            pEstado.Value?.ToString() ?? "0",
            pResultado.Value?.ToString() ?? string.Empty
        );
    }

    public async Task<ActualizacionMasivoViewPagedResult> GetMasivoAsync(int page, int limit, string codCompania, string? estado, int? codEjecucion, CancellationToken cancellationToken = default)
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = ListaMasivaStoredProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@PCOD_COMPANIA", codCompania));
        command.Parameters.Add(new SqlParameter("@Pestado_log_ejecucion", estado));
        command.Parameters.Add(new SqlParameter("@PCOD_EJECUCION", codEjecucion));
        command.Parameters.Add(new SqlParameter("@Page", page));
        command.Parameters.Add(new SqlParameter("@Limit", limit));
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var items = new List<ActualizacionMasivoItem>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new ActualizacionMasivoItem(
                 ConvertDbHelper.ToString(reader["codAlumno"]),
                 ConvertDbHelper.ToString(reader["nombreAlumno"]),
                 ConvertDbHelper.ToString(reader["codCampus"]),
                 ConvertDbHelper.ToString(reader["nombreCampus"]),
                 ConvertDbHelper.ToString(reader["codFacultad"]),
                 ConvertDbHelper.ToString(reader["codCarrera"]),
                 ConvertDbHelper.ToString(reader["nombreCarrera"]),
                 ConvertDbHelper.ToString(reader["codDepartamento"]),
                 ConvertDbHelper.ToString(reader["nombreDepartamento"]),
                 ConvertDbHelper.ToString(reader["estado"]),
                 ConvertDbHelper.ToString(reader["nombreIncidente"])
            ));
        }

        var totalRegistros = 0;
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false)
            && await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            totalRegistros = reader.GetInt32(reader.GetOrdinal("TotalRegistros"));
        }

        return new ActualizacionMasivoViewPagedResult(
            items,
            totalRegistros,
            page,
            limit);
    }
    #endregion
}
