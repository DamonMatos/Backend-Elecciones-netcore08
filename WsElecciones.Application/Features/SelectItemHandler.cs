using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class SelectItemHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        public async Task<Response<IEnumerable<SelectItemDto>>> GetByIdAsync(int Tipo,int IdCliente, CancellationToken cancellationToken = default) { 

            var data = await unitOfWork.SelectItemRepository.
                GetByIdAsync(Tipo,IdCliente, cancellationToken).
                ConfigureAwait(false);

            if (data.Count == 0)
                return Response<IEnumerable<SelectItemDto>>.Failure("No se encontraron datos", Array.Empty<string>());

            var result = mapper.Map<IReadOnlyCollection<SelectItemDto>>(data);

            return Response< IEnumerable<SelectItemDto>>.Ok(result);

        }
    }
}
