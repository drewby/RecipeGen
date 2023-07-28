// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react';
import { screen, render } from '@testing-library/react';
import RecipeDetails from './RecipeDetails';
import { IntlProvider } from 'react-intl';
import { en } from '../translations';

describe('RecipeDetails', () => { 
  const recipe = { 
    name: 'Test Recipe',
    description: 'A delicious test recipe', 
    preparationTime: 30, 
    servings: 4, 
    parts: [ 
      { 
        name: 'Part 1', 
        ingredients: [ "Ingredient 1", "Ingredient 2", ], 
        steps: [ "Step 1", "Step 2", ], 
      }, 
      { 
        name: 'Part 2', 
        ingredients: [ "Ingredient 3", "Ingredient 4", ], 
        steps: [ "Step 3", "Step 4", ], 
      }, 
    ], 
  } 
  
  it('renders correctly', () => { 
    render( 
      <IntlProvider locale="en" messages={en}> 
        <RecipeDetails recipe={recipe} /> 
      </IntlProvider>
    );

    expect(screen.getByText('A delicious test recipe')).toBeInTheDocument();
    expect(screen.getByText('Preparation Time: 30 minutes')).toBeInTheDocument();
    expect(screen.getByText('Servings: 4')).toBeInTheDocument();
    expect(screen.getByText('Part 1')).toBeInTheDocument();
    expect(screen.getByText('Part 2')).toBeInTheDocument();
    expect(screen.getByText('Ingredient 1')).toBeInTheDocument();
    expect(screen.getByText('Ingredient 2')).toBeInTheDocument();
    expect(screen.getByText('Ingredient 3')).toBeInTheDocument();
    expect(screen.getByText('Ingredient 4')).toBeInTheDocument();
    expect(screen.getByText('Step 1')).toBeInTheDocument();
    expect(screen.getByText('Step 2')).toBeInTheDocument();
    expect(screen.getByText('Step 3')).toBeInTheDocument();
    expect(screen.getByText('Step 4')).toBeInTheDocument();
  });
});
