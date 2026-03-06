using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public sealed class CreatePagoAsbancValidator : AbstractValidator<CreatePagoAsbancDTO.CreatePagoAsbancRequestDTO>
    {
        public CreatePagoAsbancValidator()
        {
            RuleFor(x => x.FechaTransaccion)
                .NotEmpty().WithMessage("La fecha de transacción es obligatoria.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de transacción no puede ser futura.");

            RuleFor(x => x.CanalPago)
                .NotEmpty().WithMessage("El canal de pago es obligatorio.")
                .Length(1, 3).WithMessage("El canal de pago debe tener máximo 3 caracteres.");

            RuleFor(x => x.CodigoBanco)
                .NotEmpty().WithMessage("El código del banco es obligatorio.")
                .Length(1, 5).WithMessage("El código del banco debe tener máximo 5 caracteres.");

            RuleFor(x => x.NumOperacionBanco)
                .NotEmpty().WithMessage("El número de operación del banco es obligatorio.")
                .Length(1, 13).WithMessage("El número de operación debe tener máximo 13 caracteres.");

            RuleFor(x => x.FormaPago)
                .NotEmpty().WithMessage("La forma de pago es obligatoria.")
                .Length(1, 3).WithMessage("La forma de pago debe tener máximo 3 caracteres.");

            RuleFor(x => x.TipoConsulta)
                .NotEmpty().WithMessage("El tipo de consulta es obligatorio.")
                .Length(1).WithMessage("El tipo de consulta debe tener exactamente 1 carácter.");

            RuleFor(x => x.IdConsulta)
                .NotEmpty().WithMessage("El ID de consulta es obligatorio.")
                .Length(1, 15).WithMessage("El ID de consulta debe tener máximo 15 caracteres.");

            RuleFor(x => x.CodigoProducto)
                .NotEmpty().WithMessage("El código de producto es obligatorio.")
                .Length(1, 3).WithMessage("El código de producto debe tener máximo 3 caracteres.");

            RuleFor(x => x.NumDocumento)
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .Length(1, 16).WithMessage("El número de documento debe tener máximo 16 caracteres.");

            RuleFor(x => x.ImportePagado)
                .GreaterThan(0).WithMessage("El importe pagado debe ser mayor que cero.");

            RuleFor(x => x.MonedaDoc)
                .NotEmpty().WithMessage("La moneda del documento es obligatoria.")
                .Length(1, 2).WithMessage("La moneda debe tener máximo 2 caracteres.");
        }
    }
}
