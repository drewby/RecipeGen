// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace RecipeGen.Observability;

public static class ConfigureTelemetry
{

  public static IApplicationBuilder UseTelemetry(this IApplicationBuilder app)
  {
    app = app ?? throw new ArgumentNullException(nameof(app));

    // get string[] list from Configuration with key PrometheusPrefixes
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var prefixes = configuration.GetSection("PrometheusPrefixes").Get<string[]>();

    // if prefixes is null or empty, use default
    if (prefixes == null || prefixes.Length == 0)
    {
      prefixes = new string[] { "http://localhost:9464/" };
    }

    var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter(MetricsService._meter.Name)
        .AddAspNetCoreInstrumentation()
        .AddPrometheusHttpListener(
          options => options.UriPrefixes = prefixes
        )
        .Build();

    var traceProvider = Sdk.CreateTracerProviderBuilder()
        .AddOtlpExporter()
        .AddAspNetCoreInstrumentation()
        .Build();

    return app;
  }
}
