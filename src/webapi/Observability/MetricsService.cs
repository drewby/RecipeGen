// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics.Metrics;
using RecipeGen.Models;

namespace RecipeGen.Observability;

internal class MetricsService
{
  private static readonly AppInfo AppInfo = AppInfo.GetAppInfo();
  internal static readonly Meter _meter = new Meter(AppInfo.Name, AppInfo.Version);
  private static readonly Counter<long> _promptLengthCounter = _meter.CreateCounter<long>("recipegen_promptlength", "bytes", "The length of the prompt");
  private static readonly Counter<long> _recipeLengthCounter = _meter.CreateCounter<long>("recipegen_recipelength", "bytes", "The length of the recipe");
  private static readonly Counter<long> _recipeGenerationTimeCounter = _meter.CreateCounter<long>("recipegen_recipetime", "ms", "The time to generate a recipe");
  private static readonly Counter<long> _recipeGenerationCountCounter = _meter.CreateCounter<long>("recipegen_recipecount", "count", "The number of recipes generated");
  private static readonly Counter<long> _completionTokensCounter = _meter.CreateCounter<long>("recipegen_completiontokens", "count", "The number of completion tokens generated");
  private static readonly Counter<long> _promptTokensCounter = _meter.CreateCounter<long>("recipegen_prompttokens", "count", "The number of prompt tokens generated");
  private static readonly Counter<long> _likedRecipesCounter = _meter.CreateCounter<long>("recipegen_likedrecipes", "count", "The number of liked recipes");
  private static readonly Counter<long> _dislikedRecipesCounter = _meter.CreateCounter<long>("recipegen_dislikedrecipes", "count", "The number of disliked recipes");
  private static readonly Counter<long> _recipeParseErrorCounter = _meter.CreateCounter<long>("recipegen_recipeparseerror", "count", "The number of recipe parsing errors");

  public static void RecordMetrics(ModelMetrics modelMetrics)
  {
    var tags = GetTags(modelMetrics);

    _recipeGenerationCountCounter.Add(1, tags);
    _promptLengthCounter.Add(modelMetrics.PromptLength, tags);
    _recipeLengthCounter.Add(modelMetrics.RecipeLength, tags);
    _recipeGenerationTimeCounter.Add(modelMetrics.TimeTaken, tags);
    _completionTokensCounter.Add(modelMetrics.CompletionTokens, tags);
    _promptTokensCounter.Add(modelMetrics.PromptTokens, tags);
  }

  public static void RecordLikedRecipe(ModelMetrics modelMetrics)
  {
    _likedRecipesCounter.Add(1, GetTags(modelMetrics));
  }

  public static void RecordDislikedRecipe(ModelMetrics modelMetrics)
  {
    _dislikedRecipesCounter.Add(1, GetTags(modelMetrics));
  }

  public static void RecordRecipeParseError(ModelMetrics modelMetrics)
  {
    _recipeParseErrorCounter.Add(1, GetTags(modelMetrics));
  }

  private static KeyValuePair<string, object?>[] GetTags(ModelMetrics modelMetrics)
  {
    return new[]
    {
      new KeyValuePair<string, object?>("prompt", modelMetrics.Prompt),
      new KeyValuePair<string, object?>("language", modelMetrics.Language),
      new KeyValuePair<string, object?>("model", modelMetrics.Model),
      new KeyValuePair<string, object?>("maxtokens", modelMetrics.MaxTokens),
      new KeyValuePair<string, object?>("frequencypenalty", modelMetrics.FrequencyPenalty),
      new KeyValuePair<string, object?>("presencepenalty", modelMetrics.PresencePenalty),
      new KeyValuePair<string, object?>("temperature", modelMetrics.Temperature),
      new KeyValuePair<string, object?>("finishreason", modelMetrics.FinishReason),
    };
  }
}
