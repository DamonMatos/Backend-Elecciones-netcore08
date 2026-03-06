using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class RevertirPagoAsbancValidator : AbstractValidator<RevertirPagoAsbancDTO.RevertirPagoAsbancRequestDTO>
    {
        public RevertirPagoAsbancValidator()
        {
            // codDocumento debe ser mayor a 0
            RuleFor(x => x.CodDocumento)
                .GreaterThan(0)
                .WithMessage("El código de documento debe ser mayor que 0.");

            // usuario no nulo, no vacío y longitud razonable
            RuleFor(x => x.Usuario)
                .NotEmpty()
                .WithMessage("El usuario es requerido.")
                .MaximumLength(100)
                .WithMessage("El usuario no puede tener más de 100 caracteres.");

            // (opcional) si quieres evitar solo espacios en blanco:
            RuleFor(x => x.Usuario)
                .Must(u => !string.IsNullOrWhiteSpace(u))
                .WithMessage("El usuario no puede ser solo espacios en blanco.");
        }
    }
}
