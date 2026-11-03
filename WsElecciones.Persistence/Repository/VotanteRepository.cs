using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.CrossCutting.Helpers;
using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Elecciones;
using WsElecciones.Domain.Views.Votante;
using WsElecciones.Persistence.SqlHelpers;
using static WsElecciones.Domain.Views.Colaboradores.GetColaboradoresView;
using static WsElecciones.Domain.Views.Votante.GetVotanteView;

namespace WsElecciones.Persistence.Repository
{
    public class VotanteRepository(IStoredProcedureExecutor spExecutor) : IVotanteRepository
    {
        public const string Sp_GetByIdVotantes = "Usp_GetVotante_Catalogo";

        public async Task<GetVotanteView.VotantePageResult> GetColaboradoresAsync(int IdUsuario, CancellationToken cancellationToken = default)
        {
            var parameters = new[]
            {
                 SqlParameterFactory.CreateInt("@IdUsuario", IdUsuario),
            };

            var mappers = new List<Func<IDataRecord, object>>
            {
                reader => new GetVotanteView.ProcesoItem(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToString(reader["NombreEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdProceso"]),
                    ConvertDbHelper.ToString(reader["NombreProceso"]),
                    ConvertDbHelper.ToString(reader["Tipodocumento"]),
                    ConvertDbHelper.ToString(reader["NumeroDocumento"]),
                    ConvertDbHelper.ToDateTime(reader["FechaFin"])
                ),

                reader => new GetVotanteView.CandidatoItem(
                    ConvertDbHelper.ToInt32(reader["IdEleccion"]),
                    ConvertDbHelper.ToInt32(reader["IdProceso"]),
                    ConvertDbHelper.ToInt32(reader["IdCandidato"]),
                    ConvertDbHelper.ToString(reader["NombreCompleto"]),
                    ConvertDbHelper.ToString(reader["Area"]),
                    ConvertDbHelper.ToString(reader["UrlFile"])
                ),
            };

            var result = await spExecutor.ExecuteMultipleReaderAsync(
                Sp_GetByIdVotantes,
                mappers,
                parameters,
                cancellationToken
                );

            var procesos   = result[0].Cast<GetVotanteView.ProcesoItem>().ToList().AsReadOnly();
            var candidatos = result[1].Cast<GetVotanteView.CandidatoItem>().ToList().AsReadOnly();

            return new VotantePageResult(
                Procesos: procesos,
                Candidatos: candidatos
            );
        }
    }
}
