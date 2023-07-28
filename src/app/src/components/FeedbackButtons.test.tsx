// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react';
import { screen, render, fireEvent } from '@testing-library/react';
import FeedbackButtons from './FeedbackButtons';
import { RecipeResponse } from '../types/recipeResponse';
import { wait } from '@testing-library/user-event/dist/utils';

describe('FeedbackButtons', () => { 
  // Mock the apiService module 
  jest.mock('../services/apiService', () => ({ 
    likeRecipe: jest.fn(() => Promise.resolve()), 
    dislikeRecipe: jest.fn(() => Promise.resolve()), 
  }));

  // const apiService = require('../services/apiService');
  var mockRecipeResponse:RecipeResponse;
  
  beforeEach(() => { 
    jest.clearAllMocks();
  
    mockRecipeResponse = { 
      id: 'recipe-id-123', 
      recipe: { 
        name: 'Test Recipe', 
        description: 'A delicious test recipe', 
        preparationTime: 30, 
        servings: 4, 
      }, 
      metrics: { 
        maxTokens: 50, 
        frequencyPenalty: 0.8, 
        presencePenalty: 0.9, 
        temperature: 1.0, 
        promptLength: 100, 
        recipeLength: 400, 
        promptTokens: 20, 
        completionTokens: 200, 
        timeTaken: 500,
      }, 
      liked: false, 
      disliked: false, 
      };
  });

  it('renders correctly', () => { 
    render(<FeedbackButtons recipeResponse={mockRecipeResponse} />);
    expect(screen.getByTitle('Like It')).toBeInTheDocument();
  });
  
  it('renders like button when not liked', () => { 
    render(<FeedbackButtons recipeResponse={mockRecipeResponse} />);
    const likeButton = screen.getByTitle('Like It');
    expect(likeButton).toBeInTheDocument();
  });

  it('renders dislike button when not disliked', () => { 
    render(<FeedbackButtons recipeResponse={mockRecipeResponse} />);
    const dislikeButton = screen.getByTitle('Dislike It');
    expect(dislikeButton).toBeInTheDocument();
  });

  it('disables the buttons when liked', () => { 
    render(<FeedbackButtons recipeResponse={{ ...mockRecipeResponse, liked: true }} />);
    
    const likeButton = screen.getByTitle('Like It');
    expect(likeButton).toBeDisabled();
    
    const dislikeButton = screen.getByTitle('Dislike It');
    expect(dislikeButton).toBeDisabled();
  });

  it('calls likeRecipe when like button is clicked', async () => { 
    render(<FeedbackButtons recipeResponse={mockRecipeResponse} />);
    
    const likeButton = screen.getByTitle('Like It');
    fireEvent.click(likeButton);
    await wait();
    
    expect(likeButton).toBeDisabled();
    expect(mockRecipeResponse.liked).toBe(true);
    expect(mockRecipeResponse.disliked).toBe(false);
    // expect(apiService.likeRecipe).toHaveBeenCalledWith( 
    //   mockRecipeResponse.id, 
    //   mockRecipeResponse.metrics 
    // );
  });

  it('calls dislikeRecipe when dislike button is clicked', async () => { 
    render(<FeedbackButtons recipeResponse={mockRecipeResponse} />);

    const dislikeButton = screen.getByTitle('Dislike It');
    fireEvent.click(dislikeButton);

    expect(dislikeButton).toBeDisabled();
    expect(mockRecipeResponse.liked).toBe(false);
    expect(mockRecipeResponse.disliked).toBe(true);
    // expect(apiService.dislikeRecipe).toHaveBeenCalledWith( 
    //   mockRecipeResponse.id, 
    //   mockRecipeResponse.metrics 
    // );
  });
});
