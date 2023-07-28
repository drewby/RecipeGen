// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Collections.ObjectModel;

namespace RecipeGen.Models;

/// <summary>
/// A part of a Recipe.
/// </summary>
public class Part
{
    /// <summary>
    /// The name of the part.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// The ingredients of the part.
    /// </summary>
    public IList<string> Ingredients { get; init; } = new List<string>();
    /// <summary>
    /// The steps of the part.
    /// </summary>
    public IList<string> Steps { get; init; } = new List<string>();
}
