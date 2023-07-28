// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react';
import RecipeDisplay from './RecipeDisplay';
import { render, screen } from '@testing-library/react';
import { IntlProvider } from 'react-intl';
import { en } from '../translations';

describe ('RecipeDetails', () => { 

  it('renders correctly', () => { 
    const recipeResponse = { 
      id: '1',
      recipe: { 
        name: 'Test Recipe',
        preparationTime: 30,
        servings: 4,
        liked: false,
      },
      errorMessage: 'Error loading recipe',
      metrics: { 
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

    render( 
      <IntlProvider locale="en" messages={en}> 
        <RecipeDisplay recipeResponse={recipeResponse} /> 
      </IntlProvider>);

    expect(screen.getByText('Test Recipe')).toBeInTheDocument();
    expect(screen.getByTitle('Like It')).toBeInTheDocument();
  });
});
