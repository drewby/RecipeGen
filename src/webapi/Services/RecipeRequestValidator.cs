// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Linq.Expressions;
using FluentValidation;
using RecipeGen.Constants;
using RecipeGen.Models.Requests;

namespace RecipeGen.Services;

#pragma warning disable CA1305 // Specify IFormatProvider

public class RecipeRequestValidator : AbstractValidator<RecipeRequest>
{
  public RecipeRequestValidator()
  {
    RuleFor(req => req.Description)
        .Length(0, Rules.MaxDescriptionLength).WithMessage(Rules.DescriptionLengthExceededMessage)
        .Matches(Rules.ValidCharactersRegex).WithMessage(string.Format(Rules.InvalidCharactersMessage, Rules.Description));

    RuleForEach(req => req.IncludeIngredients)
        .Length(0, Rules.MaxItemLength).WithMessage(string.Format(Rules.MaxItemLengthExceededMessage, Rules.Ingredients))
        .Matches(Rules.ValidCharactersRegex).WithMessage(string.Format(Rules.InvalidCharactersMessage, Rules.Ingredients));

    ApplyItemRules(req => req.IncludeIngredients, Rules.Ingredients);
    ApplyItemRules(req => req.ExcludeIngredients, Rules.Ingredients);
    ApplyItemRules(req => req.Cuisines, Rules.Cuisines);
    ApplyItemRules(req => req.Diets, Rules.Diets);
    ApplyItemRules(req => req.Intolerances, Rules.Intolerances);
    ApplyItemRules(req => req.DishTypes, Rules.DishTypes);
    ApplyItemRules(req => req.MealTypes, Rules.MealTypes);
    ApplyItemRules(req => req.Equipments, Rules.Equipments);

    ApplyMaxItemsRule(req => req.IncludeIngredients, Rules.Ingredients);
    ApplyMaxItemsRule(req => req.ExcludeIngredients, Rules.Ingredients);
    ApplyMaxItemsRule(req => req.Cuisines, Rules.Cuisines);
    ApplyMaxItemsRule(req => req.Diets, Rules.Diets);
    ApplyMaxItemsRule(req => req.Intolerances, Rules.Intolerances);
    ApplyMaxItemsRule(req => req.DishTypes, Rules.DishTypes);
    ApplyMaxItemsRule(req => req.MealTypes, Rules.MealTypes);
    ApplyMaxItemsRule(req => req.Equipments, Rules.Equipments);

    RuleFor(req => req.Language)
        .Must(lang => Rules.SupportedLanguages.Contains(lang)).WithMessage(Rules.UnsupportedLanguageMessage);
  }

  private void ApplyItemRules(Expression<Func<RecipeRequest, IEnumerable<string>>> property, string rule)
  {
    RuleForEach<string>(property)
        .Length(0, Rules.MaxItemLength).WithMessage(string.Format(Rules.MaxItemLengthExceededMessage, rule))
        .Matches(Rules.ValidCharactersRegex).WithMessage(string.Format(Rules.InvalidCharactersMessage, rule))
        .When(req => property.Compile()(req) != null);
  }

  private void ApplyMaxItemsRule(Expression<Func<RecipeRequest, IList<string>>> property, string rule)
  {
    // Null is ok because it means the user didn't specify any items for this rule.
    RuleFor(property)
        .Must(list => list.Count <= Rules.MaxItems).WithMessage(string.Format(Rules.MaxItemsExceededMessage, rule))
        .When(req => property.Compile()(req) != null);
  }
}
