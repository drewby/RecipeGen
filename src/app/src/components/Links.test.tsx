// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import { screen, render } from '@testing-library/react';
import Links from './Links';

describe('Links', () => { 
  
  it('renders correctly', () => { 
    render(<Links language="en" onLanguageChange={jest.fn()} />);
    screen.getByText('Source Code');
  });

  it('renders English button when language is English', () => { 
    render(<Links language="en" onLanguageChange={jest.fn()} />);
    screen.getByText('English');
  });

  it('renders Japanese button when language is Japanese', () => { 
    render(<Links language="ja" onLanguageChange={jest.fn()} />);
    screen.getByText('日本語');
  });

  it('calls onLanguageChange when English button is clicked', () => { 
    const onLanguageChange = jest.fn();
    render(<Links language="ja" onLanguageChange={onLanguageChange} />);
  
    screen.getByText('English').click();
  
    expect(onLanguageChange).toHaveBeenCalledWith('en');
  });

  it('calls onLanguageChange when Japanese button is clicked', () => { 
    const onLanguageChange = jest.fn();
    render(<Links language="en" onLanguageChange={onLanguageChange} />);

    screen.getByText('日本語').click();

    expect(onLanguageChange).toHaveBeenCalledWith('ja');
  });
});
