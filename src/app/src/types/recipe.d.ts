export interface Recipe {
  name?: string;
  description?: string;
  preparationTime: number;
  servings: number;
  parts?: Part[];
}