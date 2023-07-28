// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Models.Requests;

public class RecipeRequest
{
    /// <summary>
    /// A description of what recipe to generate. It can be as general or specific as you want.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Ingredients that you want to include in the recipe.
    /// </summary>
    public IList<string> IncludeIngredients { get; init; } = new List<string>();

    /// <summary>
    /// Ingredients that you want to exclude from the recipe.
    /// </summary>
    public IList<string> ExcludeIngredients { get; init; } = new List<string>();

    /// <summary>
    /// World cuisines that you want to limit recipes to. More than one cuisine can be specified
    /// and the result may be a fusion.
    /// </summary>
    public IList<string> Cuisines { get; init; } = new List<string>();

    /// <summary>
    /// Any popular diets such as vegan, vegetarian, pescatarian, paleo, keto, etc.
    /// </summary>
    public IList<string> Diets { get; init; } = new List<string>();

    /// <summary>
    /// Alergies or intolerances such as gluten, peanut, dairy, egg, soy, sesame, wheat, etc.
    /// </summary>
    public IList<string> Intolerances { get; init; } = new List<string>();

    /// <summary>
    /// The type of Dish such as "Main", "Side", "Dessert", etc.
    /// </summary>
    public IList<string> DishTypes { get; init; } = new List<string>();

    /// <summary>
    /// The type of Meal such as "Breakfast", "Lunch", "Dinner", etc.
    /// </summary>
    public IList<string> MealTypes { get; init; } = new List<string>();

    /// <summary>
    /// Any equipment that you want to use for the recipe such as "Sous Vide", "Instant Pot", etc.
    /// </summary>
    public IList<string> Equipments { get; init; } = new List<string>();

    /// <summary>
    /// The language the AI should respond in.
    /// </summary>
    public string Language { get; init; } = "en";
}
