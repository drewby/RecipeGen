// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
import React, { Component } from 'react';
import { ModelMetrics } from '../types/modelMetrics';

interface Props { 
  modelMetrics: ModelMetrics | null;
} 

export default class ModelMetricsDisplay extends Component<Props, {}> { 
  render() { 
    return (
      <div className="modelMetrics"> 
        <ul className="metrics"> 
          <li> 
            <div className="metricName">Prompt Name</div> 
            <div className="metricValue">{this.props.modelMetrics?.prompt}</div> 
          </li> 
          <li> 
            <div className="metricName">Model Name</div> 
            <div className="metricValue">{this.props.modelMetrics?.model}</div> 
          </li>
          <li>
            <div className="metricName">Max Tokens</div>
            <div className="metricValue">{this.props.modelMetrics?.maxTokens}</div>
          </li>
          <li>
            <div className="metricName">Frequency Penalty</div>
            <div className="metricValue">{this.props.modelMetrics?.frequencyPenalty}</div>
          </li>
          <li>
            <div className="metricName">Presence Penalty</div>
            <div className="metricValue">{this.props.modelMetrics?.presencePenalty}</div>
          </li>
          <li>
            <div className="metricName">Temperature</div>
            <div className="metricValue">{this.props.modelMetrics?.temperature}</div>
          </li>
          <li>
            <div className="metricName">Prompt Length</div>
            <div className="metricValue">{this.props.modelMetrics?.promptLength}ch</div>
          </li>
          <li>
            <div className="metricName">Recipe Length</div>
            <div className="metricValue">{this.props.modelMetrics?.recipeLength}ch</div>
          </li>
          <li>
            <div className="metricName">Prompt Tokens</div>
            <div className="metricValue">{this.props.modelMetrics?.promptTokens}</div>
          </li>
          <li>
            <div className="metricName">Completion Tokens</div>
            <div className="metricValue">{this.props.modelMetrics?.completionTokens}</div>
          </li>
          <li>
            <div className="metricName">Finish Reason</div>
            <div className="metricValue">{this.props.modelMetrics?.finishReason}</div>
          </li>
          <li>
            <div className="metricName">Time Taken</div>
            <div className="metricValue">{(this.props.modelMetrics?.timeTaken ?? 0)/1000}s</div>
          </li>
        </ul> 
      </div>
    );
  } 
}
