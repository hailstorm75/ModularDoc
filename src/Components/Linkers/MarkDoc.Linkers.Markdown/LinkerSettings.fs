namespace MarkDoc.Linkers.Markdown

open System
open System.Collections.Generic
open MarkDoc.Linkers

type LinkerSettings(data: IReadOnlyDictionary<string, string>) =
  member val Platform = data.["platform"] with get, set
  static member val ENTRY_PLATFORM = "platform"
  
  interface ILinkerSettings with
    member this.Id = Guid("1B9469FD-41F5-4FDD-B7CB-40D971F6F418")