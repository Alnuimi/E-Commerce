using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExeceptionMiddleware(IHostEnvironment env,RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            System.Console.WriteLine("start middleware");
            await next(context);
        }
        catch (Exception ex)
        {
            
            await HandleExpeceptionAsync(context,ex, env);
        }
    }

    private static Task HandleExpeceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError ;
        var response =env.IsDevelopment()
        ? new ApiErrorResponse(context.Response.StatusCode,ex.Message ,ex.StackTrace)
        : new ApiErrorResponse(context.Response.StatusCode,ex.Message ,"Internal Server Error");
        var option = new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
        var json = JsonSerializer.Serialize(response,option);

        return  context.Response.WriteAsync(json);
    }
}