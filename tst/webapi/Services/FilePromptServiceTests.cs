// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Tests.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using RecipeGen.Services;

public class FilePromptServiceTests
{
    private readonly ILogger<FilePromptService> _logger;

    public FilePromptServiceTests()
    {
        var loggerMock = new Mock<ILogger<FilePromptService>>();
        _logger = loggerMock.Object;
    }

    [Fact]
    public void Constructor_NoPromptFile_ThrowsConfigException()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(config => config["PromptFile"]).Returns("");

        // Act
        var exception = Record.Exception(() => new FilePromptService(_logger, configurationMock.Object));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ConfigException>(exception);
    }

    [Fact]
    public void Constructor_PromptFileDoesNotExist_ThrowsConfigException()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(config => config["PromptFile"]).Returns("TestData/doesnotexist");

        // Act
        var exception = Record.Exception(() => new FilePromptService(_logger, configurationMock.Object));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ConfigException>(exception);
    }

    [Fact]
    public void GetPrompt_RecipeRequest_ReturnsPrompt()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(config => config["PromptFile"]).Returns("TestData/prompt1");

        var promptService = new FilePromptService(_logger, configurationMock.Object);
        var recipeRequest = new RecipeRequest
        {
            Description = "Test Description",
            IncludeIngredients = { "Test Ingredient 1", "Test Ingredient 2" },
            ExcludeIngredients = { "Test Ingredient 3" },
            Cuisines = { "Test Cuisine" },
            Diets = { "Test Diet" },
            Intolerances = { "Test Intolerance 1", "Test Intolerance 2" },
            DishTypes = { "Test Dish Type" },
            MealTypes = { "Test Meal Type" },
            Equipments = { "Test Equipment" }
        };

        // Act
        var prompt = promptService.GetPrompt(recipeRequest);

        // Assert
        Assert.Equal("TestData/prompt1", prompt.Name);
        var expectedMessage = @"Test prompt
--Recipe Request--
Description: Test Description
IncludeIngredients:
- Test Ingredient 1
- Test Ingredient 2
ExcludeIngredients:
- Test Ingredient 3
Cuisines:
- Test Cuisine
Diets:
- Test Diet
Intolerances:
- Test Intolerance 1
- Test Intolerance 2
DishTypes:
- Test Dish Type
MealTypes:
- Test Meal Type
Equipments:
- Test Equipment
Language: en

--End Recipe Request--";
        Assert.Equal(expectedMessage, prompt.UserMessage);
        Assert.Null(prompt.SystemMessage);
    }

    [Fact]
    public void GetPrompt_RecipeRequest_WithSystem_ReturnsPromptWithSystemMessage()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(config => config["PromptFile"]).Returns("TestData/prompt2");

        var promptService = new FilePromptService(_logger, configurationMock.Object);

        var recipeRequest = new RecipeRequest
        {
            Description = "Test Description"
        };

        // Act
        var prompt = promptService.GetPrompt(recipeRequest);

        // Assert
        Assert.Equal("TestData/prompt2", prompt.Name);
        var expectedUserMessage = @"Test prompt
--Recipe Request--
Description: Test Description
IncludeIngredients: []
ExcludeIngredients: []
Cuisines: []
Diets: []
Intolerances: []
DishTypes: []
MealTypes: []
Equipments: []
Language: en

--End Recipe Request--";
        Assert.Equal(expectedUserMessage, prompt.UserMessage);
        Assert.Equal("You are an AI system.", prompt.SystemMessage);
    }

}
