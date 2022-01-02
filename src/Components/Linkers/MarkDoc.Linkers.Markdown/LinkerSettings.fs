namespace MarkDoc.Linkers.Markdown

open System
open System.Collections.Generic
open MarkDoc.Linkers

type LinkerSettings(data: IReadOnlyDictionary<string, string>) =
  member val Platform = data.[LinkerSettings.ENTRY_PLATFORM] with get, set
  member val GitPlatformUser = data.[LinkerSettings.PLATFORM_USER] with get, set
  member val GitPlatformBranch = data.[LinkerSettings.PLATFORM_BRANCH] with get, set
  member val GitPlatformRepository = data.[LinkerSettings.PLATFORM_REPOSITORY] with get, set

  static member val ENTRY_PLATFORM = "platform"
  static member val PLATFORM_USER = "platformUser"
  static member val PLATFORM_BRANCH = "platformBranch"
  static member val PLATFORM_REPOSITORY = "rootRepo"
  
  interface ILinkerSettings with
    member this.Id = Guid("1B9469FD-41F5-4FDD-B7CB-40D971F6F418")