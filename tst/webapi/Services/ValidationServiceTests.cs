// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Constants;
using RecipeGen.Models.Requests;
using RecipeGen.Services;

#pragma warning disable CA1305 // Specify IFormatProvider

namespace RecipeGen.Tests.Services;

public class ValidationServiceTests
{
  [Fact]
  public void Validate_WhenDescriptionIsTooLong_ReturnsInvalid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      Description = new string('a', 1001)
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.False(result.IsValid);
    Assert.Equal(Rules.DescriptionLengthExceededMessage, result.Message);
  }

  [Fact]
  public void Validate_WhenDescriptionContainsInvalidCharacters_ReturnsInvalid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      Description = "a\nb"
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.False(result.IsValid);
    Assert.Equal(string.Format(Rules.InvalidCharactersMessage, Rules.Description), result.Message);
  }

  [Fact]
  public void Validate_DescriptionSupportsJapaneseCharacters_ReturnsValid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      Description = "日本語"
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.True(result.IsValid);
  }

  [Fact]
  public void Validate_WhenIncludeIngredientsContainsTooManyItems_ReturnsInvalid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      IncludeIngredients = new List<string>(Enumerable.Repeat("a", 11))
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.False(result.IsValid);
    Assert.Equal(string.Format(Rules.MaxItemsExceededMessage, Rules.Ingredients), result.Message);
  }

  [Fact]
  public void Validate_WhenIncludeIngredientsContainsItemThatIsTooLong_ReturnsInvalid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      IncludeIngredients = new List<string>() { new string('a', 101) }
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.False(result.IsValid);
    Assert.Equal(string.Format(Rules.MaxItemLengthExceededMessage, Rules.Ingredients), result.Message);
  }

  [Fact]
  public void Validate_WhenIncludeIngredientsContainsItemWithInvalidCharacters_ReturnsInvalid()
  {
    // Arrange
    var request = new RecipeRequest()
    {
      IncludeIngredients = new List<string>() { "a\nb" }
    };

    var validationService = new ValidationService();

    // Act
    var result = validationService.ValidateRecipeRequest(request);

    // Assert
    Assert.False(result.IsValid);
    Assert.Equal(string.Format(Rules.InvalidCharactersMessage, Rules.Ingredients), result.Message);
  }
}
