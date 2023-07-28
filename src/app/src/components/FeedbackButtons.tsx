// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react';
import { apiService } from '../services/apiService';
import { RecipeResponse } from '../types/recipeResponse';
import smile from '../assets/images/smile.png';
import smileFilled from '../assets/images/smile_filled.png';
import frown from '../assets/images/frown.png';
import frownFilled from '../assets/images/frown_filled.png';

interface Props { 
  recipeResponse: RecipeResponse;
} 

interface State { 
  liked: boolean;
  disliked: boolean;
} 

export default class FeedbackButtons extends Component<Props, State> { 
  constructor(props: Props) { 
    super(props);
    this.state = { 
      liked: props.recipeResponse.liked, 
      disliked: props.recipeResponse.disliked 
    } 
  } 
  
  handleLikeClick = async (event: React.MouseEvent) => { 
    this.setState({ liked: true, disliked: false });

    this.props.recipeResponse.liked = true;
    this.props.recipeResponse.disliked = false;

    await apiService.likeRecipe(this.props.recipeResponse.id, this.props.recipeResponse.metrics);
  } 
  
  handleDislikeClick = async (event: React.MouseEvent) => { 
    this.setState({ liked: false, disliked: true });
    
    this.props.recipeResponse.liked = false;
    this.props.recipeResponse.disliked = true;

    await apiService.dislikeRecipe(this.props.recipeResponse.id, this.props.recipeResponse.metrics);
  } 
  
  render() { 
    return ( 
      <div className="feedbackButtons"> 
        <button className="actionButton likeButton" title='Like It' onClick={this.handleLikeClick} disabled={this.state.liked || this.state.disliked} >
          {this.state.liked 
            ? <img className="feedbackFilled" src={smileFilled} height="30" alt="Liked" /> 
            : <img className="feedback" src={smile} height="30" alt="Smile" /> 
          } 
        </button> 
        <button className="actionButton dislikeButton" title='Dislike It' onClick={this.handleDislikeClick} disabled={this.state.liked || this.state.disliked} >
          {this.state.disliked 
            ? <img className="feedbackFilled" src={frownFilled} height="30" alt="Disliked" /> 
            : <img className="feedback" src={frown} height="30" alt="Frown" /> 
          } 
        </button> 
      </div> 
    ) 
  } 
}
