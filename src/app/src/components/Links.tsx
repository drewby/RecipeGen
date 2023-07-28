// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react';
import { Language } from '../types/language';

interface Props { language: Language;
  onLanguageChange: (language: Language) => void;
} 

export default class Links extends Component<Props,{}> { 
  render() { return ( 
    <div className="links"> 
      <a href="http://github.com/drewby/RecipeGen" target="_blank" rel="noreferrer">Source Code</a>
      { " [ " } 
      { this.props.language === 'en' 
        ? <span>English</span> 
        : <button className="linkButton" onClick={() => this.props.onLanguageChange('en')}>English</button> 
      } 
      { " | " } 
      { this.props.language === 'ja' 
        ? <span>日本語</span> 
        : <button className="linkButton" onClick={() => this.props.onLanguageChange('ja')}>日本語</button> 
      } 
      { " ] " } 
    </div> 
    ) 
  } 
}
