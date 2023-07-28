// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics.CodeAnalysis;
using RecipeGen.Constants;
using RecipeGen.Middleware;
using RecipeGen.Observability;
using RecipeGen.Services;

namespace RecipeGen;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static int Main(string[] args)
    {
        var appInfo = AppInfo.GetAppInfo();
        if (args.Contains("--version"))
        {
            Console.WriteLine($"{appInfo.Name}\n{appInfo.Version}\n{appInfo.Revision}\n{appInfo.BuildTime}");
            return 0;
        }

        var builder = WebApplication.CreateBuilder(args);

        builder.AddHealth();

        builder.Services.AddSingleton<IValidationService, ValidationService>();
    builder.Services.AddSingleton<IParsingService, YamlParsingService>();
        builder.Services.AddSingleton<IGenerativeAIService, OpenAIService>();
        // builder.Services.AddSingleton<IGenerativeAIService, StubAIService>();
        builder.Services.AddSingleton<IPromptService, FilePromptService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseTelemetry();
        app.MapGet("/version", () => $"App Name: {appInfo.Name}, Version: {appInfo.Version}, Revision: {appInfo.Revision}, BuildTime: {appInfo.BuildTime}");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseHealth();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();

        return 0;
    }
}
