// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Models;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;

namespace RecipeGen.Services;

public interface IParsingService
{
  Recipe ParseRecipeResponse(string response);
}