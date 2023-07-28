// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Collections.ObjectModel;

namespace RecipeGen.Models;

/// <summary>
/// A Recipe. A Recipe is made up of one or more Parts.
/// </summary>
public class Recipe
{
    /// <summary>
    /// The name of the Recipe.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// The description of the Recipe.
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// The time it takes to prepare/cook the recipe.
    /// </summary>
    public int PreparationTime { get; set; }
    /// <summary>
    /// The number of servings the recipe makes.
    /// </summary>
    public int Servings { get; set; }
    /// <summary>
    /// The parts of the recipe.
    /// </summary>
    public IList<Part> Parts { get; init; } = new List<Part>();
}
