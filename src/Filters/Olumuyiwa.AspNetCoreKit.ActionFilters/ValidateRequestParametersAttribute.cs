using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Olumuyiwa.AspNetCoreKit.ActionFilters
{
    /// <summary>
    /// An action filter attribute that validates the parameters of an action method using FluentValidation.
    /// </summary>
    /// <remarks>This attribute checks each action parameter for null values and validates them using the
    /// appropriate <see cref="IValidator{T}"/> implementation registered in the dependency injection container. If
    /// validation fails, the request is short-circuited, and a <see cref="BadRequestObjectResult"/> is returned with
    /// detailed validation error information.</remarks>
    public class ValidateRequestParametersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                {
                    context.Result = new BadRequestObjectResult("No parameters provided.");
                    return;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

                if (serviceProvider.GetService(validatorType) is IValidator validator)
                {
                    ValidationResult validationResult = validator.Validate(new ValidationContext<object>(argument));

                    if (!validationResult.IsValid)
                    {
                        Dictionary<string, string[]> validationFailures = [];
                        foreach (ValidationFailure failure in validationResult.Errors)
                        {
                            if (validationFailures.TryGetValue(failure.PropertyName, out string[]? value))
                                value.ToList().Add(failure.ErrorMessage);
                            else
                                validationFailures[failure.PropertyName] = [failure.ErrorMessage];
                        }

                        var validationProblemDetails = new ProblemDetails()
                        {
                            Title = "Bad Request",
                            Status = (int)HttpStatusCode.BadRequest,
                            Instance = context.HttpContext.Request.Path,
                            Detail = "One or more validation errors occurred.",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        };

                        validationProblemDetails.Extensions.Add("errors", validationFailures);

                        context.Result = new BadRequestObjectResult(validationProblemDetails);
                        return;
                    }
                }
            }


            base.OnActionExecuting(context);
        }
    }
}
