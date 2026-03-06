using FluentValidation;
using WsElecciones.Application.DTOs.Auth;

namespace WsElecciones.Api.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El usuario es requerido.")
                .MaximumLength(100);

            RuleFor(x => x.Clave)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
