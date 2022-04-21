namespace MarkDoc.Generator.Basic

open System
open FSharpPlus
open MarkDoc.Generator
open MarkDoc.Generator.Basic
open MarkDoc.Helpers
open MarkDoc.Members.Types
open MarkDoc.Elements
open MarkDoc.Documentation.Tags

/// <summary>
/// Basic type composer
/// </summary>
/// <param name="creator">Injected element creator</param>
/// <param name="docResolver">Injected documentation resolver</param>
/// <param name="memberResolver">Injected member resolver</param>
/// <param name="linker">Injected linker</param>
type TypeComposer(creator, docResolver, memberResolver, linker, diagramResolver) =
  let m_tools: Tools = { linker = linker; creator = creator; docResolver = docResolver; typeResolver = memberResolver; diagramResolver = diagramResolver }

  let composeSections input level =
    let createSection (x, y) = Section(x, y, level)

    input
    // Filter out invalid items
    |> SomeHelpers.whereSome2
    // Compose the sections
    |> Seq.map (createSection >> ElementHelpers.initialize)

  let printIntroduction (input: IType) tools =
    // If there is a summary for the given type..
    match TagHelpers.findTypeTag input ITag.TagType.Summary tools |> Seq.tryExactlyOne with
    // print the summary
    | Some x -> Some(seq [ ElementHelpers.initialize (TagHelpers.tagShort input x tools |> TextElement) ])
    // otherwise return no summary
    | None -> None
    
  let printDiagram (input: IType) (tools: Tools) =
    let mutable result: struct (string * string) = ("", "");
    if tools.diagramResolver.TryGenerateDiagram(input, &result) then
      Some (seq [ result.ToTuple() |> DiagramElement |> ElementHelpers.initialize ])
    else
      None

  let printMemberTables = TypeContentHelpers.processTableOfContents

  let printDetailed (input: IType) tools =
    let sections =
      // Define the section types to be included in the documentation
      seq [
        TypeSummary;
        TypeRemarks;
        TypeExample;
        TypeGenerics;
        TypeInheritance;
        TypeNested;
        TypeSeeAlso;
        TypeConstructors;
        TypeMethods;
        TypeProperties;
        TypeEvents;
        TypeDelegates;
        TypeFields
      ]
      // Process the section types to generate content
      |> TypeContentHelpers.processContents input tools
      // Compose the content into sections
      |> composeSections

    // Set the sections level to 2
    sections 2
    // If there are no sections return nothing
    |> SomeHelpers.emptyToNone

  let composeLink =
    let link = ("MarkDoc" |> Italic, "https://github.com/hailstorm75/MarkDoc.Core") |> LinkElement
    let text = "Generated with" |> Italic |> TextElement
    let res tools = tools.creator.JoinTextContent([ text; link ] |> Seq.map (fun x -> ElementHelpers.initialize x tools :?> ITextContent), " ") :> IElement
    seq [ res ] |> Some

  let applyTools input = input m_tools
  let composeContent input tools =

    // Get the content
    let content = seq [
                    (printIntroduction input tools, "Description");
                    (printDiagram input tools, "Diagram");
                    (printMemberTables input tools, "Members");
                    (printDetailed input tools, "Details")
                    (composeLink, "");
                  ]
                  // Filter out invalid items
                  |> SomeHelpers.whereSome2
                  |> Seq.map (fun (x, y) -> (x |> Seq.map applyTools |> Some, y))
    // Compose the content
    composeSections content 1 |> Seq.map applyTools
    
  let getTypeName (input: IType) =
    let name = TypeHelpers.getTypeName input
    let tName = match input with
                | :? IStruct as x ->
                  if x.IsReadOnly then "readonly struct" else "struct"
                | :? IRecord -> "record"
                | :? IClass -> "class"
                | :? IInterface -> "interface"
                | :? IEnum -> "enum"
                | _ -> ""
                |> InlineCode
                |> TextHelpers.processText
                <| m_tools

    let content = tName.Print () |> Seq.head
    name + " " + content
    
  let lift (x: 'T option) (f: 'T -> 'U option): 'U option =
    match x with
    | Some v -> f v
    | None -> None

  let composeTableOfContents =
    let rec createList (node: TrieNamespace.TrieNode) (space: string): Element<#ITextContent> option =
      let types = m_tools.typeResolver.Types.Value;
      let subLists = match node.Nodes with
                     | sequence when Seq.isEmpty sequence -> None
                     | sequence
                       -> Some(seq {
                            for item in sequence do
                              let newSpace = String.Join(".", seq { space; item.Key } |> Seq.filter (String.IsNullOrEmpty >> not))
                              if types.ContainsKey(space) then
                                for t in types.[space] |> Seq.filter (fun x -> not x.IsNested) do
                                  yield (t.Name |> Italic, m_tools.linker.CreateLink(t)) |> LinkElement |> ElementHelpers.initialize <| m_tools |> Some

                              yield item.Key |> Normal |> TextElement |> ElementHelpers.initialize <| m_tools |> Some
                              yield lift (createList item.Value newSpace) (ElementHelpers.initialize >> applyTools >> Some)
                          })
      match subLists with
      | Some list -> ListElement(list |> SomeHelpers.whereSome, IList.ListType.Dotted, "", 0) |> Some
      | None -> None

    let result = lift (createList memberResolver.Namespaces.Value.Root "") (ElementHelpers.initialize >> applyTools >> Some)
    seq { result }

  interface ITypeComposer with
    /// <inheritdoc />
    member _.ComposeTableOfContents() =
      let page = Page(composeTableOfContents |> SomeHelpers.whereSome, "Table of contents", 0)
      ElementHelpers.initialize page m_tools :?> IPage

    /// <inheritdoc />
    member _.Compose input =
      // If the input is null..
      if (isNull input) then
        // throw an exception
        raise (ArgumentNullException("input"))
      // Otherwise..
      else
        // compose the page
        let page = Page(composeContent input m_tools, getTypeName input, 0)
        // return the composed type page
        ElementHelpers.initialize page m_tools :?> IPage