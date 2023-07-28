// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Models;
using RecipeGen.Models.Requests;

namespace RecipeGen.Services;

public interface IValidationService
{
    ValidationResult ValidateRecipeRequest(RecipeRequest request);
    ValidationResult ValidateId(string id);
}
