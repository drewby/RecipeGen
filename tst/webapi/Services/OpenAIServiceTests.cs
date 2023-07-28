// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;
using RecipeGen.Services;
using RecipeGen.Wrappers;

namespace RecipeGen.Tests.Services;

public class OpenAIServiceTests
{
  private readonly ILogger<OpenAIService> _logger;

  public OpenAIServiceTests()
  {
    _logger = new Mock<ILogger<OpenAIService>>().Object;
  }

  [Fact]
  public void Constructor_NoConfig_ThrowsArgumentNullException()
  {
    // Arrange
    var promptService = new Mock<IPromptService>().Object;

    // Act
    var exception = Record.Exception(() => new OpenAIService(_logger, null!, promptService));

    // Assert
    Assert.NotNull(exception);
    Assert.IsType<ArgumentNullException>(exception);
  }

  [Fact]
  public void Constructor_NoOpenApiKey_ThrowsConfigException()
  {
    // Arrange
    var configurationMock = new Mock<IConfiguration>();

    var promptService = new Mock<IPromptService>().Object;

    // Act
    var exception = Record.Exception(() => new OpenAIService(_logger, configurationMock.Object, promptService));

    // Assert
    Assert.NotNull(exception);
    Assert.IsType<ConfigException>(exception);
  }

  [Fact]
  public void Constructor_NoAzureOpenApiKey_ThrowsConfigException()
  {
    // Arrange
    var configurationMock = new Mock<IConfiguration>();
    configurationMock.SetupGet(config => config["AzureOpenAIUri"]).Returns("test");

    var promptService = new Mock<IPromptService>().Object;

    // Act
    var exception = Record.Exception(() => new OpenAIService(_logger, configurationMock.Object, promptService));

    // Assert
    Assert.NotNull(exception);
    Assert.IsType<ConfigException>(exception);
  }

  [Fact]
  public void GenerateRecipeAsync_Returns_RecipeResponse()
  {
    // Arrange
    var configValues = new Dictionary<string, string?>
      {
        { "OpenAIKey", "test" },
      };
    var configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();

    var promptServiceMock = new Mock<IPromptService>();
    promptServiceMock.Setup(service => service.GetPrompt(It.IsAny<RecipeRequest>())).Returns(new Prompt("test", "test"));

    var openAIClientMock = new Mock<IOpenAIClientWrapper>();
    openAIClientMock.Setup(client => client.GetChatCompletionsAsync(It.IsAny<string>(), It.IsAny<ChatCompletionsOptions>()))
        .ReturnsAsync(new ChatCompletionResult()
        {
          Content = "Name: test",
          FinishReason = "test",
          PromptTokens = 1,
          CompletionTokens = 1
        });

    var openAIService = new OpenAIService(_logger, configuration, promptServiceMock.Object);
    openAIService._openAIClient = openAIClientMock.Object;

    // Act
    var response = openAIService.GenerateRecipeAsync("test", new RecipeRequest()).Result;

    // Assert
    Assert.NotNull(response);
    Assert.IsType<RecipeResponse>(response);
  }

  [Fact]
  public void GenerateRecipeAsync_InvalidResponse_Sets_ErrorMessage()
  {
    // Arrange
    var configValues = new Dictionary<string, string?>
      {
        { "OpenAIKey", "test" },
      };
    var configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();

    var promptServiceMock = new Mock<IPromptService>();
    promptServiceMock.Setup(service => service.GetPrompt(It.IsAny<RecipeRequest>())).Returns(new Prompt("test", "test"));

    var openAIClientMock = new Mock<IOpenAIClientWrapper>();
    openAIClientMock.Setup(client => client.GetChatCompletionsAsync(It.IsAny<string>(), It.IsAny<ChatCompletionsOptions>()))
        .ReturnsAsync(new ChatCompletionResult()
        {
          Content = "test",
          FinishReason = "test",
          PromptTokens = 1,
          CompletionTokens = 1
        });

    var openAIService = new OpenAIService(_logger, configuration, promptServiceMock.Object);
    openAIService._openAIClient = openAIClientMock.Object;

    // Act
    var response = openAIService.GenerateRecipeAsync("test", new RecipeRequest()).Result;

    // Assert
    Assert.NotNull(response);
    Assert.IsType<RecipeResponse>(response);
    Assert.NotNull(response.ErrorMessage);
  }
}
