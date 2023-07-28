// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Models;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;

namespace RecipeGen.Services;

public class StubAIService : IGenerativeAIService
{
  public Task<RecipeResponse> GenerateRecipeAsync(string modelName, RecipeRequest request)
  {
    return Task.FromResult(new RecipeResponse
    {
      Metrics = new ModelMetrics
      {
        Prompt = "Prompts/20001",
        Model = "gpt-3.5-turbo",
        MaxTokens = 1000,
        Temperature = 1.0f,
        FrequencyPenalty = 0.0f,
        PresencePenalty = 0.0f,
        PromptLength = 515,
        RecipeLength = 1000,
        PromptTokens = 100,
        CompletionTokens = 900,
        FinishReason = "stop",
        TimeTaken = 1000
      },
      Recipe = new Recipe
      {
        Name = "Chocolate Chip Cookies",
        Description = "A delicious chocolate chip cookie recipe",
        Parts = new List<Part> {
          new Part {
            Name = "Chocolate Chip Cookies",
            Ingredients = new List<string> {
              "1 cup butter",
              "1 cup white sugar",
              "1 cup packed brown sugar",
              "2 eggs",
              "2 teaspoons vanilla extract",
              "3 cups all-purpose flour",
              "1 teaspoon baking soda",
              "2 teaspoons hot water",
              "1/2 teaspoon salt",
              "2 cups semisweet chocolate chips",
              "1 cup chopped walnuts"
            },
            Steps = new List<string> {
              "Preheat oven to 350 degrees F (175 degrees C).",
              "Cream together the butter, white sugar, and brown sugar until smooth. Beat in the eggs one at a time, then stir in the vanilla. Dissolve baking soda in hot water. Add to batter along with salt. Stir in flour, chocolate chips, and nuts. Drop by large spoonfuls onto ungreased pans.",
              "Bake for about 10 minutes in the preheated oven, or until edges are nicely browned."
            }
          }
        }
      }
    });
  }
}
