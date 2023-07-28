// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Net;
using RecipeGen.Constants;
using RecipeGen.Observability;

namespace RecipeGen.Middleware;

internal class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    _logger = logger;
    _next = next;
  }

#pragma warning disable CA1031 // Do not catch general exception types
  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      LogActions.ServerException(_logger, ex);
      await HandleExceptionAsync(httpContext, ex);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    // contenttype is plain text
    context.Response.ContentType = "text/plain";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var message = "Internal Server Error.";

    return context.Response.WriteAsync(message);
  }
}
