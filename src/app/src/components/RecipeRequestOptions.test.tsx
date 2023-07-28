// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT

import { fireEvent, render, screen } from '@testing-library/react';
import RecipeRequestOptions from './RecipeRequestOptions';
import { en } from '../translations';
import { IntlProvider } from 'react-intl';

describe('RecipeRequestOptions', () => {
    const recipeRequest = {
        description: 'Test Description',
        includeIngredients: ['Test Ingredient 1', 'Test Ingredient 2'],
        excludeIngredients: ['Test Ingredient 3', 'Test Ingredient 4'],
        cuisines: [],
        diets: [],
        intolerances: [],
        dishTypes: [],
        mealTypes: [],
        equipments: [],
        language: 'en',
    }

    test('renders correctly', () => {
        render(
            <IntlProvider locale="en" messages={en}>
                <RecipeRequestOptions 
                    recipeRequest={recipeRequest} 
                    onAddTag={jest.fn()} 
                    onRemoveTag={jest.fn()}
                />
            </IntlProvider>
        );
        expect(screen.getByPlaceholderText('Add ingredients to include')).toBeInTheDocument();
    });

    test('calls onAddTag when enter is clicked', () => {
        const mockOnAddTag = jest.fn();
        render(
            <IntlProvider locale="en" messages={en}>
                <RecipeRequestOptions 
                    recipeRequest={recipeRequest} 
                    onAddTag={mockOnAddTag} 
                    onRemoveTag={jest.fn()}
                />
            </IntlProvider>
        );
        const includeIngredientsInput = screen.getByPlaceholderText('Add ingredients to include');
        fireEvent.change(includeIngredientsInput, { target: { value: 'NewTag' } });
        fireEvent.keyDown(includeIngredientsInput, { key: 'Enter' });
        expect(mockOnAddTag).toHaveBeenCalled();
    });

    test('calls onRemoveTag when X is clicked', () => {
        const mockOnRemoveTag = jest.fn();
        render(
            <IntlProvider locale="en" messages={en}>
                <RecipeRequestOptions 
                    recipeRequest={recipeRequest} 
                    onAddTag={jest.fn()} 
                    onRemoveTag={mockOnRemoveTag}
                />
            </IntlProvider>
        );
        const removeTagButton = screen.getByTitle('Remove Test Ingredient 1');
        fireEvent.click(removeTagButton);
        expect(mockOnRemoveTag).toHaveBeenCalled();
    });
});
