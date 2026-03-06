using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain;
using WsElecciones.Domain.Entitites;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.CuentaCorrienteCuotas;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.SqlHelpers;

namespace WsElecciones.Persistence.Repository
{
    public class CuentaCorrienteCuotaRepository(CuentaCorrienteContext context, IStoredProcedureExecutor spExecutor): Repository<CuentaCorrienteCuotum>(context), ICuentaCorrienteCuotaRepository
    {
        private const string SpListarCuotas = "ctacte.Sp_Sel_Cuenta_Corriente_Cuota_By_Cuenta_Corriente";
        private const string ConceptosPorCuentaCorrienteCuotaStoredProcedureName = "[ctacte].[Sp_Sel_Conceptos_By_Cuota_Cuenta_Corriente_Id]";
        public async Task<IReadOnlyCollection<GetCuotasCuentaCorrienteView>> GetCuotasByCodigoCuentaCorrienteAsync(int? codCuentaCorriente, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                SqlParameterFactory.CreateNullableInt("@CodCuentaCorriente", codCuentaCorriente),
                SqlParameterFactory.CreateNullableVarchar("@CodCompania", 50, Constants.CodCompania)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                SpListarCuotas,
                reader => new GetCuotasCuentaCorrienteView(
                    ConvertDbHelper.ToInt32(reader["CodCuentaCorrienteCuota"]),
                    ConvertDbHelper.ToInt32(reader["NumeroCuota"]),
                    ConvertDbHelper.ToString(reader["FechaPago"]),
                    ConvertDbHelper.ToString(reader["CodMoneda"]),
                    ConvertDbHelper.ToString(reader["CodMonedaInternacional"]),
                    ConvertDbHelper.ToDecimal(reader["Importe"]),
                    ConvertDbHelper.ToDecimal(reader["Impuesto"]),
                    ConvertDbHelper.ToDecimal(reader["TotalPagado"]),
                    ConvertDbHelper.ToDecimal(reader["BeneficiosTotales"]),
                    ConvertDbHelper.ToDecimal(reader["TotalAPagar"]),
                    ConvertDbHelper.ToDecimal(reader["Saldo"]),
                    ConvertDbHelper.ToString(reader["Vigente"]),
                    ConvertDbHelper.ToInt32(reader["TieneCancelacion"])
                ),
                parameters,
                cancellationToken);

            return result;
        }

        public async Task<IReadOnlyCollection<GetCuentaCorrienteConceptoResumenView>>GetConceptosByCuentaCorrienteCuotaIdAsync(
            string codCompania,
            int codCuentaCorrienteCuota,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(codCompania))
                throw new ArgumentException("El código de compañía es obligatorio.", nameof(codCompania));

            if (codCuentaCorrienteCuota <= 0)
                throw new ArgumentException("El código de la cuota de cuenta corriente es obligatorio.", nameof(codCuentaCorrienteCuota));

            var parameters = new[]
            {
                SqlParameterFactory.CreateVarchar("@CodCompania", 50, codCompania.Trim()),
                SqlParameterFactory.CreateInt("@CodCuentaCorrienteCuota", codCuentaCorrienteCuota)
            };

            var result = await spExecutor.ExecuteReaderAsync(
                ConceptosPorCuentaCorrienteCuotaStoredProcedureName,
                reader => new GetCuentaCorrienteConceptoResumenView(
                    ConvertDbHelper.ToString(reader["VIGENTE_CUENTA_CORRIENTE_CUOTA"]),
                    ConvertDbHelper.ToInt32Null(reader["COD_CONCEPTO"]),
                    ConvertDbHelper.ToString(reader["CONCEPTO"]),
                    ConvertDbHelper.ToString(reader["BENEFICIO"]),
                    ConvertDbHelper.ToInt32Null(reader["NUMERO_CUOTA"]),
                    ConvertDbHelper.ToDecimalNull(reader["IMPORTE_TOTAL"]),
                    ConvertDbHelper.ToDecimalNull(reader["IMPORTE_BENEFICIO"]),
                    ConvertDbHelper.ToDecimalNull(reader["IMPORTE_IMPUESTO"]),
                    ConvertDbHelper.ToDecimalNull(reader["TOTAL_A_PAGAR"]),
                    ConvertDbHelper.ToDecimalNull(reader["IMPORTE_CANCELADO"]),
                    ConvertDbHelper.ToDecimalNull(reader["SALDO"])
                ),
                parameters,
                cancellationToken);

            return result;
        }
    }
}