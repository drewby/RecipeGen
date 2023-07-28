// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT

import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';
import { act } from 'react-dom/test-utils';

describe('App', () => {
  // mock apiService module
  test('renders learn react link', () => {
    render(<App />);
    const linkElement = screen.getByText(/RecipeGen/i);
    expect(linkElement).toBeInTheDocument();
  });

  test('renders loading spinner when loading', () => {
    jest.mock('./services/apiService', () => ({
      submitRecipe: jest.fn(() => new Promise((resolve) => {
        setTimeout(() => resolve({}), 200);
      })),
    }));

    const apiService = require('./services/apiService');
    render(<App apiService={apiService} />);
    var getRecipeButton = screen.getByText('Get Recipe');
    expect(getRecipeButton).toBeInTheDocument();

    act(() => {
      getRecipeButton.click();
    });

    expect(screen.getByTitle('Loading...')).toBeInTheDocument();
  });

  test('renders error message when recipeResponse contains error message', async () => {
    const apiService = require('./services/apiService');
    const mockSubmitRecipe = jest.spyOn(apiService, 'submitRecipe');
    mockSubmitRecipe.mockImplementation(() => new Promise((resolve) => setTimeout(() => resolve({ errorMessage: 'Test Error'}), 200)));
    
    render(<App apiService={apiService} />);
    var getRecipeButton = screen.getByText('Get Recipe');
    expect(getRecipeButton).toBeInTheDocument();
    
    await act(async () => {
      getRecipeButton.click();
    });

    await screen.findByText('Test Error');
  });

  test('changes language when language link is clicked', async () => {
    render(<App />);
    var getJapaneseLink = screen.getByText('日本語');
    expect(getJapaneseLink).toBeInTheDocument();

    await act(async () => {
      getJapaneseLink.click();
    });

    expect(screen.getByPlaceholderText('作りたいものを入力してください。')).toBeInTheDocument();
  });
});
