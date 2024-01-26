using EazyScenes.Core.Exchange;
using FluentValidation;
using MediatR;

namespace EazyScenes.Core;

// Pipeline Behaviour for validating commands using Fluent Validation with every mediatr command handler
public class FluentValidationBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    #region Fields

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    #endregion

    #region Construction

    public FluentValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    #endregion

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
                               .Select(validator => validator.Validate(context))
                               .Where(validationResult => validationResult.Errors.Count != 0)
                               .SelectMany(validationResult => validationResult.Errors)
                               .Select(validationFailure => new Error(validationFailure.ErrorCode, validationFailure.ErrorMessage))
                               .ToList();

        if (validationErrors.Count != 0)
        {
            throw new ValidationException(validationErrors);
        }

        return await next();
    }

    #endregion
}
