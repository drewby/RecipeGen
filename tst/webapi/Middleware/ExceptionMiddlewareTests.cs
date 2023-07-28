// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeGen.Middleware;

namespace RecipeGen.Tests.Middleware;

#pragma warning disable CA2201 // Do not raise reserved exception types

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenExceptionIsThrown_LogsException()
    {
        // Arrange
        var logger = new Mock<ILogger<ExceptionMiddleware>>();
        logger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var middleware = new ExceptionMiddleware((innerHttpContext) => throw new Exception(), logger.Object);
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        logger.Verify(
          x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
          Times.Once);
    }
}
