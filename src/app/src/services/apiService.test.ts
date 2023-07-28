import fetchMock from 'jest-fetch-mock';
import { RecipeRequest } from '../types/recipeRequest';
import { apiService } from './apiService';
import { ModelMetrics } from '../types/modelMetrics';
global.fetch = fetchMock as any;

describe('apiService', () => {
  beforeEach(() => {
    fetchMock.resetMocks();
  });

  const modelMetrics: ModelMetrics = {
    model: 'model',
    prompt: 'prompt',
    maxTokens: 1,
    frequencyPenalty: 1,
    presencePenalty: 1,
    temperature: 1,
    promptLength: 1,
    recipeLength: 1,
    promptTokens: 1,
    completionTokens: 1,
    timeTaken: 1
  };

  test('submitRecipe posts data and returns response', async () => {
    const mockResponse = { id: '1', liked: false, disliked: false, recipe: 'Recipe' };
    fetchMock.mockResponseOnce(JSON.stringify(mockResponse));

    const recipeRequest: RecipeRequest = { 
      description: 'description',
      language: 'en'
     };
    const response = await apiService.submitRecipe(recipeRequest);

    expect(fetchMock).toHaveBeenCalledTimes(1);
    expect(fetchMock).toHaveBeenCalledWith('/api/Recipe', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(recipeRequest)
    });
    expect(response).toEqual(mockResponse);
  });

  test('submitRecipe throws error when response is not ok', async () => {
    fetchMock.mockResponseOnce(JSON.stringify({}), { status: 500 });

    const recipeRequest: RecipeRequest = { 
      description: 'description',
      language: 'en'
     };

    console.error = jest.fn();
    await expect(apiService.submitRecipe(recipeRequest)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

  test('submitRecipe throws error when fetch throws error', async () => {
    fetchMock.mockRejectOnce(new Error('Error'));

    const recipeRequest: RecipeRequest = { 
      description: 'description',
      language: 'en'
     };
    
    console.error = jest.fn();
    await expect(apiService.submitRecipe(recipeRequest)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

  test('likeRecipe posts data and returns no response', async () => {
    fetchMock.mockResponseOnce(JSON.stringify({}));

    await apiService.likeRecipe('123', modelMetrics);

    expect(fetchMock).toHaveBeenCalledTimes(1);
    expect(fetchMock).toHaveBeenCalledWith('/api/Recipe/123/like', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(modelMetrics)
    });
  });


  test('likeRecipe throws error when response is not ok', async () => {
    fetchMock.mockResponseOnce(JSON.stringify({}), { status: 500 });

    console.error = jest.fn();
    await expect(apiService.likeRecipe('123', modelMetrics)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

  test('likeRecipe throws error when fetch throws error', async () => {
    fetchMock.mockRejectOnce(new Error('Error'));

    console.error = jest.fn();
    await expect(apiService.likeRecipe('123', modelMetrics)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

  test('dislikeRecipe posts data and returns no response', async () => {
    fetchMock.mockResponseOnce(JSON.stringify({}));

    await apiService.dislikeRecipe('123', modelMetrics);

    expect(fetchMock).toHaveBeenCalledTimes(1);
    expect(fetchMock).toHaveBeenCalledWith('/api/Recipe/123/dislike', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(modelMetrics)
    });
  });

  test('dislikeRecipe throws error when response is not ok', async () => {
    fetchMock.mockResponseOnce(JSON.stringify({}), { status: 500 });

    console.error = jest.fn();
    await expect(apiService.dislikeRecipe('123', modelMetrics)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

  test('dislikeRecipe throws error when fetch throws error', async () => {
    fetchMock.mockRejectOnce(new Error('Error'));

    console.error = jest.fn();
    await expect(apiService.dislikeRecipe('123', modelMetrics)).rejects.toThrowError();
    expect(console.error).toHaveBeenCalled();
  });

});