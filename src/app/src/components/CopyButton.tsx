// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component, forwardRef } from 'react'; 
import { Recipe } from '../types/recipe'; 
import copyIcon from '../assets/images/copy_icon.png'; 
import { useIntl } from 'react-intl';
import { Part } from '../types/part';

interface Props { 
  recipe: Recipe; 
  writeText?: ((text: string) => Promise<void>) | null; 
} 

interface State { 
  showCopiedPopup: boolean; 
} 

interface RecipeTextProps { 
  recipe: Recipe;
} 

export const RecipeText = forwardRef<HTMLDivElement, RecipeTextProps>((props:RecipeTextProps, ref) => {
  const recipe: Recipe = props.recipe;

  const intl = useIntl(); 
  const preparationTime = intl.formatMessage({ id: 'recipe.preparationTime' });
  const minutes = intl.formatMessage({ id: 'recipe.minutes' });
  const servings = intl.formatMessage({ id: 'recipe.servings' });
  const steps = intl.formatMessage({ id: 'recipe.steps' });
  const ingredients = intl.formatMessage({ id: 'recipe.ingredients' });

  let recipeText = `${recipe.name}\n\n`
  recipeText += `${recipe.description}\n\n`
  recipeText += `${preparationTime}: ${recipe.preparationTime}${minutes}\n`
  recipeText += `${servings}: ${recipe.servings}\n\n`
  recipe.parts && recipe.parts.forEach((part: Part) => {
    recipeText += part.name + '\n'
    recipeText += `${ingredients}:\n`
    part.ingredients && part.ingredients.forEach((ingredient: string) => {
      recipeText += `- ${ingredient}\n`
    })
    recipeText += `\n${steps}:\n`
    part.steps && part.steps.forEach((step: string, index: number) => {
      recipeText += `${index+1}. ${step}\n`
    })
    recipeText += '\n'
  })

  return ( 
    <div ref={ref} className="copyText" title="Text to Copy">{recipeText}</div>
  ); 
}); 


export default class CopyButton extends Component<Props, State> { 
  recipeTextRef: React.RefObject<HTMLDivElement>;
  
  constructor(props: Props) { 
    super(props) 
    this.state = { 
      showCopiedPopup: false 
    } 

    this.recipeTextRef = React.createRef<HTMLDivElement>();
  } 
  
  writeToClipboard(text: string) { 
    // For tests 
    if (this.props.writeText) { 
      return this.props.writeText(text) 
    } 
    
    return global.navigator.clipboard.writeText(text) 
  } 
  
  handleCopyClick = (event: React.MouseEvent) => { 
    let recipeText = this.recipeTextRef.current?.firstChild?.textContent;

    if (recipeText) {
      this.writeToClipboard(recipeText).then(() => { 
        this.setState({ showCopiedPopup: true }, () => { 
          setTimeout(() => this.setState({ showCopiedPopup: false }), 3000) 
        }) 
      })
    }
  } 
  
  render() { 
    return ( 
      <div className="copyButton"> 
        <div className={`copiedPopup ${this.state.showCopiedPopup ? 'show' : ''}`}>Copied</div> 
        <button className="actionButton copyButton" title='Copy to Clipboard' onClick={this.handleCopyClick}> 
          <img className="clipboard" src={copyIcon} height="30" alt="Clipboard to copy Recipe" /> 
        </button> 
        <RecipeText ref={this.recipeTextRef} recipe={this.props.recipe} />
      </div> 
    ) 
  } 
}
