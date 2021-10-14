namespace MarkDoc.Linkers.Markdown

open System
open MarkDoc.Linkers

type LinkerSettings(platform: GitPlatform) =
  member val Platform = platform with get, set
  
  interface ILinkerSettings with
    member this.Description = "Settings for creating links between documentation pages"
    member this.Id = Guid("1B9469FD-41F5-4FDD-B7CB-40D971F6F418")
    member this.Name = "Linker settings"