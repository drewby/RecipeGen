// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component, FC } from 'react'; 
import RecipeRequestOptions from './RecipeRequestOptions'; 
import { RecipeRequest } from '../types/recipeRequest'; 
import PropTypes from 'prop-types';
import { FormattedMessage, useIntl } from 'react-intl'; 

interface State { 
  recipeRequest: RecipeRequest; 
  optionsVisible: boolean; 
  showOptionsPopup: boolean; 
  hasTyped: boolean; 
} 

interface Props { 
  onSubmitRecipeRequest: (recipeRequest: RecipeRequest) => void; 
  children?: React.ReactNode; 
  loading?: boolean; 
} 

interface TextInputProps { 
  description: string; 
  placeholderId: string; 
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void; 
  onKeyDown: (event: React.KeyboardEvent<HTMLInputElement>) => void; 
} 

export const TextInput: FC<TextInputProps> = ({ description, onChange, onKeyDown, placeholderId }) => { 
  const intl = useIntl(); 
  return ( 
    <input 
      type="text" 
      className="recipeRequestInput" 
      placeholder={intl.formatMessage({ id: placeholderId })} 
      value={description} 
      onChange={onChange} 
      onKeyDown={onKeyDown} 
    /> 
  ); 
}; 

export default class RecipeRequestInput extends Component<Props, State> { 
  constructor(props: Props) { 
    super(props);
    this.state = { 
      recipeRequest: { 
        description: '', 
        includeIngredients: [], 
        excludeIngredients: [], 
        intolerances: [], 
        diets: [], 
        cuisines: [], 
        mealTypes: [], 
        dishTypes: [], 
        equipments: [], 
        language: 'en' 
      }, 
      optionsVisible: false, 
      showOptionsPopup: false, 
      hasTyped: false 
    } 
  }

  static propTypes = { 
    onSubmitRecipeRequest: PropTypes.func.isRequired 
  }

  handleChange = (event: React.ChangeEvent<HTMLInputElement>) => { 
    const regex = /[^\u3040-\u309F\u30A0-\u30FF\u4E00-\u9FAFa-zA-Z0-9 .,;:!?'"-]/; 
    if (regex.test(event.target.value)) { 
      // Input does not match the pattern, so ignore it. 
      return; 
    } 
    this.setState((prevState) => ({ 
      recipeRequest: { 
        ...prevState.recipeRequest, 
        description: event.target.value 
      } 
    })); 
    if (!this.state.hasTyped) { 
      this.setState({ 
        showOptionsPopup: true 
      }, () => { 
        setTimeout(() => this.setState({ 
          showOptionsPopup: false, 
          hasTyped: true 
        }), 5000) 
      }) 
    } 
  }

  handleOptionsClick = (event: React.MouseEvent) => { 
    this.setState((prevState) => ({ 
      optionsVisible: !prevState.optionsVisible 
    })); 
  }

  submitRecipeRequest = () => { 
    this.setState({ optionsVisible: false });
    this.props.onSubmitRecipeRequest(this.state.recipeRequest); 
  }

  handleGetRecipeClick = (event: React.MouseEvent) => { 
    this.submitRecipeRequest(); 
  }

  handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => { 
    if (event.key === 'Enter') { 
      this.submitRecipeRequest(); 
    } 
  }

  handleAddTag = (field: keyof RecipeRequest, tag: string) => { 
    this.setState((prevState) => ({ 
      recipeRequest: { 
        ...prevState.recipeRequest, 
        [field]: [...prevState.recipeRequest[field] as string[], tag] 
      } 
    })); 
  }

  handleRemoveTag = (field: keyof RecipeRequest, index: number) => { 
    this.setState((prevState) => ({ 
      recipeRequest: { 
        ...prevState.recipeRequest, 
        [field]: [...prevState.recipeRequest[field] as string[]].filter((_, i) => i !== index) 
      } 
    })); 
  }

  render () { 
    return ( 
      <div className="recipeRequest"> 
        <h1>RecipeGen</h1> 
        <div className="recipeInputs"> 
          <TextInput 
            description={this.state.recipeRequest.description} 
            onChange={this.handleChange} 
            onKeyDown={this.handleKeyDown} 
            placeholderId="app.describe" 
          />
          <button className="button optionsButton" onClick={this.handleOptionsClick}> 
            {this.state.optionsVisible ? '<<' : '>>'}
            <div className={`optionsPopup ${this.state.showOptionsPopup ? ' show' : 'hide'}`}>
              <FormattedMessage id="app.optionsPopup" />
            </div> 
          </button> 
        </div> 
        {this.state.optionsVisible && 
          <div className="options"> 
            <RecipeRequestOptions 
              recipeRequest={this.state.recipeRequest} 
              onAddTag={this.handleAddTag} 
              onRemoveTag={this.handleRemoveTag} 
            /> 
          </div>
        }
        <button className="button recipeRequestButton" onClick={this.handleGetRecipeClick} disabled={this.props.loading}>
          <FormattedMessage id="app.getRecipe" />
        </button> 
        <div>{this.props.children}</div> 
      </div> 
    ) 
  } 
}
