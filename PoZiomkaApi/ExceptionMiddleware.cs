﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PoZiomkaDomain.Exceptions;
using PoZiomkaInfrastructure.Exceptions;
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
        catch (UnauthorizedException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = exception.Message,
                Title = "User cannot make this request"
            };
        }
        catch (InvalidTokenException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
                Title = "Given token is invalid"
            };
        }
        catch (ClaimNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = exception.Message,
                Title = "Request is unauthorized because a claim could not be found"
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
        catch (InvalidCredentialsException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = exception.Message,
                Title = "Invalid email or password"
            };
        }
        catch (StudentNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message,
                Title = "Student not found"
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
