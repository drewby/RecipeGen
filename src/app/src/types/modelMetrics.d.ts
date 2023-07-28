export interface ModelMetrics {
  prompt?: string;
  model?: string;
  maxTokens?: number;
  frequencyPenalty?: number;
  presencePenalty?: number;
  temperature?: number;
  promptLength?: number;
  recipeLength?: number;
  promptTokens?: number;
  completionTokens?: number;
  finishReason?: string;
  timeTaken?: number;
}