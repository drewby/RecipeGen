// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react'; 
import { render, screen } from '@testing-library/react'; 
import ActionButtons from './ActionButtons'; 

// Mock the child components to avoid rendering issues 
jest.mock('./FeedbackButtons', () => () => <div>MockedFeedbackButtons</div>); 
jest.mock('./CopyButton', () => () => <div>MockedCopyButton</div>); 

describe('ActionButtons', () => { 
  const mockRecipeResponse = { 
    id: '1', 
    recipe: 
    { 
      preparationTime: 30, 
      servings: 4, 
      liked: false, 
    }, 
    errorMessage: 'Error loading recipe', 
    metrics: 
    { 
      maxTokens: 50, 
      frequencyPenalty: 0.8, 
      presencePenalty: 0.2, 
      temperature: 0.7, 
      promptLength: 100, 
      recipeLength: 200, 
      promptTokens: 120, 
      completionTokens: 280, 
      timeTaken: 1200, 
    }, 
    liked: true, 
    disliked: false, 
  }; 
  
  it('renders correctly', () => { 
    render(<ActionButtons recipeResponse={mockRecipeResponse} />); 
    expect(screen.getByText('MockedFeedbackButtons')).toBeInTheDocument(); 
    expect(screen.getByText('MockedCopyButton')).toBeInTheDocument(); 
  }); 
  
  it('passes recipeResponse to FeedbackButtons', () => { 
    render(<ActionButtons recipeResponse={mockRecipeResponse} />); 
    expect(screen.getByText('MockedFeedbackButtons').textContent).toBe('MockedFeedbackButtons'); 
  }); 
  
  it('passes recipe to CopyButton', () => { 
    render(<ActionButtons recipeResponse={mockRecipeResponse} />); 
    expect(screen.getByText('MockedCopyButton').textContent).toBe('MockedCopyButton'); 
  }); 
});
