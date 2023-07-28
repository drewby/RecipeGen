// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Formatting.Compact;

namespace RecipeGen.Observability;

public static class ConfigureTelemetry
{
  private static readonly AppInfo _appInfo = AppInfo.GetAppInfo();

  public static WebApplicationBuilder AddTelemetry(this WebApplicationBuilder builder)
  {
    var logger = new LoggerConfiguration()
        .ReadFrom
        .Configuration(builder.Configuration)
        .Enrich.WithProperty("ServiceName", _appInfo.Name)
        .Enrich.WithProperty("ServiceVersion", _appInfo.Version)
        .Enrich.WithProperty("ServiceRevision", _appInfo.Revision)
        .Enrich.WithProperty("ServiceBuildTime", _appInfo.BuildTime)
        .CreateLogger();
    builder.Host.UseSerilog(logger);

    return builder;
  }

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
