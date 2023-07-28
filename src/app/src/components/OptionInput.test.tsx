// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React from 'react';
import { screen, render, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import OptionInput from './OptionInput';
import { IntlProvider } from 'react-intl';
import { en } from '../translations';

describe('OptionInput', () => { 
  const tags = ['tag1', 'tag2'];
  const addTagMock = jest.fn();
  const removeTagMock = jest.fn();

  afterEach(() => { 
    addTagMock.mockClear();
    removeTagMock.mockClear();
  });

  it('renders correctly', () => { 
    render( 
      <IntlProvider locale="en" messages={en}> 
        <OptionInput optionType="includeIngredients" tags={tags} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> 
      </IntlProvider> 
    );

    screen.getByPlaceholderText('Add ingredients to include');
  });

  it('adds a tag when user presses Enter key', () => { 
    render( 
      <IntlProvider locale="en" messages={en}> 
        <OptionInput optionType="includeIngredients" tags={tags} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> 
      </IntlProvider> 
    );

    const inputElement = screen.getByPlaceholderText('Add ingredients to include');

    fireEvent.change(inputElement, { target: { value: 'NewTag' } });
    fireEvent.keyDown(inputElement, { key: 'Enter' });

    expect(addTagMock).toHaveBeenCalledWith('NewTag');
    expect(inputElement).toHaveValue('');
  });

  it('adds a tag when user selects an option from datalist', () => { 
    render( <IntlProvider locale="en" messages={en}> <OptionInput optionType="includeIngredients" tags={tags} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> </IntlProvider> );
    const inputElement = screen.getByPlaceholderText('Add ingredients to include');

    fireEvent.change(inputElement, { target: { value: 'NewTag' } });
    fireEvent.keyDown(inputElement, { key: 'ArrowDown' });

    // Simulate selecting an option fireEvent.keyDown(inputElement, { key: 'Enter' });
    expect(addTagMock).toHaveBeenCalledWith('NewTag');
    expect(inputElement).toHaveValue('');
  });

  it('does not add a tag when user presses Enter with an empty input', () => { 
    render( <IntlProvider locale="en" messages={en}> <OptionInput optionType="includeIngredients" tags={tags} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> </IntlProvider> );
    const inputElement = screen.getByPlaceholderText('Add ingredients to include');

    fireEvent.keyDown(inputElement, { key: 'Enter' });
    
    expect(addTagMock).not.toHaveBeenCalled();
  });

  it('adds a tag when user focuses out of the input', () => { 
    render( <IntlProvider locale="en" messages={en}> <OptionInput optionType="includeIngredients" tags={tags} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> </IntlProvider> );
    const inputElement = screen.getByPlaceholderText('Add ingredients to include');

    fireEvent.change(inputElement, { target: { value: 'NewTag' } });
    fireEvent.blur(inputElement);

    expect(addTagMock).toHaveBeenCalledWith('NewTag');
    expect(inputElement).toHaveValue('');
  });

  it('removes a tag when remove button is clicked', () => { 
    render( <IntlProvider locale="en" messages={en}> <OptionInput optionType="includeIngredients" tags={[ "tag1" ]} onAddTag={addTagMock} onRemoveTag={removeTagMock} /> </IntlProvider> );
    const removeButton = screen.getByText('X');

    fireEvent.click(removeButton);
    
    expect(removeTagMock).toHaveBeenCalledWith(0);
  });
});
