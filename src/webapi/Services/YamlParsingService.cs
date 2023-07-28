// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using System.Text.RegularExpressions;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using RecipeGen.Models.Responses;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace RecipeGen.Services;

public class YamlParsingService : IParsingService
{
  private readonly Regex _ignoreLinesRegex;
  private IDeserializer _deserializer;

  public YamlParsingService()
  {
    _ignoreLinesRegex = new Regex(@"^(Name:|Description:|PreparationTime:|Servings:|Parts:|\s+Ingredients:|\s+Steps:|(\s+-\s+))\s*(?<value>.*)$", RegexOptions.Multiline);
    _deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
  }

  public Recipe ParseRecipeResponse(string response)
  {
    if (string.IsNullOrWhiteSpace(response)) { throw new ArgumentException("response is null or whitespace"); }

    response = FilterYaml(response);
    Recipe result;
    try
    {
      result = _deserializer.Deserialize<Recipe>(response);
    }
    catch (YamlException ex)
    {
      throw new ParsingException("Could not parse response", ex);
    }

    if (result == null ||
        string.IsNullOrEmpty(result.Name) ||
        string.IsNullOrEmpty(result.Description) ||
        result.Parts.Count < 1)
    {
      throw new ParsingException("Could not parse response");
    }

    foreach (var part in result.Parts)
    {
      if (string.IsNullOrEmpty(part.Name) ||
          part.Ingredients.Count < 1 ||
          part.Steps.Count < 1)
      {
        throw new ParsingException("Could not parse response");
      }
    }

    return result;
  }

  private string FilterYaml(string input)
  {
    var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    List<string> filteredLines = new List<string>();

    foreach (var line in lines)
    {
      if (_ignoreLinesRegex.IsMatch(line))
      {
        filteredLines.Add(line);
      }
    }

    return string.Join("\n", filteredLines);
  }
}