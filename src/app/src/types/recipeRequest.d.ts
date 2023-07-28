export interface RecipeRequest {
  description: string;
  includeIngredients?: string[];
  excludeIngredients?: string[];
  cuisines?: string[];
  diets?: string[];
  intolerances?: string[];
  dishTypes?: string[];
  mealTypes?: string[];
  equipments?: string[];
  language: string;
}