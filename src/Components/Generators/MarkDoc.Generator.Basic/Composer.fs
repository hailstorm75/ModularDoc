namespace MarkDoc.Generator.Basic

open System
open MarkDoc.Generator
open MarkDoc.Members.Types
open MarkDoc.Elements
open MarkDoc.Documentation.Tags

type TypeComposer2(creator, docResolver, memberResolver, linker) = 
  let m_tools: Tools = { linker = linker; creator = creator; docResolver = docResolver; typeResolver = memberResolver }

  let composeSections input level =
    let createSection (x, y) =
      Element.Section(x, y, level)

    input
    |> SomeHelpers.whereSome2
    |> Seq.map (createSection >> ElementHelpers.initialize)

  let printIntroduction (input: IType) tools =
    match TagHelpers.findTypeTag input ITag.TagType.Summary tools |> Seq.tryExactlyOne with
    | None -> None
    | Some x -> Some(seq [ ElementHelpers.initialize (TagHelpers.tagShort input x tools |> TextElement) ])
  let printMemberTables =
    TypeContentHelpers.processTableOfContents
  let printDetailed (input: IType) tools =
    let sections =
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
      |> TypeContentHelpers.processContents input tools
      |> composeSections

    sections 2 |> SomeHelpers.emptyToNone

  let composeContent input tools =
    let applyTools input = input tools

    let content = seq [
                    (printIntroduction input tools, "Description");
                    (printMemberTables input tools, "Members");
                    (printDetailed input tools, "Details")
                  ]
                  |> SomeHelpers.whereSome2
                  |> Seq.map (fun (x, y) -> (x |> Seq.map applyTools |> Some, y))
    composeSections content 1
    |> Seq.map applyTools

  interface ITypeComposer with
    /// <inheritdoc />
    member __.Compose input =
      // If the input is null..
      if (isNull input) then
        // throw an exception
        raise (ArgumentNullException("input"))
      // Otherwise..
      else
        // return the composed type page
        let page = Page(composeContent input m_tools, TypeHelpers.getTypeName input, 0)
        ElementHelpers.initialize page m_tools :?> IPage