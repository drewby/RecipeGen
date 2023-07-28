// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Diagnostics.CodeAnalysis;

namespace RecipeGen.Models.Exceptions;

[ExcludeFromCodeCoverage]
public class ConfigException : Exception
{
    public ConfigException() { }

    public ConfigException(string message)
        : base(message) { }

    public ConfigException(string message, Exception inner)
        : base(message, inner) { }
}
