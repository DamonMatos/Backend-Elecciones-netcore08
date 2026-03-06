using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs
{
    public record PageDTO
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
    public class PageValidator : AbstractValidator<PageDTO>
    {
        public PageValidator()
        {
            RuleFor(request => request.Page)
                .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a cero.");

            RuleFor(request => request.Limit)
                .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor a cero.");
        }
    }
}
