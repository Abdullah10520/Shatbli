using FluentValidation;
using MediatR;
using Shatabli.Core.Domain.Common;


namespace Shatabli.Core.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        /// This field will hold all validators that are registered for the current TRequest.
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// The constructor uses Dependency Injection to get all registered validators.
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            /// ex: This could be just [AddCourseValidator] or [AddCourseValidator, AddCourseDbValidator]
            _validators = validators;
        }

        /// This is the core method of the pipeline behavior.
        /// 'next' is a delegate that calls the *next* item in the pipeline (or the handler itself).
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            /// 'request' is the command or query object, e.g., AddCourseCommand

            /// If there are no validators registered for this specific request,
            /// skip all the logic and pass it straight to the handler.
            if (!_validators.Any())
            {
                return await next();
            }

            /// The ValidationContext is essential. It's not just the data (request),
            /// it's a "container" that also carries metadata (the "how") for the validation.
            ///
            /// Key features it provides:
            ///
            /// 1. PropertyChain: This is crucial. It tracks the full path of nested properties 
            ///    (e.g., "Customer.Address.Street"). This allows the validator to know exactly
            ///    which nested property failed, which is impossible if you only pass the raw 'request'.
            ///
            /// 2. RuleSetToValidate: This allows a single validator class to contain multiple
            ///    sets of rules (e.g., different rules for "Create" vs. "Update"). The Context
            ///    is what tells the validator, "Please run only the 'Create' rule set this time."
            ///
            /// 3. Validation Strategy (e.g., ThrowOnFirstFailure): The Context carries the
            ///    strategy for this specific run. Should it stop on the first error, or should
            ///    it continue and collect all errors?
            ///
            /// 4. Extensibility (Summary): Using this container allows the library to add
            ///    future features and pass more metadata without ever changing the
            ///    IValidator.ValidateAsync method signature, which prevents breaking existing code.
            var context = new ValidationContext<TRequest>(request);

            /// 1. .Select(...) does a "preparation loop" and creates a list of "Tasks", not results.
            ///    The output is: [ Task_For_Validator1, Task_For_Validator2 ]
            ///
            /// 2. Task_For_Validator1 (the first task) knows it must run Validator1's rules (e.g., RuleFor(c=>c.Id))
            ///
            /// 3. Task_For_Validator2 (the second task) knows it must run Validator2's rules (e.g., RuleFor(c=>c.Name))
            ///
            /// 4. Task.WhenAll(...) takes this list of "Tasks" and gives them the "start" signal to run all at once (in parallel).
            ///
            /// 5. `await` waits until "all Tasks" are finished and have returned their results.
            ///
            /// 6. After they finish, Task_For_Validator1 returns Result_From_Validator1 (which has Errors = [ "Error A1" ])
            ///    and Task_For_Validator2 returns Result_From_Validator2 (which has Errors = [ "Error B1" ])
            ///
            /// 7. `validationResults` collects all these "Results" into an array.
            ///    validationResults = [ Result_From_Validator1, Result_From_Validator2 ]
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                // ✅ Group errors by PropertyName
                var errorsDictionary = failures
                    .GroupBy(f => ToCamelCase(f.PropertyName))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToList()
                    );

                // ✅ Check if TResponse is Result<T>
                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var failureMethod = typeof(TResponse).GetMethod(
                        "Failure",
                        new[] { typeof(string), typeof(int), typeof(Dictionary<string, List<string>>) }
                    );

                    if (failureMethod != null)
                    {
                        var result = failureMethod.Invoke(
                            null,
                            new object[] { "Validation failed", 400, errorsDictionary }
                        );
                        return (TResponse)result!;
                    }
                }
                // ✅ Check if TResponse is Result (non-generic)
                else if (typeof(TResponse) == typeof(Result))
                {
                    var result = Result.Failure("Validation failed", 400, errorsDictionary);
                    return (TResponse)(object)result;
                }

                // ❌ Fallback: If it's not Result type, throw exception
                throw new ValidationException(failures);
            }

            return await next();
        }

        private static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
