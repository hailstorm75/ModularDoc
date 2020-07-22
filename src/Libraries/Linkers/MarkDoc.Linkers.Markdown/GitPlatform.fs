namespace MarkDoc.Linkers.Markdown

open System.ComponentModel

type GitPlatform =
  | [<Description("GitHub")>]       GitHub = 0
  | [<Description("GitLab")>]       GitLab = 1
  | [<Description("Azure DevOps")>] Azure = 2
  | [<Description("Bitbucket")>]    BitBucket = 3 

