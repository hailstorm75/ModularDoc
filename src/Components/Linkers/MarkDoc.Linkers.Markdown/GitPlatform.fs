namespace MarkDoc.Linkers.Markdown

open System.ComponentModel

/// <summary>
/// Git platform types
/// </summary>
type GitPlatform =
  /// <summary>
  /// GitHub platform
  /// </summary>
  | [<Description("GitHub")>]       GitHub = 0
  /// <summary>
  /// GitLab platform
  /// </summary>
  | [<Description("GitLab")>]       GitLab = 1
  /// <summary>
  /// Azure platform
  /// </summary>
  | [<Description("Azure DevOps")>] Azure = 2
  /// <summary>
  /// Bitbucket platform
  /// </summary>
  | [<Description("Bitbucket")>]    BitBucket = 3 

