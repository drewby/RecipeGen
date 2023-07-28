// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
using RecipeGen.Constants;
using RecipeGen.Models;
using RecipeGen.Models.Exceptions;
using RecipeGen.Models.Requests;
using YamlDotNet.Serialization;

namespace RecipeGen.Services;

/// <summary>
/// A service that provides prompts for the language model by
/// loading a template from a file. The template will contain
/// a {{RecipeRequest}} placeholder that will be replaced
/// with RecipeRequest serialized as YAML.
public class FilePromptService : IPromptService
{
  private readonly ILogger<FilePromptService> _logger;
  private readonly IConfiguration _configuration;
  private readonly string? _promptFile;
  private readonly string _template;
  private readonly ISerializer _serializer;

  public FilePromptService(ILogger<FilePromptService> logger, IConfiguration configuration)
  {
    _logger = logger;
    _configuration = configuration;

    _promptFile = _configuration[ConfigKeys.PromptFile];
    if (string.IsNullOrWhiteSpace(_promptFile))
    {
      throw new ConfigException($"{ConfigKeys.PromptFile} is not configured.");
    }
    if (!File.Exists(_promptFile))
    {
      throw new ConfigException($"{ConfigKeys.PromptFile} '{_promptFile}' does not exist.");
    }

    _template = File.ReadAllText(_promptFile);

    _serializer = new SerializerBuilder().Build();
  }

  public Prompt GetPrompt(RecipeRequest request)
  {
    request = request ?? throw new ArgumentNullException(nameof(request));

    var yaml = _serializer.Serialize(request);

    string prompt;
    prompt = _template.Replace(PromptKeys.PlaceHolder, yaml, StringComparison.Ordinal);

    const string system = "\nSystem:\n";

    // look for a line "System:" and split the prompt into two parts
    // if it exists
    var systemIndex = prompt.IndexOf(system, StringComparison.Ordinal);
    string? systemMessage = null;
    if (systemIndex > 0)
    {
      systemMessage = prompt.Substring(systemIndex + system.Length);
      prompt = prompt.Substring(0, systemIndex);
    }

    return new Prompt(_promptFile!, prompt, systemMessage);
  }
}
