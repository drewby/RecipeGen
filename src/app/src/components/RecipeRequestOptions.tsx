// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react'; 
import OptionInput from './OptionInput'; 
import { RecipeRequest } from '../types/recipeRequest'; 

const fields: (keyof RecipeRequest)[] = [
  'includeIngredients', 
  'excludeIngredients', 
  'intolerances', 
  'cuisines', 
  'diets', 
  'mealTypes', 
  'dishTypes', 
  'equipments',
]; 

interface Props { 
  recipeRequest: RecipeRequest; 
  onAddTag: (field: keyof RecipeRequest, tag: string) => void; 
  onRemoveTag: (field: keyof RecipeRequest, index: number) => void; 
} 

export default class RecipeRequestOptions extends Component<Props, {}> {
  render() {
    return (
      <div className="recipeRequestOptions">
        {fields.map((field, index) => (
          <OptionInput 
            key={index} 
            optionType={field} 
            tags={this.props.recipeRequest[field] as string[]} 
            onAddTag={(tag:string) => this.props.onAddTag(field,tag)}
            onRemoveTag={(index:number) => this.props.onRemoveTag(field,index)}
          />
        ))}
      </div>
    )
  }
}
