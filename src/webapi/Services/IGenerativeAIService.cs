// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;

namespace RecipeGen.Services;

public interface IGenerativeAIService
{
    Task<RecipeResponse> GenerateRecipeAsync(string modelName, RecipeRequest request);
}
