namespace T3.Web.Filters;

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using T3.API.Shared.Abstract;

internal sealed class ProblemDetailsResultFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        var response = await next(context) as Response;
        if (response is not Response)
            return response;

        if (!response.Errors.Any())
            return response;

        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        var problem = TypedResults.Problem(
            title: "An error(s) occurred. Check out errors section for more information",
            statusCode: StatusCodes.Status400BadRequest,
            extensions: new Dictionary<string, object?>() 
            {
                {
                    "errors", 
                    response.Errors
                        .Select(err => KeyValuePair.Create(err.Code, err.Message))
                        .ToDictionary()
                }
            });

        return problem; 
    }
}
