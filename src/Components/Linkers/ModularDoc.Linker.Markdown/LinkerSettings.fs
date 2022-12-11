namespace ModularDoc.Linker.Markdown

open System
open System.Collections.Generic
open ModularDoc.Linker

type LinkerSettings(data: IReadOnlyDictionary<string, string>) =
  member val Platform = data.[LinkerSettings.ENTRY_PLATFORM] with get, set
  member val GitPlatformUser = data.[LinkerSettings.PLATFORM_USER] with get, set
  member val GitPlatformBranch = data.[LinkerSettings.PLATFORM_BRANCH] with get, set
  member val GitPlatformRepository = data.[LinkerSettings.PLATFORM_REPOSITORY] with get, set
  member val LinksToSourceCodeEnabled = data.[LinkerSettings.ENABLE_LINKS_TO_SOURCE] with get, set
  member val OutputTargetWiki = data.[LinkerSettings.OUTPUT_TARGET_WIKI] with get, set
  member val OutputStructured = data.[LinkerSettings.OUTPUT_STRUCTURED] with get, set

  static member val ENTRY_PLATFORM = "platform"
  static member val PLATFORM_USER = "platformUser"
  static member val PLATFORM_BRANCH = "platformBranch"
  static member val PLATFORM_REPOSITORY = "rootRepo"
  static member val ENABLE_LINKS_TO_SOURCE = "toggleLinksToSource"
  static member val OUTPUT_TARGET_WIKI = "outputTargetWiki"
  static member val OUTPUT_STRUCTURED = "outputStructured"
  
  interface ILinkerSettings with
    member this.Id = Guid("1B9469FD-41F5-4FDD-B7CB-40D971F6F418")