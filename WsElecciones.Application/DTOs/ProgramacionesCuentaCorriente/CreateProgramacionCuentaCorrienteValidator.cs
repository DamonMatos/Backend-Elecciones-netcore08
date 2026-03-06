using FluentValidation;

namespace WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;

public sealed class CreateProgramacionCuentaCorrienteValidator
    : AbstractValidator<CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO>
{
    public CreateProgramacionCuentaCorrienteValidator()
    {
        RuleFor(x => x.CodNivel)
            .NotEmpty()
            .WithMessage("El nivel es obligatorio");

        RuleFor(x => x.CodPeriodoAcademicoCtaCte)
            .NotEmpty()
            .WithMessage("El Periodo academico es obligatorio");
        
        RuleFor(x => x.CodCompania)
            .NotEmpty()
            .WithMessage("La Compania es obligatorio");

        RuleFor(x => x.CodOperacionProgramacionCtaCte)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El código de operación programada debe ser mayor o igual a 1.");

        RuleForEach(x => x.CodDepartamentos)
            .NotEmpty()
            .WithMessage("Todos los códigos de departamento deben ser válidos.");

        RuleForEach(x => x.CodCampus)
            .NotEmpty()
            .WithMessage("Todos los códigos de campus deben ser válidos.");

        RuleForEach(x => x.FacultadCarreras)
            .ChildRules(facultadCarrera =>
            {
                facultadCarrera.RuleFor(x => x.CodFacultad)
                    .NotEmpty()
                    .WithMessage("Todos los códigos de facultad deben ser válidos.");

                facultadCarrera.RuleFor(x => x.CodCarrera)
                    .Must(codCarrera => codCarrera is null || !string.IsNullOrWhiteSpace(codCarrera))
                    .WithMessage("Todos los códigos de carrera deben ser válidos cuando se proporcionan.");
            });

        RuleForEach(x => x.CodAlumnos)
            .NotEmpty()
            .WithMessage("Todos los códigos de alumno deben ser válidos.");
        RuleForEach(x => x.CodAlumnosExcluir)
            .NotEmpty()
            .WithMessage("Todos los códigos de alumno deben ser válidos.");
    }
}
