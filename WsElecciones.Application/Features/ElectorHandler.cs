using FluentValidation;
using MapsterMapper;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Elector;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Views.Elector;

namespace WsElecciones.Application.Features
{
    public class ElectorHandler
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateElectorDTO> _validator;

        public ElectorHandler(IMapper mapper, IUnitOfWork unitOfWork, IValidator<CreateElectorDTO> validatior)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator = validatior;
        }

        public async Task<Response<ResponseDTO>> CreateAsync(CreateElectorDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ElectorRepository.CreateElectorAsync(
                _mapper.Map<CreateElectorView>(request), cancellationToken);

            if (result.Estado == 0) 
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al generar tu voto.", Array.Empty<string>());
               
            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);
        }
    }
}
