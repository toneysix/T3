namespace T3.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using T3.Data.Extensions;
using T3.Logger.Extensions;
using T3.Transactions.API;
using T3.Web.Endpoints.v1;
using T3.Web.Exceptions;

public class Program
{
    public static void Main(string[] args)
    {
        var app = Program.BuildWebApp(args);
        Program.ConfigureWebApp(app);

        app.Run();
    }

    private static WebApplication BuildWebApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddProblemDetails(o =>
            {
                o.CustomizeProblemDetails = (c) =>
                {
                    var httpCtx = c.HttpContext;
                    var activity = httpCtx.Features.Get<IHttpActivityFeature>()?.Activity;

                    c.ProblemDetails.Instance = $"{httpCtx.Request.Method} {httpCtx.Request.Path}";
                    c.ProblemDetails.Extensions.Add("requestId", httpCtx.TraceIdentifier);
                    c.ProblemDetails.Extensions.Add("traceId", activity?.Id);
                };
            })
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddCustomLogger(builder.Environment.IsDevelopment() ? LogLevel.Debug : LogLevel.Information)
            .AddSqlite("Data Source=./T3.db3;Foreign Keys=True;")
            .AddRepositories()
            .AddTransactionAPI()
            .AddExceptionHandler<RfcExceptionHandler>();

        return builder.Build();
    }

    private static void ConfigureWebApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();
        app.MapGroup("/api/v1")
            .AddTransactionEndpoints();
    }
}
