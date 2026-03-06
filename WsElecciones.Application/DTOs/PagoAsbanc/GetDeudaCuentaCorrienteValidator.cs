using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WsElecciones.Application.DTOs.PagoAsbanc.GetDeudaCuentaCorrienteDTO;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class GetDeudaCuentaCorrienteValidator : AbstractValidator<CuentaCorrienteRequestDTO>
    {
        public GetDeudaCuentaCorrienteValidator()
        {
            RuleFor(x => x.CodigoProducto)
                .NotEmpty().WithMessage("El campo 'CodigoProducto' es obligatorio.");

            RuleFor(x => x.TipoDocumento)
                .NotEmpty().WithMessage("El campo 'TipoDocumento' es obligatorio.");

            RuleFor(x => x.NumDocumento)
                .NotEmpty().WithMessage("El campo 'NumDocumento' es obligatorio.");

            RuleFor(x => x.CodigoBanco)
                .NotEmpty().WithMessage("El campo 'CodigoBanco' es obligatorio.");

            RuleFor(x => x.Origen)
                .NotEmpty().WithMessage("El campo 'Origen' es obligatorio.");
        }
    }
}
