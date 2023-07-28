// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Mvc;
using RecipeGen.Constants;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;
using RecipeGen.Observability;
using RecipeGen.Services;

namespace RecipeGen.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{


    private readonly ILogger<RecipeController> _logger;
    private readonly IValidationService _validationService;
    private readonly IGenerativeAIService _generativeAIService;
    private readonly string? _defaultModelName;

    public RecipeController(ILogger<RecipeController> logger,
                            IConfiguration configuration,
                            IValidationService validationService,
                            IGenerativeAIService generativeAIService)
    {
        configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _generativeAIService = generativeAIService ?? throw new ArgumentNullException(nameof(generativeAIService));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

        _logger = logger;
        _validationService = validationService;
        _generativeAIService = generativeAIService;

        _defaultModelName = configuration[ConfigKeys.ModelName];

        if (string.IsNullOrWhiteSpace(_defaultModelName!))
        {
            throw new ConfigException("Model name is not configured");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecipeResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GenerateRecipeAsync([FromBody] RecipeRequest request)
    {
        var valid = _validationService.ValidateRecipeRequest(request);
        if (!valid.IsValid)
        {
            return BadRequest(valid.Message);
        }

        var response = await _generativeAIService.GenerateRecipeAsync(_defaultModelName!, request);

        return Ok(response);
    }

    [HttpPost("{recipeId}/like")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult LikeRecipeAsync(string recipeId, [FromBody] ModelMetrics metrics)
    {
        LogActions.FeedbackSubmitted(_logger, recipeId, "like", metrics, null);
        MetricsService.RecordLikedRecipe(metrics);
        return Ok();
    }

    [HttpPost("{recipeId}/dislike")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DislikeRecipeAsync(string recipeId, [FromBody] ModelMetrics metrics)
    {
        LogActions.FeedbackSubmitted(_logger, recipeId, "dislike", metrics, null);
        MetricsService.RecordDislikedRecipe(metrics);
        return Ok();
    }
}
