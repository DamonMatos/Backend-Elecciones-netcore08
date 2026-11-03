using WsElecciones.Application.DTOs.Auth;
using FluentValidation;

namespace WsElecciones.Application.Validator.Auth
{
    public class UpdateRequestDTOValidator : AbstractValidator<UpdateRequestDTO>
    {
        public UpdateRequestDTOValidator()
        {
            RuleFor(x => x.IdUsuario)
                .GreaterThan(0)
                .WithMessage("El IdUsuario es obligatorio.");

            RuleFor(x => x.IdPersonal)
                .GreaterThan(0)
                .WithMessage("El IdPersonal es obligatorio.");

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El Nombre es obligatorio.")
                .MaximumLength(100)
                .WithMessage("El Nombre no debe superar 100 caracteres.")
                .When(x => x.Nombre != null);

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty()
                .WithMessage("El Apellido Paterno es obligatorio.")
                .MaximumLength(100)
                .WithMessage("El Apellido Paterno no debe superar 100 caracteres.")
                .When(x => x.ApellidoPaterno != null);

            RuleFor(x => x.ApellidoMaterno)
                .MaximumLength(100)
                .WithMessage("El Apellido Materno no debe superar 100 caracteres.")
                .When(x => x.ApellidoMaterno != null);

            RuleFor(x => x.FechaNacimiento)
                .LessThan(DateTime.Today)
                .WithMessage("La fecha de nacimiento no puede ser mayor a hoy.")
                .When(x => x.FechaNacimiento.HasValue);

            RuleFor(x => x.TipoDocumento)
                .MaximumLength(20)
                .WithMessage("El Tipo de Documento no debe superar 20 caracteres.")
                .When(x => x.TipoDocumento != null);

            RuleFor(x => x.NumeroDocumento)
                .MaximumLength(20)
                .WithMessage("El Número de Documento no debe superar 20 caracteres.")
                .When(x => x.NumeroDocumento != null);

            RuleFor(x => x.NombreFoto)
                .NotEmpty()
                .WithMessage("El nombre de la foto es obligatorio.")
                .MaximumLength(200)
                .WithMessage("El nombre de la foto no debe superar 200 caracteres.");

            RuleFor(x => x.NombreCliente)
                .MaximumLength(150)
                .WithMessage("El Nombre del Cliente no debe superar 150 caracteres.")
                .When(x => x.NombreCliente != null);

            RuleFor(x => x.RazonSocial)
                .NotEmpty()
                .WithMessage("La Razón Social es obligatoria.")
                .MaximumLength(200)
                .WithMessage("La Razón Social no debe superar 200 caracteres.");

            RuleFor(x => x.Ruc)
                .NotEmpty()
                .WithMessage("El RUC es obligatorio.")
                .Length(11)
                .WithMessage("El RUC debe tener exactamente 11 dígitos.")
                .Matches("^[0-9]*$")
                .WithMessage("El RUC solo debe contener dígitos numéricos.");

            RuleFor(x => x.Foto)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("La foto no puede estar vacía.")
                .Must(file => file == null || file.ContentType.StartsWith("image/"))
                .WithMessage("El archivo debe ser una imagen.")
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("La imagen no debe superar 5 MB.");
        }
    }
}


