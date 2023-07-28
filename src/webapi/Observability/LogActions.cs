// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Constants;
using RecipeGen.Models;

namespace RecipeGen.Observability;

internal class LogActions
{
  // Use LoggerMessage delegates for strong typing and avoid boxing for performance improvements
  internal static readonly Action<ILogger, string, Exception?> DebugPromptCreated =
    LoggerMessage.Define<string>(LogLevel.Debug, new EventId(LogEvents.PromptCreatedId, LogEvents.PromptCreatedName), LogEvents.PromptCreatedMessage);
  internal static readonly Action<ILogger, string, Exception?> DebugRecipeGenerated =
    LoggerMessage.Define<string>(LogLevel.Debug, new EventId(LogEvents.DebugRecipeGeneratedId, LogEvents.DebugRecipeGeneratedName), LogEvents.DebugRecipeGeneratedMessage);
  internal static readonly Action<ILogger, ModelMetrics, Exception?> RecipeGenerated =
    LoggerMessage.Define<ModelMetrics>(LogLevel.Information, new EventId(LogEvents.RecipeGeneratedId, LogEvents.RecipeGeneratedName), LogEvents.RecipeGeneratedMessage);
  internal static readonly Action<ILogger, string, Exception> RecipeParsingFailed =
    LoggerMessage.Define<string>(LogLevel.Error, new EventId(LogEvents.RecipeParsingFailedId, LogEvents.RecipeParsingFailedName), LogEvents.RecipeParsingFailedMessage);
  internal static readonly Action<ILogger, Exception> ServerException =
    LoggerMessage.Define(LogLevel.Error, new EventId(LogEvents.ServerExceptionId, LogEvents.ServerExceptionName), LogEvents.ServerExceptionMessage);
  internal static readonly Action<ILogger, string, string, ModelMetrics, Exception?> FeedbackSubmitted =
    LoggerMessage.Define<string, string, ModelMetrics>(LogLevel.Information, new EventId(LogEvents.FeedbackSubmittedId, LogEvents.FeedbackSubmittedName), LogEvents.FeedbackSubmittedMessage);
}
