// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react';
import ActionButtons from './ActionButtons';
import RecipeDetails from './RecipeDetails';
import { RecipeResponse } from '../types/recipeResponse';
import ModelMetricsDisplay from './ModelMetricsDisplay';

interface Props { 
  recipeResponse: RecipeResponse | null;
} 

export default class RecipeDisplay extends Component<Props, {}> { 
  render () { 
    return ( 
      <div className={`recipeDisplay ${this.props.recipeResponse ? "" : "hide"}`} > 
        { this.props.recipeResponse && this.props.recipeResponse.recipe && 
          <div> 
            <div className="recipeHeader"> 
              <h1>{this.props.recipeResponse.recipe.name}</h1> 
              <ActionButtons recipeResponse={this.props.recipeResponse} /> 
            </div> 
            <RecipeDetails recipe={this.props.recipeResponse.recipe} /> 
          </div> 
        } 
        { this.props.recipeResponse && this.props.recipeResponse.metrics && 
          <ModelMetricsDisplay modelMetrics={this.props.recipeResponse.metrics} /> 
        } 
      </div> 
    ) 
  } 
}
