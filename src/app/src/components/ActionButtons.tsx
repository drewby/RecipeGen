// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react'; 
import FeedbackButtons from './FeedbackButtons'; 
import CopyButton from './CopyButton'; 
import { RecipeResponse } from '../types/recipeResponse'; 

interface Props { 
  recipeResponse: RecipeResponse; 
} 

export default class ActionButtons extends Component<Props, {}> { 
  render () { 
    return ( 
      <div className="actionButtons"> 
        <FeedbackButtons recipeResponse={this.props.recipeResponse} />
        <CopyButton recipe={this.props.recipeResponse.recipe} /> 
      </div>
    ) 
  } 
}
