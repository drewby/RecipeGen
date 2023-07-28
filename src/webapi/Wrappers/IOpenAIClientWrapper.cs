// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using Azure.AI.OpenAI;

namespace RecipeGen.Wrappers;

public interface IOpenAIClientWrapper
{
    Task<ChatCompletionResult> GetChatCompletionsAsync(string modelName, ChatCompletionsOptions options);
}
