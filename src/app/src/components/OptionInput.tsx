// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component, FC } from 'react';
import { useIntl } from 'react-intl';

interface Props { 
  optionType: string;
  tags: string[];
  autocompleteOptions?: string[];
  onAddTag: (tag: string) => void;
  onRemoveTag: (index: number) => void;
} 

interface State { 
  options: string[];
  value: string;
} 

interface TextInputProps { 
  value: string;
  optionType: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyDown: (event: React.KeyboardEvent<HTMLInputElement>) => void;
  onBlur: (event: React.FocusEvent<HTMLInputElement>) => void;
} 

export const TextInput: FC<TextInputProps> = ({ value, onKeyDown, onChange, onBlur, optionType }) => { 
  const intl = useIntl();

  const optionsData = optionType === 'includeIngredients'
                      || optionType === 'excludeIngredients' 
                      ? 'ingredients' 
                      : optionType;

  const options:string[] = require(`../assets/tags/${optionsData}.json`);

  return ( 
    <div>
        <input type="text" 
           className="tagInput" 
           placeholder={intl.formatMessage({ id: `recipeRequestOptions.${optionType}` })} 
           value={value} 
           onKeyDown={onKeyDown} 
           onChange={onChange} 
           onBlur={onBlur} 
           list={optionType} />
        <datalist id={optionType}> 
          {options.sort().map((option, index) => ( 
            <option key={index} value={option} /> 
          ))} 
        </datalist> 
      </div>
   );
};

export default class OptionInput extends Component<Props, State> { 
  constructor(props: Props) { 
    super(props);
    this.state = { 
      options: [], 
      value: '', 
    } 
  } 
  
  keyDown = false 
  handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => { 
    if (event.key === 'Enter') { 
      event.preventDefault() 
      this.addTagAndClearInput(this.state.value) 
    } 
    else if (event.key !== 'Unidentified') { 
      this.keyDown = true 
    } 
  } 
  
  handleChange = (event: React.ChangeEvent<HTMLInputElement>) => { 
    const value = event.target.value 
    this.setState({ value }) 
    
    // If there was a change with no KeyDown event 
    // then the user selected an option from the datalist 
    if (!this.keyDown) { 
      this.addTagAndClearInput(value) 
    } 
    
    this.keyDown = false 
  } 
  
  handleBlur = (event: React.FocusEvent<HTMLInputElement>) => { 
    // don't add whitespace 
    if (event.target.value.trim()) { 
      this.addTagAndClearInput(event.target.value) 
    } 
  } 
    
  addTagAndClearInput(value: string) { 
    value = value.replace(/[^\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FAFa-zA-Z0-9 ,-]/g, '') 
    value.split(',')
         .filter((tag: string) => tag.trim() !== '')
         .forEach((tag: string) => { 
            this.props.onAddTag(tag) 
          }) 
    this.setState({ value: '' }) 
  } 
  
  render () { 
    return ( 
      <div className="optionInput"> 
        <TextInput 
          value={this.state.value} 
          optionType={this.props.optionType} 
          onChange={this.handleChange} 
          onKeyDown={this.handleKeyDown} 
          onBlur={this.handleBlur} /> 
        <div className="tags"> 
          {this.props.tags.map((tag, index) => ( 
            <span className="tag" key={index}> 
              <button className="button removeTagButton" title={`Remove ${tag}`} onClick={() => this.props.onRemoveTag(index)}>X</button> 
              {tag} 
            </span>
          ))} 
        </div> 
      </div> 
    ) 
  } 
}
