// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react';
import { Recipe } from '../types/recipe';
import { FormattedMessage } from 'react-intl';

interface Props { 
  recipe: Recipe;
} 

export default class RecipeDetails extends Component<Props, {}> 
{ 
  render () { 
    return ( 
      <div className="recipeDetails"> 
        <p>{this.props.recipe.description}</p> 
        <p><FormattedMessage id="recipe.preparationTime" />: {this.props.recipe.preparationTime} <FormattedMessage id="recipe.minutes" /></p>
        <p><FormattedMessage id="recipe.servings" />: {this.props.recipe.servings}</p> 
        {this.props.recipe.parts && this.props.recipe.parts.map((part, partIndex) => ( 
          <div key={partIndex}> 
            <h2>{part.name}</h2> 
            <ul> 
              {part.ingredients.map((ingredient: string, ingredientIndex: number) => ( 
                <li key={ingredientIndex}> {ingredient} </li> 
              ))} 
            </ul> 
            <h3><FormattedMessage id="recipe.steps" /></h3> 
            <ol> 
              {part.steps.map((step: string, stepIndex: number) => ( 
                <li key={stepIndex}>{step}</li> 
              ))} 
            </ol> 
            </div> 
        ))} 
      </div> 
      ) 
    } 
  }
