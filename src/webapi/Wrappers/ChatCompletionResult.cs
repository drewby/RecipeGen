// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Wrappers;

public class ChatCompletionResult
{
  public string Content { get; set; } = null!;
  public string FinishReason { get; set; } = null!;
  public int PromptTokens { get; set; }
  public int CompletionTokens { get; set; }
}
