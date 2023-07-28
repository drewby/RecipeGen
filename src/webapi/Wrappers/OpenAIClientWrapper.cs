// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics.CodeAnalysis;
using Azure.AI.OpenAI;

namespace RecipeGen.Wrappers;

public class OpenAIClientWrapper : IOpenAIClientWrapper
{
  private readonly OpenAIClient _openAIClient;

  public OpenAIClientWrapper(OpenAIClient openAIClient)
  {
    _openAIClient = openAIClient;
  }

  [ExcludeFromCodeCoverage]
  public async Task<ChatCompletionResult> GetChatCompletionsAsync(string modelName, ChatCompletionsOptions options)
  {
    var response = await _openAIClient.GetChatCompletionsAsync(modelName, options);

    return new ChatCompletionResult()
    {
      Content = response.Value.Choices[0].Message.Content,
      FinishReason = response.Value.Choices[0].FinishReason,
      PromptTokens = response.Value.Usage.PromptTokens,
      CompletionTokens = response.Value.Usage.CompletionTokens
    };
  }

}
