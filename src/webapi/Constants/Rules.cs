// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Constants;

internal static class Rules
{
    internal const int MaxDescriptionLength = 1000;
    internal const int MaxItemLength = 100;
    internal const int MaxItems = 10;
    internal const string ValidCharactersRegex = @"^[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}A-Za-z0-9 \.,;:!?'""-]*$";

    internal static string[] SupportedLanguages = { "en", "ja" };

    internal const string DescriptionLengthExceededMessage = "Description cannot exceed 1000 characters.";
    internal const string InvalidCharactersMessage = "{0} contains invalid characters.";
    internal const string MaxItemsExceededMessage = "Cannot include more than 10 {0}.";
    internal const string MaxItemLengthExceededMessage = "{0} cannot exceed 100 characters.";
    internal const string UnsupportedLanguageMessage = "Language not supported. Supported languages are: en, jp.";

    internal const string Description = "Description";
    internal const string Ingredients = "Ingredients";
    internal const string Cuisines = "Cuisines";
    internal const string Diets = "Diets";
    internal const string Intolerances = "Intolerances";
    internal const string DishTypes = "Dish Types";
    internal const string MealTypes = "Meal Types";
    internal const string Equipments = "Equipments";

}
