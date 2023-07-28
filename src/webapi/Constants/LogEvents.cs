// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Constants;

internal static class LogEvents
{
    internal const int ServerExceptionId = 1000;
    internal const string? ServerExceptionName = "ServerException";
    internal const int RecipeGeneratedId = 1001;
    internal const string RecipeGeneratedName = "RecipeGenerated";
    internal const string RecipeGeneratedMessage = "Recipe generated. {@ModelMetrics}";
    internal const int RecipeParsingFailedId = 1002;
    internal const string RecipeParsingFailedName = "RecipeParsingFailed";
    internal const string RecipeParsingFailedMessage = "Recipe parsing failed: {RecipeContent}";
    internal const string ServerExceptionMessage = "An exception was caught by middleware.";
    internal const int PromptCreatedId = 3001;
    internal const string? PromptCreatedName = "PromptCreated";
    internal const string PromptCreatedMessage = "Prompt created: {Prompt}";
    internal const int DebugRecipeGeneratedId = 3002;
    internal const string? DebugRecipeGeneratedName = "DebugRecipeGenerated";
    internal const string DebugRecipeGeneratedMessage = "Recipe generated: {Recipe}";
    internal const int FeedbackSubmittedId = 1003;
    internal const string? FeedbackSubmittedName = "FeedbackSubmitted";
    internal const string FeedbackSubmittedMessage = "Feedback submitted for {RecipeId}: {Feedback} {@ModelMetrics}";
}
