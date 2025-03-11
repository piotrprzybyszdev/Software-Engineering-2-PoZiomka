using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace PoZiomkaDomain;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<ValidationFailure> validationFailures = [];

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);

            validationFailures.AddRange(result.Errors);
        }

        if (validationFailures.Count != 0)
            throw new ValidationException(validationFailures.First().ErrorMessage, validationFailures);

        return await next();
    }
}
