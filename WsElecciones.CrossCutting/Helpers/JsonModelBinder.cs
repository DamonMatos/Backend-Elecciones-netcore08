using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WsElecciones.CrossCutting.Helpers
{
    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize(value, bindingContext.ModelType, options);

                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (JsonException)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Formato JSON inválido para Procesos.");
            }

            return Task.CompletedTask;
        }
    }
}