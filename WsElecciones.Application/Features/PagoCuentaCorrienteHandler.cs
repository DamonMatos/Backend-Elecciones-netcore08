using WsElecciones.Application.DTOs.Pago;
using WsElecciones.Application.DTOs.PagoAsbanc;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Views;
using MapsterMapper;

namespace WsElecciones.Application.Features
{
    public class PagoCuentaCorrienteHandler
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PagoCuentaCorrienteHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<CreatePagoAsbancDTO.PagoAsbancDTO>> CrearPagoAsbancAsync(CreatePagoAsbancDTO.CreatePagoAsbancRequestDTO request)
        {
            int numOperacionERP = await _unitOfWork.PagoAsbancRepository.Add(_mapper.Map<PagoAsbanc>(request));
            var pagoAsbancDTO = new CreatePagoAsbancDTO.PagoAsbancDTO(numOperacionERP, string.Empty);
            return Response<CreatePagoAsbancDTO.PagoAsbancDTO>.Ok(pagoAsbancDTO);
        }
        public async Task<Response<AnularPagoAsbancDTO.PagoAsbancAnuladoDTO>> AnularPagoAsbancAsync(AnularPagoAsbancDTO.AnularPagoAsbancRequestDTO request)
        {
            int numOperacionERP = await _unitOfWork.PagoAsbancRepository.Add(
                _mapper.Map<PagoAsbanc>(request)
            );
            var pagoAsbancDTO = new AnularPagoAsbancDTO.PagoAsbancAnuladoDTO(numOperacionERP, string.Empty);
            return Response<AnularPagoAsbancDTO.PagoAsbancAnuladoDTO>.Ok(pagoAsbancDTO);
        }
        public async Task<Response<RevertirPagoAsbancDTO.PagoRevertidoDTO>> RevertirPagoAsbancAsync(RevertirPagoAsbancDTO.RevertirPagoAsbancRequestDTO request)
        {
            int resultado = await _unitOfWork.PagoAsbancRepository.RevertirPagoAsync(request.CodDocumento, request.Usuario);
            var pagoRevertidoDTO = new RevertirPagoAsbancDTO.PagoRevertidoDTO(resultado);
            return Response<RevertirPagoAsbancDTO.PagoRevertidoDTO>.Ok(pagoRevertidoDTO);
        }
        public async Task<Response<GetDeudaCuentaCorrienteDTO.CuentaCorrienteDetalleDTO[]>> GetConsultaDeudaAsync(GetDeudaCuentaCorrienteDTO.CuentaCorrienteRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PagoAsbancRepository
                                        .GetConsultaDeudaAsync(request.TipoDocumento, string.Empty, request.CodigoProducto, cancellationToken)
                                        .ConfigureAwait(false);

            var items = result.Items
                .Select(item => new GetDeudaCuentaCorrienteDTO.CuentaCorrienteDetalleDTO(
                        item.NumDocumento,
                        item.CodigoProducto,
                        item.DescDocumento,
                        item.FechaVencimiento,
                        item.FechaEmision,
                        item.Deuda,
                        item.Mora,
                        item.GastosAdm,
                        item.PagoMinimo,
                        item.Periodo,
                        item.Anio,
                        item.Cuota,
                        item.MonedaDoc))
                .ToArray();
            return Response<GetDeudaCuentaCorrienteDTO.CuentaCorrienteDetalleDTO[]>.Ok(items);
        }

        public async Task<Response<IEnumerable<GetPagoDTO.PagoDto>>> GetPagoAsync(int codDocumento, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PagoRepository
                .GetPagoAsync(
                    codDocumento, cancellationToken
                    )
                .ConfigureAwait(false);

            var items = result.Select(item => new GetPagoDTO.PagoDto(
                   //item.CodigoCancelacion,
                   item.CodigoMedioCancelacion,
                   item.MedioPago,
                   item.CodigoMoneda,
                   item.Moneda,
                   item.CodigoBanco,
                   item.Importe,
                   item.FechaCancelacion,
                   item.EsTotal,
                   item.NumeroOperacionBancaria,
                   item.CodCuentaBancarioContinental,
                   item.Banco,
                   item.CanalPago,
                   item.NumeroDocumento,
                   item.Serie,
                   item.CodigoDocumento
                    ));
            return Response<IEnumerable<GetPagoDTO.PagoDto>>.Ok(items);
        }

    }
}