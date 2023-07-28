// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics;
using Azure;
using Azure.AI.OpenAI;
using RecipeGen.Constants;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;
using RecipeGen.Observability;
using RecipeGen.Wrappers;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace RecipeGen.Services;

public class OpenAIService : IGenerativeAIService
{
  private readonly ILogger<OpenAIService> _logger;
  private readonly IPromptService _promptService;
  private readonly IParsingService _parsingService;
  internal IOpenAIClientWrapper _openAIClient;
  private float _temperature;
  private readonly int _maxTokens;
  private readonly float _frequencyPenalty;
  private readonly float _presencePenalty;

  public OpenAIService(ILogger<OpenAIService> logger, IConfiguration config, IPromptService promptService, IParsingService parsingService)
  {
    config = config ?? throw new ArgumentNullException(nameof(config));

    _logger = logger;
    _promptService = promptService;
    _parsingService = parsingService;

    _openAIClient = CreateOpenAIClientWrapper(config);

    // Model parameters
    _maxTokens = config.GetValue(ConfigKeys.MaxTokens, 1000);
    _temperature = config.GetValue(ConfigKeys.Temperature, 1.0f);
    _frequencyPenalty = config.GetValue(ConfigKeys.FrequencyPenalty, 0.0f);
    _presencePenalty = config.GetValue(ConfigKeys.PresencePenalty, 0.0f);
  }

  public async Task<RecipeResponse> GenerateRecipeAsync(string modelName, RecipeRequest request)
  {
    var prompt = _promptService.GetPrompt(request);
    LogActions.DebugPromptCreated(_logger, prompt.UserMessage, null);

    var options = GetChatCompletionsOptions(prompt);

    var timer = Stopwatch.StartNew();
    var response = await _openAIClient.GetChatCompletionsAsync(modelName, options);
    timer.Stop();
    LogActions.DebugRecipeGenerated(_logger, response.Content, null);

    var modelMetrics = GetMetrics(modelName, prompt, timer, response);
    LogActions.RecipeGenerated(_logger, modelMetrics, null);
    MetricsService.RecordMetrics(modelMetrics);

    RecipeResponse recipeResponse;
    try
    {
      recipeResponse = new RecipeResponse()
      {
        Recipe = _parsingService.ParseRecipeResponse(response.Content),
        Metrics = modelMetrics
      };
    }
    catch (ParsingException ex)
    {
      LogActions.RecipeParsingFailed(_logger, response.Content, ex);
      recipeResponse = new RecipeResponse()
      {
        ErrorMessage = ErrorMessages.ParseError,
        Metrics = modelMetrics
      };
      MetricsService.RecordRecipeParseError(modelMetrics);
    }

    return recipeResponse;
  }

  private ModelMetrics GetMetrics(string modelName, Prompt prompt, Stopwatch timer, ChatCompletionResult response)
  {
    return new ModelMetrics()
    {
      TimeTaken = timer.ElapsedMilliseconds,
      Model = modelName,
      Prompt = prompt.Name,
      MaxTokens = _maxTokens,
      Temperature = _temperature,
      FrequencyPenalty = _frequencyPenalty,
      PresencePenalty = _presencePenalty,
      PromptLength = prompt.UserMessage.Length + (prompt.SystemMessage?.Length ?? 0),
      RecipeLength = response.Content.Length,
      PromptTokens = response.PromptTokens,
      CompletionTokens = response.CompletionTokens,
      FinishReason = response.FinishReason
    };
  }

  private static IOpenAIClientWrapper CreateOpenAIClientWrapper(IConfiguration config)
  {
    string? azureOpenAIUri = config[ConfigKeys.AzureOpenAiUri];
    string? azureOpenAIKey = config[ConfigKeys.AzureOpenAiKey];
    string? openAIKey = config[ConfigKeys.OpenAiKey];

    if (string.IsNullOrWhiteSpace(azureOpenAIUri) && string.IsNullOrWhiteSpace(openAIKey))
    {
      throw new ConfigException($"Either {ConfigKeys.AzureOpenAiUri} or {ConfigKeys.OpenAiKey} must be configured.");
    }

    if (!string.IsNullOrWhiteSpace(azureOpenAIUri) && string.IsNullOrWhiteSpace(azureOpenAIKey))
    {
      throw new ConfigException($"{ConfigKeys.AzureOpenAiKey} must be configured when {ConfigKeys.AzureOpenAiUri} is configured.");
    }

    var client = !string.IsNullOrEmpty(azureOpenAIUri)
                        ? new OpenAIClient(
                            new Uri(azureOpenAIUri),
                            new AzureKeyCredential(azureOpenAIKey ?? string.Empty))
                        : new OpenAIClient(openAIKey);

    return new OpenAIClientWrapper(client);
  }

  private ChatCompletionsOptions GetChatCompletionsOptions(Prompt prompt)
  {
    var options = new ChatCompletionsOptions
    {
      MaxTokens = _maxTokens,
      Temperature = _temperature,
      FrequencyPenalty = _frequencyPenalty,
      PresencePenalty = _presencePenalty,
    };
    options.Messages.Add(new ChatMessage(ChatRole.User, prompt.UserMessage));
    if (!string.IsNullOrWhiteSpace(prompt.SystemMessage))
    {
      options.Messages.Add(new ChatMessage(ChatRole.System, prompt.SystemMessage));
    }
    return options;
  }
}
