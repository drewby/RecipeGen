// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react'; 
import { fireEvent, render, screen, act } from '@testing-library/react'; 
import CopyButton from './CopyButton'; 
import { IntlProvider } from 'react-intl';
import { en } from '../translations';

describe('CopyButton', () => { 
  const recipe = { 
    name: 'Test Recipe', 
    description: 'Test Description', 
    preparationTime: 30, 
    servings: 4, 
    parts: [ 
      { 
        name: 'Part 1', 
        ingredients: ['Ingredient 1', 'Ingredient 2'], 
        steps: ['Step 1', 'Step 2'], 
      }, 
    ], 
  }; 
  
  it('renders CopyButton component', () => { 
    render(
      <IntlProvider locale="en" messages={en}>
        <CopyButton recipe={recipe} />
      </IntlProvider>
    ); 
    const copyButtonElement = screen.getByTitle(/Copy to Clipboard/i); 
    expect(copyButtonElement).toBeInTheDocument(); 
  }); 
    
  it('clicking on the copy button triggers handleCopyClick', async () => { 
    const writeTextMock = jest.fn(() => Promise.resolve()); 
    render(
      <IntlProvider locale="en" messages={en}>
        <CopyButton recipe={recipe} writeText={writeTextMock} />
      </IntlProvider>
    ); 
    const copyButton = screen.getByTitle('Copy to Clipboard'); 
    
    // eslint-disable-next-line testing-library/no-unnecessary-act 
    await act(async () => { fireEvent.click(copyButton); }); 
    
    expect(writeTextMock).toHaveBeenCalledTimes(1); 
  }); 
  
  it('writeToClipboard is called with the correct text', async () => { 
    const writeTextMock = jest.fn(() => Promise.resolve()); 
    render(
      <IntlProvider locale="en" messages={en}>
        <CopyButton recipe={recipe} writeText={writeTextMock} />
      </IntlProvider>
    ); 
    
    const copyButton = screen.getByTitle('Copy to Clipboard'); 
    
    // eslint-disable-next-line testing-library/no-unnecessary-act 
    await act(async () => { fireEvent.click(copyButton); }); 
    
    expect(writeTextMock).toHaveBeenCalledWith('Test Recipe\n\nTest Description\n\nPreparation Time: 30 minutes\nServings: 4\n\nPart 1\nIngredients:\n- Ingredient 1\n- Ingredient 2\n\nSteps:\n1. Step 1\n2. Step 2\n\n'); 
  }); 

  it('renders a div called copyText with recipe', async () => {
    const writeTextMock = jest.fn(() => Promise.resolve());
    render(
      <IntlProvider locale="en" messages={en}>
        <CopyButton recipe={recipe} writeText={writeTextMock} />
      </IntlProvider>
    );

    const copyText = screen.getByTitle('Text to Copy');
    expect(copyText).toBeInTheDocument();
  });
    

  it('shows copiedPopup when state.showCopiedPopup is true', async () => { 
    const writeTextMock = jest.fn(() => Promise.resolve()); 
    render(
      <IntlProvider locale="en" messages={en}>
        <CopyButton recipe={recipe} writeText={writeTextMock} />
      </IntlProvider>
    ); 
    
    const copiedPopup = screen.queryByTestId('copiedPopup'); 
    expect(copiedPopup).toBeNull(); 
    
    const copyButton = screen.getByTitle('Copy to Clipboard'); 
    
    // eslint-disable-next-line testing-library/no-unnecessary-act
    await act(async () => { fireEvent.click(copyButton); }); 
  
    // Wait for the copiedPopup to appear 
    setTimeout(() => { 
      const updatedCopiedPopup = screen.getByTestId('copiedPopup'); 
      expect(updatedCopiedPopup).toBeInTheDocument(); }, 3500); 
  }); 
});
