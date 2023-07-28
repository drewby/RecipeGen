// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics.CodeAnalysis;

namespace RecipeGen.Models.Exceptions;

[ExcludeFromCodeCoverage]
public class ParsingException : Exception
{
  public ParsingException() { }

  public ParsingException(string message)
      : base(message) { }

  public ParsingException(string message, Exception inner)
      : base(message, inner) { }
}
