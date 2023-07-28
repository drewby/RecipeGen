// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Text.RegularExpressions;
using FluentValidation;
using RecipeGen.Models;
using RecipeGen.Models.Requests;

namespace RecipeGen.Services;

public class ValidationService : IValidationService
{
    private readonly Regex _idRegex;
    private readonly IValidator<RecipeRequest> _recipeRequestValidator;

    public ValidationService()
    {
        _idRegex = new Regex("^[0-9a-fA-F]{32}$");
        _recipeRequestValidator = new RecipeRequestValidator();
    }

    public ValidationResult ValidateRecipeRequest(RecipeRequest request)
    {
        var result = _recipeRequestValidator.Validate(request);

        return new ValidationResult()
        {
            IsValid = result.IsValid,
            Message = !result.IsValid ? result.Errors[0].ErrorMessage : string.Empty
        };
    }

    public ValidationResult ValidateId(string id)
    {
        // A 32-character hexadecimal string
        if (string.IsNullOrWhiteSpace(id) || !_idRegex.IsMatch(id))
        {
            return new ValidationResult()
            {
                IsValid = false,
                Message = "Id is invalid"
            };
        }

        return new ValidationResult()
        {
            IsValid = true,
            Message = string.Empty
        };
    }

}
