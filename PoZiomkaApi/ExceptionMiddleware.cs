using Microsoft.AspNetCore.Mvc;
using PoZiomkaDomain.Exceptions;
using PoZiomkaInfrastructure.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace PoZiomkaApi;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        ProblemDetails problemDetails;

        try
        {
            await next(context);
            return;
        }
        catch (EmailTakenException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
                Title = "Email is taken"
            };
        }
        catch (EmailNotRegisteredException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
                Title = "Email not registered"
            };
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
                Title = "Validation failed",
                Extensions =
                {
                    ["validationFailures"] = exception.Errors
                }
            };
        }
        catch (DomainException exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Title = "Internal Server Domain Layer Error - something went wrong"
            };
        }
        catch (InfrastructureException exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Title = "Internal Server Infrastructure Layer Error - something went wrong"
            };
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Title = "Internal Server Error - something went wrong"
            };
        }

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
