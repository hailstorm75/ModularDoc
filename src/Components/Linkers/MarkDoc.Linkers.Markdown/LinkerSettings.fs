namespace MarkDoc.Linkers.Markdown

open MarkDoc.Linkers

type LinkerSettings(platform: GitPlatform) =
  member val Platform = platform with get, set
  
  interface ILinkerSettings with