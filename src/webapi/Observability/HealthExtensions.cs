// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Builder;

namespace RecipeGen.Observability;

public static class HealthExtensions
{
  public static WebApplicationBuilder AddHealth(this WebApplicationBuilder builder)
  {
    builder = builder ?? throw new ArgumentNullException(nameof(builder));

    builder.Services.AddHealthChecks();

    return builder;
  }

  public static IEndpointRouteBuilder UseHealth(this IEndpointRouteBuilder app)
  {
    app = app ?? throw new ArgumentNullException(nameof(app));

    app.MapHealthChecks("/health");

    return app;
  }
}
