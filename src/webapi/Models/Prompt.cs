// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Models;

public class Prompt
{
  /// <summary>
  /// The name and/or version of the prompt.
  /// </summary>
  public string Name { get; }
  /// <summary>
  /// The prompt User message to send to the language model.
  /// </summary>
  public string UserMessage { get; }
  /// <summary>
  /// An optional prompt System message to send to the language model.
  /// </summary>
  public string? SystemMessage { get; }

  public Prompt(string name, string userMessage, string? systemMessage = null)
  {
    Name = name;
    UserMessage = userMessage;
    SystemMessage = systemMessage;
  }
}
