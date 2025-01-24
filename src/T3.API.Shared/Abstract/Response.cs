namespace T3.API.Shared.Abstract;

using Mapster;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using T3.Domain.Shared.Errors;

public abstract record Response
{
    private readonly List<Error> errors = new ();

    [JsonIgnore]
    public IEnumerable<Error> Errors => this.errors;

    protected Response()
    {
    }

    public static TResponse FromValidationErrors<TResponse>(IDictionary<string, string[]> errors)
        where TResponse : Response, new()
    {
        var response = new TResponse();
        response.errors.AddRange(
            errors.Select(e => new Error("Error.Validation", $"{e.Key} {e.Value}")));

        return response;
    }

    public static TResponse FromResult<TResponse, TResult>(Result<TResult> result)
        where TResponse : Response, new()
    {
        if (result.IsFailure)
        {
            var response = new TResponse();
            response.errors.Add(result.Error);

            return response;
        }

        return result.Value.Adapt<TResponse>();
    }

    public static TResponse FromResult<TResponse>(Result result)
        where TResponse : Response, new()
    {
        var response = new TResponse();

        if (result.IsFailure)
            response.errors.Add(result.Error);

        return response;
    }

    public static TResponse FromResultNValue<TResponse, TValue>(Result result, TValue value)
        where TResponse : Response, new()
    {
        var response = value.Adapt<TResponse>();
        if (result.IsFailure)
            response.errors.Add(result.Error);

        return response;
    }
}
