// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Text.Json;
using RecipeGen.Models;
using RecipeGen.Models.Requests;
using YamlDotNet.Serialization;

namespace RecipeGen.Models.Tests;

public class SerializationTests
{
    // Ensures model classes have appropriate init; properties
    [Fact]
    public void Recipe_Model_Should_Serialize_Json()
    {
        // Arrange
        var recipe = new Recipe();
        recipe.Name = "Test Recipe";
        recipe.Description = "Test Description";
        recipe.PreparationTime = 10;
        recipe.Parts.Add(new Part()
        {
            Name = "Test Part",
            Ingredients = { "Test Ingredient" },
            Steps = { "Test Step" }
        });

        // Act
        var json = JsonSerializer.Serialize<Recipe>(recipe);
        var deserializedRecipe = JsonSerializer.Deserialize<Recipe>(json);

        // Assert
        Assert.NotNull(deserializedRecipe);
        Assert.Equal(recipe.Name, deserializedRecipe.Name);
        Assert.Equal(recipe.Description, deserializedRecipe.Description);
        Assert.Equal(recipe.PreparationTime, deserializedRecipe.PreparationTime);
        Assert.Equal(recipe.Parts.Count, deserializedRecipe.Parts.Count);
        Assert.Equal(recipe.Parts[0].Name, deserializedRecipe.Parts[0].Name);
        Assert.Equal(recipe.Parts[0].Ingredients.Count, deserializedRecipe.Parts[0].Ingredients.Count);
        Assert.Equal(recipe.Parts[0].Ingredients[0], deserializedRecipe.Parts[0].Ingredients[0]);
        Assert.Equal(recipe.Parts[0].Steps.Count, deserializedRecipe.Parts[0].Steps.Count);
        Assert.Equal(recipe.Parts[0].Steps[0], deserializedRecipe.Parts[0].Steps[0]);
    }

    [Fact]
    public void RecipeRequest_Model_Should_Serialize_Json()
    {
        // Arrange
        var recipeRequest = new RecipeRequest
        {
            Description = "Test Description",
            IncludeIngredients = new List<string> { "Test Ingredient" },
            ExcludeIngredients = { "Test Ingredient" },
            Cuisines = { "Test Cuisine" },
            Diets = { "Test Diet" },
            Intolerances = { "Test Intolerance" },
            DishTypes = { "Test Dish Type" },
            MealTypes = { "Test Meal Type" },
            Equipments = { "Test Equipment" }
        };

        // Act
        var json = JsonSerializer.Serialize<RecipeRequest>(recipeRequest);
        var deserializedRecipeRequest = JsonSerializer.Deserialize<RecipeRequest>(json);

        // Assert
        Assert.NotNull(deserializedRecipeRequest);
        Assert.Equal(recipeRequest.Description, deserializedRecipeRequest.Description);
        Assert.Equal(recipeRequest.IncludeIngredients.Count, deserializedRecipeRequest.IncludeIngredients?.Count);
        Assert.Equal(recipeRequest.IncludeIngredients[0], deserializedRecipeRequest.IncludeIngredients?[0]);
        Assert.Equal(recipeRequest.ExcludeIngredients.Count, deserializedRecipeRequest.ExcludeIngredients.Count);
        Assert.Equal(recipeRequest.ExcludeIngredients[0], deserializedRecipeRequest.ExcludeIngredients[0]);
        Assert.Equal(recipeRequest.Cuisines.Count, deserializedRecipeRequest.Cuisines.Count);
        Assert.Equal(recipeRequest.Cuisines[0], deserializedRecipeRequest.Cuisines[0]);
        Assert.Equal(recipeRequest.Diets.Count, deserializedRecipeRequest.Diets.Count);
        Assert.Equal(recipeRequest.Diets[0], deserializedRecipeRequest.Diets[0]);
        Assert.Equal(recipeRequest.Intolerances.Count, deserializedRecipeRequest.Intolerances.Count);
        Assert.Equal(recipeRequest.Intolerances[0], deserializedRecipeRequest.Intolerances[0]);
        Assert.Equal(recipeRequest.DishTypes.Count, deserializedRecipeRequest.DishTypes.Count);
        Assert.Equal(recipeRequest.DishTypes[0], deserializedRecipeRequest.DishTypes[0]);
        Assert.Equal(recipeRequest.MealTypes.Count, deserializedRecipeRequest.MealTypes.Count);
        Assert.Equal(recipeRequest.MealTypes[0], deserializedRecipeRequest.MealTypes[0]);
        Assert.Equal(recipeRequest.Equipments.Count, deserializedRecipeRequest.Equipments.Count);
        Assert.Equal(recipeRequest.Equipments[0], deserializedRecipeRequest.Equipments[0]);
    }

    [Fact]
    public void Recipe_Model_Should_Deserialize_Yaml()
    {
        // Arrange
        var recipeText = @"Name: Chocolate Chip Cookies
Description: A delicious dessert that is perfect for any occasion! 
PreparationTime: 30
Parts:
  - Name: Chocolate Chip Cookies
    Ingredients:
      - 1 cup butter, softened
      - 1 cup white sugar
    Steps:
      - Preheat oven to 350 degrees F (175 degrees C).
      - Bake for about 10 minutes in the preheated oven.";
        var deserializer = new DeserializerBuilder()
          .Build();

        // Act
        var recipe = deserializer.Deserialize<Recipe>(recipeText);

        // Assert
        Assert.Equal("Chocolate Chip Cookies", recipe.Name);
        Assert.Equal("A delicious dessert that is perfect for any occasion!", recipe.Description);
        Assert.Equal(30, recipe.PreparationTime);
        Assert.Equal(1, recipe.Parts.Count);
        Assert.Equal("Chocolate Chip Cookies", recipe.Parts[0].Name);
        Assert.Equal(2, recipe.Parts[0].Ingredients.Count);
        Assert.Equal("1 cup butter, softened", recipe.Parts[0].Ingredients[0]);
        Assert.Equal(2, recipe.Parts[0].Steps.Count);
        Assert.Equal("Preheat oven to 350 degrees F (175 degrees C).", recipe.Parts[0].Steps[0]);
    }

}
