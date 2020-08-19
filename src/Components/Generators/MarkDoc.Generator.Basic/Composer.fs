namespace MarkDoc.Generator.Basic

open System
open MarkDoc.Generator
open MarkDoc.Members.Types
open MarkDoc.Elements

type TypeComposer2(creator, docResolver, memberResolver, linker) = 
  let m_tools: Tools = { linker = linker; creator = creator; docResolver = docResolver; typeResolver = memberResolver }

  let composeSections input level =
    let createSection (x, y) =
      Element.Section(x, y, level)

    input
    |> SomeHelpers.whereSome2
    |> Seq.map (SomeHelpers.get2 >> createSection >> ElementHelpers.initialize)

  let printIntroduction input =
    Seq.empty |> Some
  let printMemberTables input =
    Seq.empty |> Some
  let printDetailed(input: IType, tools) =
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
    (seq [
       (printIntroduction input, "Description");
       (printMemberTables input, "Members");
       (printDetailed(input, tools), "Details")
     ]
     |> composeSections) 1
     |> Seq.map (fun x -> x tools)

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