namespace Shatabli.Core.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; protected set; }
        public T? Data { get; protected set; }
        public string Message { get; protected set; } = string.Empty;
        public Dictionary<string, List<string>> Errors { get; protected set; } = new Dictionary<string, List<string>>();
        public int StatusCode { get; protected set; } = 200;

        protected Result() { }

        public static Result<T> Success(T data, string message = "Operation completed successfully")
        {
            return new Result<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                Errors = new Dictionary<string, List<string>>(),
                StatusCode = 200
            };
        }

        // ✅ Main Failure method - Dictionary-based
        public static Result<T> Failure(string message, int statusCode, Dictionary<string, List<string>> errors)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Data = default,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }

        // ✅ Overload for simple list (converted to Dictionary with "general" key)
        public static Result<T> Failure(string message, int statusCode = 400, List<string>? errors = null)
        {
            var errorDict = new Dictionary<string, List<string>>();
            if (errors != null && errors.Any())
            {
                errorDict["general"] = errors;
            }

            return new Result<T>
            {
                IsSuccess = false,
                Data = default,
                Message = message,
                Errors = errorDict,
                StatusCode = statusCode
            };
        }

        // ✅ Overload for single error message
        //public static Result<T> Failure(string message, int statusCode, string error)
        //{
        //    return new Result<T>
        //    {
        //        IsSuccess = false,
        //        Data = default,
        //        Message = message,
        //        Errors = new Dictionary<string, List<string>>
        //        {
        //            ["general"] = new List<string> { error }
        //        },
        //        StatusCode = statusCode
        //    };
        //}

        // ✅ Overload for single field error
        //public static Result<T> Failure(string message, int statusCode, string fieldName, string error)
        //{
        //    return new Result<T>
        //    {
        //        IsSuccess = false,
        //        Data = default,
        //        Message = message,
        //        Errors = new Dictionary<string, List<string>>
        //        {
        //            [fieldName] = new List<string> { error }
        //        },
        //        StatusCode = statusCode
        //    };
        //}

        public static Result<T> NotFound(string message = "Resource not found")
        {
            return Failure(message, 404);
        }

        public static Result<T> Unauthorized(string message = "Unauthorized")
        {
            return Failure(message, 401);
        }

        public static Result<T> Conflict(string message = "Resource already exists")
        {
            return Failure(message, 409);
        }

        public static Result<T> Forbidden(string message = "Forbidden")
        {
            return Failure(message, 403);
        }
    }

    // For operations that don't return data
    public class Result : Result<object>
    {
        public static Result Success(string message = "Operation completed successfully")
        {
            return new Result
            {
                IsSuccess = true,
                Message = message,
                StatusCode = 200,
                Errors = new Dictionary<string, List<string>>()
            };
        }

        public new static Result Failure(string message, int statusCode, Dictionary<string, List<string>> errors)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }

        public new static Result Failure(string message, int statusCode = 400, List<string>? errors = null)
        {
            var errorDict = new Dictionary<string, List<string>>();
            if (errors != null && errors.Any())
            {
                errorDict["general"] = errors;
            }

            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = errorDict,
                StatusCode = statusCode
            };
        }

        public new static Result Failure(string message, int statusCode, string error)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = new Dictionary<string, List<string>>
                {
                    ["general"] = new List<string> { error }
                },
                StatusCode = statusCode
            };
        }

        public static Result Failure(string message, int statusCode, string fieldName, string error)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = new Dictionary<string, List<string>>
                {
                    [fieldName] = new List<string> { error }
                },
                StatusCode = statusCode
            };
        }

        public new static Result NotFound(string message = "Resource not found")
        {
            return Failure(message, 404);
        }

        public new static Result Unauthorized(string message = "Unauthorized")
        {
            return Failure(message, 401);
        }

        public new static Result Conflict(string message = "Resource already exists")
        {
            return Failure(message, 409);
        }

        public new static Result Forbidden(string message = "Forbidden")
        {
            return Failure(message, 403);
        }
    }
}