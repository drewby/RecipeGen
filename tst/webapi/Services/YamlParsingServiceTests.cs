// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Models.Exceptions;
using RecipeGen.Services;

namespace RecipeGen.Tests.Services;

public class YamlParsingServiceTests
{
  [Fact]
  public void YamlParsingService_ReturnsValidRecipe()
  {
    // Arrange
    var parsingService = new YamlParsingService();

    // Act
    var recipe = parsingService.ParseRecipeResponse(@"Name: Chocolate Chip Cookies
Description: A delicious treat
Parts:
  - Name: Cookies
    Ingredients:
    - 1 cup of flour
    - 1 cup of sugar  
    Steps:
    - Mix ingredients
    - Bake at 350 degrees for 10 minutes
");

    // Assert
    Assert.Equal("Chocolate Chip Cookies", recipe.Name);
    Assert.Equal("A delicious treat", recipe.Description);
    Assert.Equal(1, recipe.Parts.Count);
    Assert.Equal(2, recipe.Parts[0].Ingredients.Count);
    Assert.Equal("1 cup of flour", recipe.Parts[0].Ingredients[0]);
    Assert.Equal("1 cup of sugar", recipe.Parts[0].Ingredients[1]);
    Assert.Equal(2, recipe.Parts[0].Steps.Count);
    Assert.Equal("Mix ingredients", recipe.Parts[0].Steps[0]);
    Assert.Equal("Bake at 350 degrees for 10 minutes", recipe.Parts[0].Steps[1]);
  }

  [Fact]
  public void YamlParsingService_WithInvalidLines_ReturnsValidRecipe()
  {
    // Arrange
    var parsingService = new YamlParsingService();

    // Act
    var recipe = parsingService.ParseRecipeResponse(@"---
Name: Chocolate Chip Cookies
Description: A delicious treat
Parts:
  - Name: Cookies
    Ingredients:
    - 1 cup of flour
    - 1 cup of sugar  
    Steps:
    - Mix ingredients
    - Bake at 350 degrees for 10 minutes
Enjoy your delicious cookies!
");

    // Assert
    Assert.Equal("Chocolate Chip Cookies", recipe.Name);
  }

  [Fact]
  public void YamlParsingService_WithNoValidRecipe_ThrowsException()
  {
    // Arrange
    var parsingService = new YamlParsingService();

    // Act
    var exception = Assert.Throws<ParsingException>(() => parsingService.ParseRecipeResponse(@"---"));
  }

}