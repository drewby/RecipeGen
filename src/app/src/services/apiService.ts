import { RecipeRequest } from '../types/recipeRequest';
import { RecipeResponse } from '../types/recipeResponse';
import { ModelMetrics } from '../types/modelMetrics';

export const apiService = {
  submitRecipe: async (recipeRequest: RecipeRequest): Promise<RecipeResponse> => {
    return fetch('/api/Recipe', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(recipeRequest)
    })
      .then(async response => {
        if (!response.ok) {
          return response.text().then(text => Promise.reject(new Error(text)));
        }
        return response.json();
      })
      .catch(error => {
        console.error('Error in apiService.submitRecipe: ', error);
        throw error;
      });
  },

  likeRecipe: async (recipeId: string, modelMetrics: ModelMetrics): Promise<void> => {
    return fetch(`/api/Recipe/${recipeId}/like`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(modelMetrics)
    })
      .then(async response => {
        if (!response.ok) {
          return response.text().then(text => Promise.reject(new Error(text)));
        }
      })
      .catch(error => {
        console.error('Error in apiService.likeRecipe: ', error);
        throw error;
      });
  },

  dislikeRecipe: async (recipeId: string, modelMetrics: ModelMetrics): Promise<void> => {
    return fetch(`/api/Recipe/${recipeId}/dislike`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(modelMetrics)
    })
      .then(async response => {
        if (!response.ok) {
          return response.text().then(text => Promise.reject(new Error(text)));
        }
      })
      .catch(error => {
        console.error('Error in apiService.dislikeRecipe: ', error);
        throw error;
      });
  },
};
