namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Members
open MarkDoc.Elements
open MarkDoc.Helpers
open System
open MarkDoc.Members.Types
open MarkDoc.Documentation.Tags

type ContentType =
  | Summary
  | Remarks
  | SeeAlso
  | Example
  | Value
  | Returns
  | Exceptions
  | Arguments
  | Inheritance

module ContentHelpers =
  let private createHeadings headings tools = headings |> Seq.map (fun x -> TextHelpers.normal x tools)
  let private getSingleTag (input: IType) t (m: IMember) tools =
    let single tags =
      match tags with
      | Some tag -> TagHelpers.tagFull(input, tag, tools) |> Seq.map ElementHelpers.toElement |> SomeHelpers.emptyToNone
      | _ -> None
    TagHelpers.findTag(input, m, t, tools)
    |> Seq.tryExactlyOne
    |> single
  let private getExceptions (input: IType) (m: IMember) tools =
    let applyTools input = input tools
    let exceptions = TagHelpers.findTag(input, m, ITag.TagType.Exception, tools)
                     |> Seq.map (fun x -> seq [ TypeHelpers.processReference(input, x.Reference, tools); TagHelpers.tagShort(input, x, tools) ] |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement) |> Linq.ToReadOnlyCollection)

    if Seq.isEmpty exceptions then
      None
    else
      seq [ tools.creator.CreateTable(exceptions, createHeadings (seq [ "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some
  let private getSeeAlso (input: IType) (m: IMember) tools =
    let applyTools input = input tools
    let seeAlsos = TagHelpers.findTag(input, m, ITag.TagType.Seealso, tools)
                   |> Seq.map (fun x -> TagHelpers.tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)

    if Seq.isEmpty seeAlsos then
      None
    else
      seq [ tools.creator.CreateList(seeAlsos, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some
  let private getArguments (input: IType) (m: IMember) tools = 
    let applyTools input = input tools
    let argumentDocs = TagHelpers.findTag(input, m, ITag.TagType.Param, tools)
                       |> Seq.map (fun x -> x.Reference, TagHelpers.tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)
                       |> dict

    let processArguments (x: IArgument) =
      let getDescription =
        let mutable value: IElement = null
        seq [ if argumentDocs.TryGetValue(x.Name, &value) then yield value ]

      let argType = 
        let argType = seq [
          let argType = x |> StringConverters.argumentTypeStr
          if not (String.IsNullOrEmpty argType) then
            yield argType |> InlineCode
        ]

        JoinedText (seq [ TypeHelpers.processResType input x.Type tools ] |> Seq.append argType, " ")

      let typeName = seq [ argType; x.Name |> Normal ]
                     |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)
      getDescription
      |> Seq.append typeName
      |> Linq.ToReadOnlyCollection

    let generateResult (args: IArgument seq) =
      let arguments = args
                      |> Seq.map processArguments
      if (Seq.isEmpty args || Seq.isEmpty arguments) then
        None
      else
        seq [ tools.creator.CreateTable(arguments, createHeadings (seq [ "Type"; "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some

    match m with
    | :? IConstructor as x -> generateResult x.Arguments
    | :? IDelegate as x -> generateResult x.Arguments
    | _ -> raise (Exception())
  let private getInheritedFrom (input: IType) (m: IMember) tools = 
    let applyTools input = input tools
    let getInheritance(x: IInterface) =
      let typeReference (t: IType) = 
        (TypeHelpers.getTypeName t |> Normal, tools.linker.CreateLink(input, t)) |> LinkElement

      let mutable result: IInterface = null
      if x.InheritedTypes.Value.TryGetValue(m, &result) then
        Some(seq [ typeReference result |> ElementHelpers.initialize |> applyTools ])
      else
        None

    match input with
    | :? IInterface as i -> getInheritance i
    | _ -> None

  let processContents content input =
    let processContent content =
      match content with
      | Summary as s -> (getSingleTag input ITag.TagType.Summary, "Summary")
      | Remarks as s -> (getSingleTag input ITag.TagType.Remarks, "Remarks")
      | Example as s -> (getSingleTag input ITag.TagType.Example, "Example")
      | Value as s -> (getSingleTag input ITag.TagType.Value, "Value")
      | Returns as s -> (getSingleTag input ITag.TagType.Returns, "Returns")
      | Exceptions as s -> (getExceptions input, "Exceptions")
      | SeeAlso as s -> (getSeeAlso input, "See also")
      | Arguments as s -> (getArguments input, "Arguments")
      | Inheritance as s -> (getInheritedFrom input, "Inherited from")

    content |> Seq.map processContent
