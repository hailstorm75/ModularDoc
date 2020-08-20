namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Types
open MarkDoc.Elements
open MarkDoc.Documentation.Tags
open System
open MarkDoc.Members.Members
open MarkDoc.Members.Enums
open System.Collections.Generic
open MarkDoc.Members
open MarkDoc.Helpers

type TypeContentType =
  | TypeSummary
  | TypeRemarks
  | TypeExample
  | TypeGenerics
  | TypeInheritance
  | TypeNested
  | TypeSeeAlso
  | TypeConstructors
  | TypeMethods
  | TypeProperties
  | TypeEvents
  | TypeDelegates
  | TypeFields

module TypeContentHelpers =
  let private createHeadings headings tools = headings |> Seq.map (fun x -> TextHelpers.normal x tools)
  let registerSection (input, name, content) tools level =
    // Register an anchor to the given member section
    tools.linker.RegisterAnchor(input, lazy(name))
    // Initialize the section
    ElementHelpers.initialize ((content tools input, name, level) |> Section) tools

  let composeSections input level =
    let createSection (x, y) =
      Section(x, y, level)

    input
    // Filter out invalid items
    |> SomeHelpers.whereSome2
    // Create a section out of each item
    |> Seq.map (createSection >> ElementHelpers.initialize)
  let methodArguments source (item: IMember) tools =
    let argument arg =
      // Compose the signature of a single argument
      JoinedText ([ arg |> StringConverters.argumentTypeStr |> Normal; TypeHelpers.processResType source arg.Type tools; Normal arg.Name ], " ")
    let processArguments args =
      // Compose arguments together 
      JoinedText (args |> Seq.map argument, ", ")

    // Process arguments for types which can have them
    match item with 
    | :? IConstructor as c -> c.Arguments |> processArguments
    | :? IDelegate as d -> d.Arguments |> processArguments
    | _ -> raise (NotSupportedException())

  let private single x input tools =
    let tag = TagHelpers.findTypeTag input x tools |> Seq.tryExactlyOne
    if Option.isNone tag then
      None
    else
      TagHelpers.tagFull input (tag |> Option.get) tools |> SomeHelpers.emptyToNone
  let private genericProcessor provider processItem tools =
    let processItems (items: 'M IReadOnlyCollection when 'M :> IMember) =
      items
      // Compose a section for every item after processing it
      |> Seq.mapi (fun x y -> registerSection (processItem(x, y)) tools 3 |> ElementHelpers.toElement)

    // If there are no items..
    if Seq.isEmpty provider then
      // return nothing
      None
    // Otherwise..
    else
      // for every item
      provider
      // process it
      |> processItems
      // if none are valid return nothing
      |> SomeHelpers.emptyToNone
  let private processContent (item: (IMember -> Tools -> IElement seq option) * string) (mem: IMember) tools =
    let applyMember input = input mem
    let applyTools input = input tools
    (item |> (fst >> applyMember >> applyTools), snd item)

  let private joinContentWSig content signature (input: IType) tools (mem: IMember) =
    let applyMember input = input mem
    let applyTools input = input tools
    let processContents (content: IType -> seq<(IMember -> Tools -> IElement seq option) * string>) =
      content input |> Seq.map processContent

    let toProcess = processContents (content |> ContentHelpers.processContents)
                    |> Seq.map (applyMember >> applyTools)

    composeSections toProcess 4
    |> Seq.map (applyTools >> ElementHelpers.toElement)
    |> Seq.append (seq [ signature |> applyMember |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement ])

  let private overloads (members: 'M IReadOnlyCollection when 'M :> IMember) =
    members
    // Group the members by their names
    |> Seq.groupBy (fun x -> x.Name)
    // Recompose the grouping to track the overload count
    |> Seq.map (fun x -> (fst x, snd x |> Seq.mapi (fun x y -> (y.RawName, x)) |> dict))
    // Materialize the sequence to a dictionary
    |> dict
  let private overloadFormat overloadCount overloadIndex =
    // If there are overloads..
    if overloadCount > 1 then
      // then print them
      String.Format(" [{0}/{1}]", overloadIndex + 1, overloadCount)
    // Otherwise..
    else
      // return nothing
      String.Empty
  let private getArgumentsSig (input: IType) tools (mem: IMember) =
    methodArguments input mem tools |> TextHelpers.processTextNoStyle

  let private nested (input: IType) tools =
    let applyTools input = input tools
    let getNested (x: IInterface) = x.NestedTypes

    let groupByType (x: IType) =
      match x with
      | :? IClass
        -> "c" |> Some
      | :? IStruct
        -> "s" |> Some
      | :? IInterface
        -> "i" |> Some
      | :? IEnum
        -> "e" |> Some
      | _ -> None

    let processGroup (x: string option * seq<IType>) =
      let createTable heading group =
        ListElement (group |> Seq.map (fun x -> ElementHelpers.initialize (TypeHelpers.getTypeName x |> InlineCode |> TextElement) tools), IList.ListType.Dotted, heading, 3)
        |> Some

      match x |> (fst >> Option.get) with
      | "c" -> createTable "Classes" (snd x)
      | "i" -> createTable "Interfaces" (snd x)
      | "s" -> createTable "Structures" (snd x)
      | "e" -> createTable "Enums" (snd x)
      | _ -> None

    match input with
    | :? IInterface as x ->
       x
       |> getNested
       |> Seq.groupBy groupByType
       |> Seq.filter (fst >> Option.isSome)
       |> Seq.map processGroup
       |> SomeHelpers.whereSome
       |> Seq.map (ElementHelpers.initialize >> applyTools)
       |> SomeHelpers.emptyToNone
    | _ -> None

  let private inheritance (input: IType) tools =
    let getInterfaces (x: 'M when 'M :> IInterface) =
      x.InheritedInterfaces
      |> Seq.map (fun x -> ElementHelpers.initialize (TypeHelpers.processResType input x tools |> TextElement) tools)

    let createList l =
      if (Seq.isEmpty l) then
        None
      else
        seq [ tools.creator.CreateList(l, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some

    match input with
    | :? IClass as x ->
      let baseType = if (isNull x.BaseClass) then None else ElementHelpers.initialize (TypeHelpers.processResType input x.BaseClass tools |> TextElement) tools |> Some
      let interfaces = getInterfaces x
      seq [ baseType ]
      |> SomeHelpers.whereSome
      |> Seq.append interfaces
      |> createList
    | :? IInterface as x ->
      x |> (getInterfaces >> createList)
    | _ -> None

  let private typeParams (input: IType) tools =
    let applyTools input = input tools
    let getTypeParams =
      let processTag (x: ITag) =
        let generics = (input :?> IInterface).Generics
        let getConstraints (x: ITag) =
          if generics.ContainsKey(x.Reference) then
            let types = generics.[x.Reference].ToTuple()
                        |> snd
                        |> Seq.map (fun x -> TypeHelpers.processResType input x tools)
            (types, Environment.NewLine) |> JoinedText |> Some
          else
            None

        let getName (x: ITag) =
          let result = seq [
            yield x.Reference |> InlineCode
            if (generics.ContainsKey(x.Reference)) then
              let variance = generics.[x.Reference].ToTuple() |> fst
              if (variance <> Enums.Variance.NonVariant) then
                yield variance |> StringConverters.varianceStr |> InlineCode
          ]
          (result, " ") |> JoinedText

        let constraints = getConstraints x
        seq [
          yield getName x
          yield TagHelpers.tagShort input x tools

          if (Option.isSome constraints) then
            yield constraints |> Option.get
        ]
        |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)

      if input :? IInterface then
        TagHelpers.findTypeTag input ITag.TagType.Typeparam tools
        |> Seq.map (processTag >> Linq.ToReadOnlyCollection)
        |> Some
      else
        None

    let ts = getTypeParams
    if (Option.isSome ts && ts |> (Option.get >> Seq.isEmpty >> not)) then
      seq [ tools.creator.CreateTable(ts |> Option.get, createHeadings (seq [ "Type"; "Description"; "Constraints" ]) tools) |> ElementHelpers.toElement ] |> Some
    else
      None

  let private constructors (input: IType) tools =
    let extractor =
      match input with
      | :? IClass as x -> x.Constructors
      | _ -> LinkedList<IConstructor>() :> IReadOnlyCollection<IConstructor>
    let processCtor (i: int, ctor: IConstructor) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1} {2}({3})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getName;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Exceptions;
          SeeAlso
        ]

      (ctor, ctor.Name + overloadFormat extractor.Count i, joinContentWSig content signature input)

    genericProcessor extractor processCtor tools

  let private methods (input: IType) tools =
    let extractor =
      match input with
      | :? IInterface as x -> x.Methods
      | _ -> new LinkedList<IMethod>() :> IReadOnlyCollection<IMethod>
    let processMethod (_, method: IMethod) =
      let getOverloads = 
        let overloads = (overloads extractor).[method.Name]
        overloadFormat overloads.Count overloads.[method.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} {3}{4}{5} {6}{7}({8})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getAsync;
          SignatureHelpers.getReturn;
          SignatureHelpers.getOperator;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Returns;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (method, (if method.Operator <> OperatorType.None then "Operator " else "") + method.Name + getOverloads, joinContentWSig content signature input)

    genericProcessor extractor processMethod tools

  let private properties (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Properties
      | _ -> new LinkedList<IProperty>() :> IReadOnlyCollection<IProperty>
    let processProperty (_, property: IProperty) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} {3} {4} {{ {5} }}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getPropertyMethods
        ])
      let content =
        seq [
          Summary;
          Remarks;
          Value;
          Example;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (property, property.Name, joinContentWSig content signature input)

    genericProcessor extractor processProperty tools

  let private events (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Events
      | _ -> new LinkedList<IEvent>() :> IReadOnlyCollection<IEvent>
    let processEvent (_, event: IEvent) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1} event {2} {3}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName
        ])
      let content =
        seq [
          Summary;
          Remarks;
          Example;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (event, event.Name, joinContentWSig content signature input)

    genericProcessor extractor processEvent tools

  let private delegates (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Delegates
      | _ -> new LinkedList<IDelegate>() :> IReadOnlyCollection<IDelegate>
    let processDelegate (_, deleg: IDelegate) =
      let getOverloads = 
        let overloads = (overloads extractor).[deleg.Name]
        overloadFormat overloads.Count overloads.[deleg.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1} delegate {2} {3}{4}({5})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Returns;
          Inheritance;
          SeeAlso
        ]

      (deleg, deleg.Name + getOverloads, joinContentWSig content signature input)

    genericProcessor extractor processDelegate tools

  let private enumFields (input: IType) tools =
    let extractor =
      match input with
      | :? IEnum as e -> e.Fields
      | _ -> new LinkedList<IEnumField>() :> IReadOnlyCollection<IEnumField>
    let processField (_, field: IEnumField) =
      let applyTools input = input tools
      let content =
        let sections =
          seq [
            Summary;
            Remarks;
            Example;
            Returns;
            SeeAlso
          ]
        let processed = ContentHelpers.processContents sections input
                        |> Seq.map (fun x -> (processContent x field) tools)

        composeSections processed 4
        |> Seq.map applyTools

      let intermediate tools _ =
        (if Seq.isEmpty content then seq [ TextHelpers.empty tools |> ElementHelpers.toElement ] else content)

      (field, field.Name, intermediate)

    genericProcessor extractor processField tools

  let processContents input tools content =
    let processContent content =
      match content with
      | TypeConstructors as s -> (constructors, "Constructors")
      | TypeInheritance as s -> (inheritance, "Inheritance")
      | TypeProperties as s -> (properties, "Properties")
      | TypeDelegates as s -> (delegates, "Delegates")
      | TypeGenerics as s -> (typeParams, "Generic types")
      | TypeSeeAlso as s -> (single ITag.TagType.Seealso, "See also")
      | TypeSummary as s -> (single ITag.TagType.Summary, "Summary")
      | TypeRemarks as s -> (single ITag.TagType.Remarks, "Remarks")
      | TypeExample as s -> (single ITag.TagType.Example, "Example")
      | TypeMethods as s -> (methods, "Methods")
      | TypeNested as s -> (nested, "Nested types")
      | TypeEvents as s -> (events, "Events")
      | TypeFields as s -> (enumFields, "Fields")

    content
    // Process every case and append the result
    |> Seq.map (processContent >> fun (c, l) -> (c input tools, l))

  let processTableOfContents (input: IType) tools =
    let applyTools input = input tools
    let memberNameSummary name (summary: ITag option) =
      // If the given member has a tag..
      match summary with
      // print its name with the tag
      | Some x -> JoinedText (seq [ name; TagHelpers.tagShort input x tools ], Environment.NewLine)
      // otherwise print the name only
      | None -> name

    let processInterface (input: IInterface) =
      let sectionHeading isStatic accessor section =
        String.Join(" ", seq [ StringConverters.accessorStr accessor; StringConverters.staticStr isStatic; section ])

      let createContent (members: seq<'M> when 'M :> IMember) newRow =
        members
        // Group members by their names
        |> Seq.groupBy SignatureHelpers.getName
        // Sort the groups by the name keys
        |> Seq.sortBy fst
        // Flatten the grouped items
        |> Seq.collect snd
        // Materialize a new row
        |> Seq.map newRow

      let createPropertySection(isStatic, accessor, properties: seq<IProperty>) =
        let createRow(property: IProperty) =
          // Get the property name
          let processName =
            // Compose the property signature
            let anchor = LinkContent(InlineCode property.Name, tools.linker.CreateAnchor(input, property))
            // Join the signature with the property summary
            memberNameSummary anchor ((TagHelpers.findTag input property ITag.TagType.Summary tools) |> Seq.tryExactlyOne)

          // Compose a sequence of property signature parts and for each part..
          seq [ TypeHelpers.processResType input property.Type tools; processName; InlineCode (SignatureHelpers.getPropertyMethods property) ]
          // Initialize each part
          |> Seq.map (TextElement >> ElementHelpers.initialize >> applyTools)
          // Materialize the sequence to a collection
          |> Linq.ToReadOnlyCollection

        // Get the content for the table
        let content = createContent properties createRow

        // If there are no properties..
        if (Seq.isEmpty properties) then
          // return nothing
          None
        // Otherwise..
        else
          // create a table of properties
          Table(content, TextHelpers.createHeadings (seq [ "Type"; "Name"; "Methods" ]) tools, sectionHeading isStatic accessor "properties", 3)
          |> Some

      let createMethodSection(isStatic, accessor, methods: seq<IMethod>) =
        let methodsArray = methods |> Seq.toArray

        let createRow (method: IMethod) =
          // Get the method return type
          let processReturn =
            // Get the method return type name
            let name = if isNull method.Returns then "void" else method.Returns.DisplayName
            // Stylize the type name
            let content = InlineCode name
            // If the return type is void..
            if isNull method.Returns then
              // return it as is
              content
            // Otherwise..
            else
              let a = input :> IType
              let b = method.Returns
              // Try to get reference to the return type
              let link = tools.linker.CreateLink(a, b)
              // If there is no reference..
              if String.IsNullOrEmpty link then
                // return the type name as is
                content
              // Otherwise..
              else
                // wrap the type name in a link to the reference
                LinkContent(content, lazy(link))

          // Get the method signature and description
          let processMethod =
            // Get whether the given method has overloads
            let hasOverloads =
              methodsArray
              // Find methods of the same name
              |> Seq.where(fun x -> x.Name = method.Name)
              // Exclude one to account for this method
              |> Seq.skip 1
              // Check if any methods are left
              |> (Seq.isEmpty >> not)

            // Get the method signature
            let signature =
              seq [
                // Depending on whether the method is an operator and what operator type it is
                // return signature keywords
                match method.Operator with
                | OperatorType.Explicit ->
                  yield InlineCode "explicit"
                  yield Normal " "
                  yield InlineCode "operator"
                  yield Normal " "
                | OperatorType.Implicit ->
                  yield InlineCode "implicit"
                  yield Normal " "
                  yield InlineCode "operator"
                  yield Normal " "
                | OperatorType.Normal ->
                  yield InlineCode "operator"
                  yield Normal " "
                | _ -> ()

                // Print the method name with an anchor to the detailed method information
                yield LinkContent(InlineCode method.Name, tools.linker.CreateAnchor(input, method))

                // Beging arguments
                yield Normal "("

                // If there are overloads do not print the arguments
                if hasOverloads then yield InlineCode "..."
                // Otherwise print the method arguments
                else yield methodArguments input method tools

                // End arguments
                yield Normal ")"
              ]

            // Compose the method signature
            let signatureText = JoinedText (signature, "")
            // Join the signature with the method summary
            memberNameSummary signatureText ((TagHelpers.findTag input method ITag.TagType.Summary tools) |> Seq.tryExactlyOne)

          seq [ processReturn; processMethod ]
          // Initialize all the text
          |> Seq.map (fun item -> ElementHelpers.initialize (item |> TextElement) tools)
          // Materialize the sequence to a collection
          |> Linq.ToReadOnlyCollection

        // Get the content for the table
        let content = createContent (methodsArray |> Seq.distinctBy(fun x -> x.Name)) createRow

        // If there are no methods..
        if (Seq.isEmpty methods) then
          // return nothing
          None
        // Otherwise..
        else
          // create a table of methods
          Table(content, TextHelpers.createHeadings (seq [ "Returns"; "Name" ]) tools , sectionHeading isStatic accessor "methods", 3)
          |> Some

      let flattenMembers item =
        let flatten item =
          let a = fst item
          snd item
          |> Seq.map(fun x -> (a, fst x, snd x))
        item
        // Flatten the grouping
        |> Seq.collect (flatten >> id)

      let createTable members create =
        let groupMembers (members: seq<'M> when 'M :> IMember) =
          let byStatic(x: 'M) = x.IsStatic
          let byAccessor (x: bool * seq<'M>) = (fst x, snd x |> Seq.groupBy(fun y -> y.Accessor))

          members
          // Group members by whether they are static
          |> Seq.groupBy byStatic
          // Group members by their accessor
          |> Seq.map byAccessor
        members
        // Group the members
        |> groupMembers
        // Flatten the members with their respective keys
        |> flattenMembers
        // Create the content tables
        |> Seq.map create
        // Filter out the invalid items
        |> SomeHelpers.whereSome

      seq [
        (createTable input.Properties createPropertySection, "Properties");
        (createTable input.Methods createMethodSection, "Methods");
      ]
      // Filter out empty collections
      |> Seq.filter (fst >> Seq.isEmpty >> not)
      // Create sections of tables
      |> Seq.map(fun x -> Section(x |> fst |> Seq.map (ElementHelpers.initialize >> applyTools), snd x, 2) |> ElementHelpers.initialize)

    // If the input type is..
    match input with
    // an interface, then process it
    | :? IInterface as x -> processInterface x
    // anything else, then return nothing
    | _ -> Seq.empty
    // Return none if the processing generated nothing
    |> SomeHelpers.emptyToNone

