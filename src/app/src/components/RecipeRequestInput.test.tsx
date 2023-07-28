// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT

import { fireEvent, render, screen } from '@testing-library/react'; 
import RecipeRequestInput from './RecipeRequestInput'; 
import { IntlProvider } from 'react-intl'; 
import { en, ja } from '../translations'; 
import { act } from 'react-dom/test-utils'; 

describe('RecipeRequestInput', () => { 

    test('renders correctly', () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        expect(screen.getByText('Test')).toBeInTheDocument(); 
        expect(screen.getByPlaceholderText('Describe what you would like to make.')).toBeInTheDocument(); 
    });

    test('japanese renders correctly', () => { 
        render( 
            <IntlProvider locale="ja" messages={ja}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        expect(screen.getByText('Test')).toBeInTheDocument(); 
        expect(screen.getByPlaceholderText('作りたいものを入力してください。')).toBeInTheDocument(); 
    }); 

    test('disables getRecipeButton when loading', () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={true}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        expect(screen.getByText('Test')).toBeInTheDocument(); 
        expect(screen.getByText('Get Recipe')).toBeDisabled(); 
    });

    test('calls onSubmitRecipeRequest when getRecipeButton is clicked', () => { 
        const mockOnSubmitRecipeRequest = jest.fn(); 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={mockOnSubmitRecipeRequest} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        const getRecipeButton = screen.getByText('Get Recipe'); 
        expect(getRecipeButton).toBeInTheDocument(); 
        act(() => { 
            getRecipeButton.click(); 
        }); 
        expect(mockOnSubmitRecipeRequest).toHaveBeenCalled(); 
    }); 

    test('ignores input that is not in [\\u3040-\\u309F\\u30A0-\\u30FF\\u4E00-\\u9FAFa-zA-Z0-9 .,;:!?\'"-]', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        const input = screen.getByPlaceholderText('Describe what you would like to make.'); 
        expect(input).toBeInTheDocument(); 
        fireEvent.change(input, { target: { value: ':' } }); 
        fireEvent.change(input, { target: { value: '\n' } }); 
        fireEvent.change(input, { target: { value: '%' } }); 
        fireEvent.change(input, { target: { value: '#' } }); 
        expect(input).toHaveValue(''); 
    }); 

    test('allows alphanumeric input', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        const input = screen.getByPlaceholderText('Describe what you would like to make.'); 
        expect(input).toBeInTheDocument(); 
        fireEvent.change(input, { target: { value: 'abc123' } }); 
        expect(input).toHaveValue('abc123'); 
    }); 

    test('allows japanese input', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        const input = screen.getByPlaceholderText('Describe what you would like to make.'); 
        expect(input).toBeInTheDocument(); 
        fireEvent.change(input, { target: { value: 'たまご' } }); 
        expect(input).toHaveValue('たまご'); 
    }); 

    test('clicking options button shows options', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        const optionsButton = screen.getByText('>>'); 
        expect(optionsButton).toBeInTheDocument(); 
        fireEvent.click(optionsButton); 
        expect(screen.getByPlaceholderText('Add ingredients to include')).toBeInTheDocument(); 
    }); 

    test('Adding a tag adds it to the list', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        fireEvent.click(screen.getByText('>>')); 
        const addIngredientInput = screen.getByPlaceholderText('Add ingredients to include'); 
        expect(addIngredientInput).toBeInTheDocument(); 
        fireEvent.change(addIngredientInput, { target: { value: 'egg' } }); 
        fireEvent.keyDown(addIngredientInput, { key: 'Enter' }); 
        expect(screen.getByText('egg')).toBeInTheDocument(); 
    }); 

    test('Removing a tag removes it from the list', async () => { 
        render( 
            <IntlProvider locale="en" messages={en}> 
                <RecipeRequestInput onSubmitRecipeRequest={jest.fn()} loading={false}> 
                    <div>Test</div> 
                </RecipeRequestInput> 
            </IntlProvider>
        ); 
        fireEvent.click(screen.getByText('>>')); 
        const addIngredientInput = screen.getByPlaceholderText('Add ingredients to include'); 
        expect(addIngredientInput).toBeInTheDocument(); 
        fireEvent.change(addIngredientInput, { target: { value: 'egg' } }); 
        fireEvent.keyDown(addIngredientInput, { key: 'Enter' }); 
        const removeIngredientButton = screen.getByTitle('Remove egg'); 
        expect(removeIngredientButton).toBeInTheDocument(); 
        fireEvent.click(removeIngredientButton); 
        expect(screen.queryByText('egg')).not.toBeInTheDocument(); 
    });
});
