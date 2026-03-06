using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class AnularPagoAsbancValidator : AbstractValidator<AnularPagoAsbancDTO.AnularPagoAsbancRequestDTO>
    {
        public AnularPagoAsbancValidator()
        {
            RuleFor(x => x.FechaTransaccion)
                .NotEmpty().WithMessage("La fecha de transacción es obligatoria.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de transacción no puede ser futura.");

            RuleFor(x => x.CodigoBanco)
                .NotEmpty().WithMessage("El código del banco es obligatorio.")
                .Length(1, 5).WithMessage("El código del banco debe tener máximo 5 caracteres.");

            RuleFor(x => x.NumOperacionBanco)
                .NotEmpty().WithMessage("El número de operación del banco es obligatorio.")
                .Length(1, 13).WithMessage("El número de operación debe tener máximo 13 caracteres.");

            RuleFor(x => x.TipoConsulta)
                .NotEmpty().WithMessage("El tipo de consulta es obligatorio.")
                .Length(1).WithMessage("El tipo de consulta debe tener exactamente 1 carácter.");

            RuleFor(x => x.IdConsulta)
                .NotEmpty().WithMessage("El ID de consulta es obligatorio.")
                .Length(1, 15).WithMessage("El ID de consulta debe tener máximo 15 caracteres.");

            RuleFor(x => x.NumOperacionBancoERP)
                .NotEmpty().WithMessage("El número de operación del banco ERP es obligatorio.")
                .MaximumLength(30).WithMessage("El número de operación del banco ERP no debe exceder los 30 caracteres.");
        }
    }
}
