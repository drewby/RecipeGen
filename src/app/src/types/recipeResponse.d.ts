export interface RecipeResponse {
  id: string;
  recipe: Recipe;
  errorMessage?: string;
  metrics: ModelMetrics;
  liked: boolean;
  disliked: boolean;
}