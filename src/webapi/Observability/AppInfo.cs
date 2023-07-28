// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT
// Copyright (c) 2023 Drew Robbins
// SPDX-License-Identifier: MIT

using System.Reflection;

namespace RecipeGen.Observability;

public class AppInfo
{
  private const string UNKNOWN = "unknown";

  public string Name { get; }
  public string Version { get; }
  public string Revision { get; }
  public string BuildTime { get; }

  public AppInfo(string name, string version, string revision, string buildTime)
  {
    Name = name;
    Version = version;
    Revision = revision;
    BuildTime = buildTime;
  }

  internal static AppInfo GetAppInfo()
  {
    var entryAssembly = Assembly.GetEntryAssembly();

    var appName = entryAssembly?.GetName().Name ?? UNKNOWN;

    var versionInfo = Attribute.GetCustomAttribute(entryAssembly!, typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
    var version = versionInfo?.InformationalVersion ?? UNKNOWN;

    var parts = version.Split('-');
    version = parts[0];
    var revision = parts.Length > 1 ? parts[1] : UNKNOWN;
    var buildTime = parts.Length > 2 ? parts[2] : UNKNOWN;

    return new AppInfo(appName, version, revision, buildTime);
  }
}
