// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
namespace RecipeGen.Models;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = null!;
}
