using FluentValidation;
using WsElecciones.Application.DTOs.Elector;

public sealed class CreateElectorValidator : AbstractValidator<CreateElectorDTO>
{
    public CreateElectorValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El IdUsuario debe ser mayor a 0.");

        RuleFor(x => x.IdEleccion)
            .GreaterThan(0)
            .WithMessage("El IdEleccion debe ser mayor a 0.");

        RuleFor(x => x.IdProceso)
            .GreaterThan(0)
            .WithMessage("El IdProceso debe ser mayor a 0.");

        // Validaciones de Candidatos (opcionales / nulos)
        //RuleFor(x => x.IdCandidato1)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato1.HasValue)
        //    .WithMessage("El IdCandidato1 debe ser mayor a 0.");

        //RuleFor(x => x.IdCandidato2)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato2.HasValue)
        //    .WithMessage("El IdCandidato2 debe ser mayor a 0.");

        //RuleFor(x => x.IdCandidato3)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato3.HasValue)
        //    .WithMessage("El IdCandidato3 debe ser mayor a 0.");

        //RuleFor(x => x.IdCandidato4)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato4.HasValue)
        //    .WithMessage("El IdCandidato4 debe ser mayor a 0.");

        //RuleFor(x => x.IdCandidato5)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato5.HasValue)
        //    .WithMessage("El IdCandidato5 debe ser mayor a 0.");

        //RuleFor(x => x.IdCandidato6)
        //    .GreaterThan(0)
        //    .When(x => x.IdCandidato6.HasValue)
        //    .WithMessage("El IdCandidato6 debe ser mayor a 0.");
    }
}