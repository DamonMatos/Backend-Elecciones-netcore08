using MapsterMapper;
using Microsoft.Extensions.Options;
using System.Reflection;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Candidatos;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Candidatos;
using static WsElecciones.Application.DTOs.Candidatos.GetCandidatosDTO;

namespace WsElecciones.Application.Features
{
    public class CandidatoHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorage, IOptions<FileStorageConfig> storageOptions)
    {
        public async Task<Response<GetCandidatosDTO.CandidatoPagedResponse>> GetCandidatoAsync(int page,int limit,int idEleccion,int idProceso, CancellationToken cancellationToken = default) {
            var result = await unitOfWork.CandidatoRepository.GetCandidatosAsync(
                page,
                limit,
                idEleccion,
                idProceso,
                cancellationToken)
                .ConfigureAwait(false);

            var storage = storageOptions.Value.Modules["Elecciones"];
            var baseUrl = storage.BaseUrl;

            var items = result.Items
           .Select(item => new GetCandidatosDTO.CandidatoItemDto(
               item.IdEleccion,
               item.IdProceso,
               item.IdCandidato,
               item.TipoDocumento,
               item.NumeroDocumento,
               item.NombreCompleto,
               item.Area,
               item.Localidad,
               item.UrlFile,
               item.Estado,
               item.Descripcion
           ) with
           {
               FotoPreview = string.IsNullOrWhiteSpace(item.UrlFile)
                   ? string.Empty
                   : $"{baseUrl}/{item.IdEleccion}/{item.IdProceso}/{item.UrlFile}"
           }).ToArray();

            //var items = result.Items
            //.Select(item => new GetCandidatosDTO.CandidatoItemDto(
            //    item.IdEleccion,
            //    item.IdProceso,
            //    item.IdCandidato,
            //    item.TipoDocumento,
            //    item.NumeroDocumento,
            //    item.NombreCompleto,
            //    item.Area,
            //    item.Localidad,
            //    item.UrlFile,
            //    item.Estado,
            //    item.Descripcion
            //    )).ToArray();

            var responseData = new GetCandidatosDTO.CandidatoPagedResponse(
            items,
            result.TotalRegistros,
            result.Page,
            result.Limit);

            return Response<GetCandidatosDTO.CandidatoPagedResponse>.Ok(responseData);
        }

        //private static string BuildFileUrl(string baseUrl, int idEleccion, int idProceso, string urlFile)
        //{
        //    if (string.IsNullOrWhiteSpace(urlFile))
        //        return string.Empty;

        //    return $"{baseUrl}/{idEleccion}/{idProceso}/{urlFile.TrimStart('/')}";
        //}

        public async Task<Response<ResponseDTO>> AddCandidatoAsync(CreateCandidatoDTO request, CancellationToken cancellationToken= default)
        {
            string modulo = string.Empty;
            string filename   = string.Empty;
            string foldername = string.Empty;   
            string foto = string.Empty; 
            int accion = request.Accion;

            var result = await unitOfWork.CandidatoRepository.
                CreateCandidatosAsync(accion, mapper.Map<GetCandidatosView.CandidatoItem>(request), cancellationToken);

            if (result.Estado == 0)
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al guardar la colaborador.", Array.Empty<string>());

            //Logica para guardar la imagen
            if (accion != 3) {
                foto = request.Foto.FileName;
                filename = request.UrlFile;
                if (request.UrlFile is not null) {
                    var extension = Path.GetExtension(foto).ToLowerInvariant();
                    if (extension != ".jpg")
                    {
                        return Response<ResponseDTO>.Failure("Solo se permiten archivos .JPG", Array.Empty<string>());
                    }
                    modulo = "Elecciones";
                    foldername = string.Format("{0}/{1}", request.IdEleccion, request.IdProceso);
                    fileStorage.SaveAsync(modulo,foldername,filename, request.Foto, cancellationToken);
                }
            }

            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);

        }
    
    }
}
