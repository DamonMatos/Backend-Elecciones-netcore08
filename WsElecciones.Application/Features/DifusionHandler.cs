using Azure.Core;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Domain.Views.Usuario;

namespace WsElecciones.Application.Features
{
    public class DifusionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        public async Task<Response<ResponseDTO>> CreateAsync(int IdEleccion, CancellationToken cancellationToken = default)
        {
            var difusionresponse = await unitOfWork.EleccionesRepository.GenerarDifusion(IdEleccion, cancellationToken);
            if (difusionresponse.Estado == 0)
            {
                return Response<ResponseDTO>.Failure(difusionresponse.Mensaje ?? "Error al generar la difusión.", Array.Empty<string>());
            }

            var colaboradores = await unitOfWork.ColaboradorRepository.GetColaboradoresEnvioMasivoAsync(IdEleccion);

            if (colaboradores == null || !colaboradores.Any())
            {
                return Response<ResponseDTO>.Ok(new ResponseDTO(difusionresponse.Id, difusionresponse.Estado, "No hay colaboradores para procesar."));
            }

            var listaColaboradoresRequest = new List<UpdateUsuarioDTO.UpdateUsuarioItemRequest>();
            // Optimizamos el paralelismo según la capacidad del servidor
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(colaboradores, parallelOptions, async (colaborador, ct) =>
            {
                if (!string.IsNullOrWhiteSpace(colaborador.Clave))
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(colaborador.Clave, workFactor: 12);

                    lock (listaColaboradoresRequest)
                    {
                        listaColaboradoresRequest.Add(new UpdateUsuarioDTO.UpdateUsuarioItemRequest(
                            colaborador.Correo,
                            passwordHash
                        ));
                    }
                }
                await Task.CompletedTask;
            });

            var updatelistausuarios = new UpdateUsuarioDTO.UpdateUsuariosListRequest(listaColaboradoresRequest);

            await unitOfWork.ColaboradorRepository.UpdateUsuarioAsync(mapper.Map<UpdateUsuarioView.UpdateUsuariosListRequest>(updatelistausuarios), cancellationToken);

            var response = new ResponseDTO(difusionresponse.Id, difusionresponse.Estado, difusionresponse.Mensaje);
            return Response<ResponseDTO>.Ok(response);

        }
    }
}


