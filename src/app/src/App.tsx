// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT

import React, { Component } from 'react'; 
import 'bootstrap/dist/css/bootstrap.css';
import './assets/styles/main.css'; 
import RecipeDisplay from './components/RecipeDisplay'; 
import RecipeRequestInput from './components/RecipeRequestInput'; 
import Links from './components/Links'; 
import { RecipeRequest } from './types/recipeRequest'; 
import { apiService } from './services/apiService'; 
import { RecipeResponse } from './types/recipeResponse'; 
import { IntlProvider } from 'react-intl'; 
import { en,ja } from './translations'; 
import { Language } from './types/language'; 

interface Props { 
  apiService?: any; 
}

interface State { 
  recipeResponse: RecipeResponse | null; 
  errorMessage: string | null; 
  loading: boolean; 
  language: Language; 
}

class App extends Component<Props, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      recipeResponse: null,
      errorMessage: null,
      loading: false,
      language: 'en'
    }
  }

  api = this.props.apiService || apiService;

  messages = {
    en: en,
    ja: ja
  }

  componentDidMount(): void {
    let localSetting = localStorage.getItem("selectedLanguage");

    if (!localSetting) {
      const browserLang = navigator.languages
        ? navigator.languages[0]
        : (navigator.language || (navigator as any).userLanguage);
      localSetting = browserLang.split('-')[0];

      if (localSetting !== 'en' && localSetting !== 'ja') {
        localSetting = 'en';
      }
    }

    this.setState({ language: localSetting as Language });
  }

  handleLanguageChange = (language: Language) => {
    localStorage.setItem("selectedLanguage", language);
    this.setState({ language: language });
  }

  handleSubmitRecipeRequest = async (recipeRequest: RecipeRequest) => {
    this.setState({ 
      recipeResponse: null, 
      errorMessage: null, 
      loading: true 
    });

    recipeRequest.language = this.state.language;

    try {
      const recipeResponse = await this.api.submitRecipe(recipeRequest);

      this.setState({
        recipeResponse: recipeResponse,
        loading: false
      });

      if (recipeResponse.errorMessage) {
        this.setState({
          errorMessage: recipeResponse.errorMessage,
          loading: false
        });
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        this.setState({
          errorMessage: "Something went wrong. Please try again.",
          loading: false
        });
      }
      console.log(error);
    }
  }

  render() {
    return (
      <IntlProvider locale={this.state.language} messages={this.messages[this.state.language]}>
        <div className="App">
          <RecipeRequestInput onSubmitRecipeRequest={this.handleSubmitRecipeRequest} loading={this.state.loading}> 
            {this.state.errorMessage && <div className="errorMessage">{this.state.errorMessage}</div>}
          </RecipeRequestInput>
          <div className={'loading' + (this.state.loading ? ' show' : '')}>
            <div className="spinner" title="Loading..."></div>
          </div>
          <RecipeDisplay recipeResponse={this.state.recipeResponse} />
          <Links language={this.state.language} onLanguageChange={this.handleLanguageChange} />
        </div>
      </IntlProvider>
    );
  }
}

export default App;
