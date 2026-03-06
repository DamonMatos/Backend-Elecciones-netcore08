using FluentValidation;
using WsElecciones.CrossCutting;

namespace WsElecciones.Api.Validations
{
    public sealed class ValidationFilter<TRequest, TResult> : IEndpointFilter
        where TRequest : class
    {
        private static readonly object _lock = new();
        private static bool _initialized;
        private static readonly Type _validatorInterfaceType = typeof(IValidator<>);
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            EnsureFluentValidationConfigured();

            var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
            if (validator is null)
                return await next(context);

            if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(TRequest)) is not TRequest entity)
                return BadRequest("Cuerpo de la solicitud no válido.");

            var validationResult = entity switch
            {
                IEnumerable<object> collection => await ValidateCollectionAsync(context, collection),
                _ => await ValidateSingleAsync(validator, entity)
            };

            return validationResult ?? await next(context);
        }
        private static void EnsureFluentValidationConfigured()
        {
            if (_initialized) return;
            lock (_lock)
            {
                if (_initialized) return;
                ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) => member?.Name;
                _initialized = true;
            }
        }
        private static async Task<IResult?> ValidateSingleAsync(IValidator<TRequest> validator, TRequest entity)
        {
            var result = await validator.ValidateAsync(entity);
            return result.IsValid ? null : BadRequest("Error en validaciones", result.Errors.Select(e => e.ErrorMessage));
        }
        private static async Task<IResult?> ValidateCollectionAsync(EndpointFilterInvocationContext context, IEnumerable<object> collection)
        {
            var elementType = typeof(TRequest).GetGenericArguments().FirstOrDefault();
            if (elementType is null)
                return BadRequest("No se pudo determinar el tipo de los elementos a validar.");

            var elementValidator = GetElementValidator(context, elementType);
            if (elementValidator is null)
                return BadRequest($"No se encontró un validador para el tipo {elementType.Name}.");

            var errors = await ValidateEachElementAsync(collection, elementValidator);
            return errors.Count > 0 ? BadRequest("Error en validaciones", errors) : null;
        }
        private static IValidator? GetElementValidator(EndpointFilterInvocationContext context, Type elementType)
        {
            var validatorType = _validatorInterfaceType.MakeGenericType(elementType);
            return context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
        }
        private static async Task<List<string>> ValidateEachElementAsync(IEnumerable<object> collection, IValidator validator)
        {
            var errors = new List<string>();
            int index = 0;

            foreach (var item in collection)
            {
                var validationContext = new ValidationContext<object>(item);
                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                    errors.AddRange(result.Errors.Select(e => $"Elemento[{index}]: {e.ErrorMessage}"));

                index++;
            }

            return errors;
        }
        private static IResult BadRequest(string message, IEnumerable<string>? errors = null)
        {
            var response = Response<TResult>.Failure(message, errors?.ToList() ?? []);
            return Results.BadRequest(response);
        }
    }
}