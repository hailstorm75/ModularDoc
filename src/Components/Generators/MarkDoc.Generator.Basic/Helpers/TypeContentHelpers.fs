namespace MarkDoc.Generator.Basic

open MarkDoc.Documentation.Tags
open MarkDoc.Members.Members
open MarkDoc.Members.Types
open MarkDoc.Members.Enums
open MarkDoc.Elements
open MarkDoc.Helpers
open System.Collections.Generic
open System

/// <summary>
/// Documentation section types for types
/// </summary>
type TypeContentType =
  /// <summary>
  /// Type summary documentation
  /// </summary>
  | TypeSummary
  /// <summary>
  /// Type remarks documentation
  /// </summary>
  | TypeRemarks
  /// <summary>
  /// Type example documentation
  /// </summary>
  | TypeExample
  /// <summary>
  /// Type generics and their constraints documentation
  /// </summary>
  | TypeGenerics
  /// <summary>
  /// Type inherited types documentation
  /// </summary>
  | TypeInheritance
  /// <summary>
  /// Type nested types documentation
  /// </summary>
  | TypeNested
  /// <summary>
  /// Type additional references
  /// </summary>
  | TypeSeeAlso
  /// <summary>
  /// Type constructors documentation
  /// </summary>
  | TypeConstructors
  /// <summary>
  /// Type methods documentation
  /// </summary>
  | TypeMethods
  /// <summary>
  /// Type properties documentation
  /// </summary>
  | TypeProperties
  /// <summary>
  /// Type events documentation
  /// </summary>
  | TypeEvents
  /// <summary>
  /// Type delegates documentation
  /// </summary>
  | TypeDelegates
  /// <summary>
  /// Enum fields documentation
  /// </summary>
  | TypeFields

module TypeContentHelpers =
  /// <summary>
  /// Registers the given <paramref name="input"/> member and composes its documentation section
  /// </summary>
  /// <param name="input">Member to register</param>
  /// <param name="name">Member name</param>
  /// <param name="content">Member documentation content composer</param>
  /// <param name="tools">Tools for registering and composing member sections</param>
  /// <param name="level">Sections heading level</param>
  /// <returns></returns>
  let registerSection (input, name, content) tools level =
    // Register an anchor to the given member section
    tools.linker.RegisterAnchor(input, lazy(name))
    // Initialize the section
    ElementHelpers.initialize ((content tools input, name, level) |> Section) tools

  /// <summary>
  /// Composes the given <paramref name="input"/> elements into sections
  /// </summary>
  /// <param name="input">Elements to compose into sections</param>
  /// <param name="level">Sections heading level</param>
  /// <returns>Composed sections</returns>
  let composeSections input level =
    let createSection (x, y) =
      Section(x, y, level)

    input
    // Filter out invalid items
    |> SomeHelpers.whereSome2
    // Create a section out of each item
    |> Seq.map (createSection >> ElementHelpers.initialize)

  /// <summary>
  /// Get the arguments of the given <paramref name="method"/>
  /// </summary>
  /// <param name="source">Method declaring type</param>
  /// <param name="method">Method to process</param>
  /// <param name="tools">Tools for composing method arguments</param>
  /// <returns>Composed arguments</returns>
  let methodArguments source (method: IMember) tools =
    let argument (arg: IArgument) =
      // Compose the signature of a single argument
      JoinedText ([ arg |> StringConverters.argumentTypeStr |> Normal; TypeHelpers.processResType source arg.Type tools; Normal arg.Name ], " ")
    let processArguments (args: IArgument seq) =
      // Compose arguments together 
      JoinedText (args |> Seq.map argument, ", ")

    // Process arguments for types which can have them
    match method with 
    | :? IConstructor as c -> c.Arguments |> processArguments
    | :? IDelegate as d -> d.Arguments |> processArguments
    | _ -> raise (NotSupportedException())

  let private single x input tools =
    // Get the documentation tag for given type
    let tag = TagHelpers.findTypeTag input x tools |> Seq.tryExactlyOne
    // If no tag was found..
    if Option.isNone tag then
      // return nothing
      None
    // Otherwise..
    else
      // process the tag into content
      TagHelpers.tagFull input (tag |> Option.get) tools
      // if there is no content return nothing
      |> SomeHelpers.emptyToNone
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
    // Process content and repackage
    (item |> (fst >> applyMember >> applyTools), snd item)

  let private joinContentWSig content signature (input: IType) tools (mem: IMember) =
    let applyMember input = input mem
    let applyTools input = input tools
    let processContents content = content input |> Seq.map processContent

    // Get documentation sections
    let toProcess = processContents (content |> ContentHelpers.processContents)
                    // Supply argument values
                    |> Seq.map (applyMember >> applyTools)

    // Compose documentation sections
    composeSections toProcess 4
    // Supply argument values for each section
    |> Seq.map (applyTools >> ElementHelpers.toElement)
    // Append the composed signature section
    |> Seq.append (seq [ signature |> applyMember |> applyTools |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement ])

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
    // Compose the method arguments
    methodArguments input mem tools
    // Materialize to unstylized text
    |> TextHelpers.processTextNoStyle

  let private nested (input: IType) tools =
    let applyTools input = input tools
    let getNested (input: IInterface) = input.NestedTypes

    let groupByType (itemType: IType) =
      match itemType with
      | :? IInterface ->
        match itemType with
        | :? IClass     -> "Classes"    |> Some
        | :? IStruct    -> "Structures" |> Some
        | _ -> "Interfaces" |> Some
      | :? IEnum      -> "Enums"      |> Some
      | _             -> None

    let processGroup (grouping: string * seq<IType>) =
      // Processes a type to an element
      let processType itemType = ElementHelpers.initialize (TypeHelpers.getTypeName itemType |> InlineCode |> TextElement) tools
      // Create a list from the elements
      ListElement (grouping |> snd |> Seq.map processType, IList.ListType.Dotted, fst grouping, 3) |> Some

    match input with
    | :? IInterface as inter ->
       inter
       // Get the nested types
       |> getNested
       // Group nested types
       |> Seq.groupBy groupByType
       // Filter out invalid types
       |> SomeHelpers.whereSome2
       // Compose content for each group
       |> Seq.map processGroup
       // Filter out invalid types
       |> SomeHelpers.whereSome
       // Compose content
       |> Seq.map (ElementHelpers.initialize >> applyTools)
       // If there is no content return nothing
       |> SomeHelpers.emptyToNone
    | _ -> None

  let private inheritance (input: IType) tools =
    let getInterfaces (x: 'M when 'M :> IInterface) =
      // Get the inherited interfaces
      x.InheritedTypesFlat
      // Compose the inherited types into elements
      |> Seq.map (fun x -> ElementHelpers.initialize (TypeHelpers.processResType input x tools |> TextElement) tools)

    let createList elements =
      // If there are no elements..
      if (Seq.isEmpty elements) then
        // return nothing
        None
      // Otherwise..
      else
        // compose a list
        seq [ tools.creator.CreateList(elements, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some

    // Process the input based on its type
    match input with
    | :? IClass as classType ->
      // Get the inherited base class
      let baseType =
        // If there is no base class..
        if (isNull classType.BaseClass) then
          // return nothing
          None
        // Otherwise..
        else
          // compose the base class element
          ElementHelpers.initialize (TypeHelpers.processResType input classType.BaseClass tools |> TextElement) tools |> Some

      // Get the inherited interfaces
      let interfaces = getInterfaces classType

      seq [ baseType ]
      // Exclude the base class if it is missing
      |> SomeHelpers.whereSome
      // Append the interfaces to the inherited types sequence
      |> Seq.append interfaces
      // Compose the list of elements
      |> createList
    | :? IInterface as interfaceType ->
      interfaceType
      // Compose the list of elemets
      |> (getInterfaces >> createList)
    | _ -> None

  let private typeParams (input: IType) tools =
    let applyTools input = input tools
    // Get the type generics
    let getTypeParams =
      let processTag (tag: ITag) =
        // Get the type generics
        let generics = (input :?> IInterface).Generics
        let getConstraints (tag: ITag) =
          // If the given tag references an existing generic type..
          if generics.ContainsKey(tag.Reference) then
            // Get the generic constraints
            let types = generics.[tag.Reference].ToTuple()
                        // Get the generics constraint types
                        |> snd
                        // Compose generic type constraints
                        |> Seq.map (fun constraintType -> TypeHelpers.processResType input constraintType tools)
            // Return the result followed by a newline
            (types, Environment.NewLine) |> JoinedText |> Some
          // Otherwise..
          else
            // return nothing
            None

        let getName (tag: ITag) =
          // Get the generic constraint signature
          let result = seq [
            // Get the constraint name
            yield tag.Reference |> InlineCode
            // If the given tag references an existing generic type..
            if (generics.ContainsKey(tag.Reference)) then
              // Get the constraint variance type
              let variance = generics.[tag.Reference].ToTuple() |> fst
              // if the constraint is a variant type..
              if (variance <> Variance.NonVariant) then
                // print the variance type
                yield variance |> StringConverters.varianceStr |> InlineCode
          ]
          // Join the text
          (result, " ") |> JoinedText

        // Get the generic constraints documentation
        let constraints = getConstraints tag
        seq [
          // Get the generic constraint name
          yield getName tag
          // Get the documention
          yield TagHelpers.tagShort input tag tools

          // If there are any constraints..
          if (Option.isSome constraints) then
            // return them
            yield constraints |> Option.get
        ]
        // Compose to elements
        |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)

      // If the input type can have type constraints..
      if input :? IInterface then
        // Get the documentation for the given input type
        TagHelpers.findTypeTag input ITag.TagType.Typeparam tools
        // Process each generic type and its constraints
        |> Seq.map (processTag >> Linq.ToReadOnlyCollection)
        |> Some
      // Otherwise..
      else
        // return nothing
        None

    // If there are generics..
    if (Option.isSome getTypeParams && getTypeParams |> (Option.get >> Seq.isEmpty >> not)) then
      // create a table of generics documentation
      seq [ tools.creator.CreateTable(getTypeParams |> Option.get, TextHelpers.createHeadings (seq [ "Type"; "Description"; "Constraints" ]) tools) |> ElementHelpers.toElement ] |> Some
    // Otherwise..
    else
      // return nothing
      None

  let private constructors (input: IType) tools =
    let extractor =
      // If the input is a class, return the constructors. Otherwise return an empty collection
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

      // Define the content to be documented for this member
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

    // Process the composed data
    genericProcessor extractor processCtor tools

  let private methods (input: IType) tools =
    let extractor =
      // If the input can have methods, return the methods. Otherwise return an empty collection
      match input with
      | :? IInterface as x -> x.Methods
      | _ -> LinkedList<IMethod>() :> IReadOnlyCollection<IMethod>
    let processMethod (_, method: IMethod) =
      let getOverloads = 
        let overloads = (overloads extractor).[method.Name]
        overloadFormat overloads.Count overloads.[method.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} {3}{4}{5} {6}{7}({8}){9}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getAsync;
          SignatureHelpers.getReturn;
          SignatureHelpers.getOperator;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools;
          SignatureHelpers.getGenericConstraints;
        ])

      // Define the content to be documented for this member
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

    // Process the composed data
    genericProcessor extractor processMethod tools

  let private properties (input: IType) tools =
    let extractor = 
      // If the input can have properties, return the properties. Otherwise return an empty collection
      match input with
      | :? IInterface as x -> x.Properties
      | _ -> LinkedList<IProperty>() :> IReadOnlyCollection<IProperty>
    let processProperty (_, property: IProperty) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2}{3} {4} {5} {{ {6} }}" (seq [
          SignatureHelpers.getAccessor
          SignatureHelpers.getIsReadonly
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getPropertyMethodsSignature
        ])

      // Define the content to be documented for this member
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

    // Process the composed data
    genericProcessor extractor processProperty tools

  let private events (input: IType) tools =
    let extractor = 
      // If the input can have events, return the events. Otherwise return an empty collection
      match input with
      | :? IInterface as x -> x.Events
      | _ -> LinkedList<IEvent>() :> IReadOnlyCollection<IEvent>
    let processEvent (_, event: IEvent) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} event {3} {4}" (seq [
          SignatureHelpers.getAccessor
          SignatureHelpers.getInheritance;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName
        ])

      // Define the content to be documented for this member
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

    // Process the composed data
    genericProcessor extractor processEvent tools

  let private delegates (input: IType) tools =
    let extractor = 
      // If the input can have delegates, return the delegates. Otherwise return an empty collection
      match input with
      | :? IInterface as x -> x.Delegates
      | _ -> LinkedList<IDelegate>() :> IReadOnlyCollection<IDelegate>
    let processDelegate (_, deleg: IDelegate) =
      let getOverloads = 
        let overloads = (overloads extractor).[deleg.Name]
        overloadFormat overloads.Count overloads.[deleg.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1} delegate {2} {3}{4}({5}){6}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools
          SignatureHelpers.getGenericConstraints;
        ])

      // Define the content to be documented for this member
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

    // Process the composed data
    genericProcessor extractor processDelegate tools

  let private enumFields (input: IType) tools =
    let extractor =
      // If the input is an enum, return its fields. Otherwise return an empty collection
      match input with
      | :? IEnum as e -> e.Fields
      | _ -> LinkedList<IEnumField>() :> IReadOnlyCollection<IEnumField>
    let processField (_, field: IEnumField) =
      let applyTools input = input tools

      // Define the content to be documented for this member
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

      // Note: Unecessary evil to comply with the generic program flow
      let intermediate tools _ =
        (if Seq.isEmpty content then seq [ TextHelpers.empty tools |> ElementHelpers.toElement ] else content)

      (field, field.Name, intermediate)

    // Process the composed data
    genericProcessor extractor processField tools

  /// <summary>
  /// Composes documentation content for the given <paramref name="input"/> type
  /// </summary>
  /// <param name="input">Input type to process</param>
  /// <param name="tools">Tools for creating the type documentation content</param>
  /// <param name="content"></param>
  /// <returns>Documentation contents</returns>
  let processContents input tools content =
    let processContent content =
      match content with
      | TypeConstructors -> (constructors, "Constructors")
      | TypeInheritance -> (inheritance, "Inheritance")
      | TypeProperties -> (properties, "Properties")
      | TypeDelegates -> (delegates, "Delegates")
      | TypeGenerics -> (typeParams, "Generic types")
      | TypeSeeAlso -> (single ITag.TagType.Seealso, "See also")
      | TypeSummary -> (single ITag.TagType.Summary, "Summary")
      | TypeRemarks -> (single ITag.TagType.Remarks, "Remarks")
      | TypeExample -> (single ITag.TagType.Example, "Example")
      | TypeMethods -> (methods, "Methods")
      | TypeNested -> (nested, "Nested types")
      | TypeEvents -> (events, "Events")
      | TypeFields -> (enumFields, "Fields")

    content
    // Process every case and append the result
    |> Seq.map (processContent >> fun (c, l) -> (c input tools, l))

  /// <summary>
  /// Composes a table of contents for the given <paramref name="input"/> type
  /// </summary>
  /// <param name="input">Input type to process</param>
  /// <param name="tools">Tools for creating the table of contents</param>
  /// <returns>Member table of contents</returns>
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
            if isNull method.Returns then
              InlineCode "void"
            else
              TypeHelpers.processResType input method.Returns tools

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

                // Begin arguments
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

      let flattenMembers (item: seq<bool * seq<AccessorType * seq<'M>>>) =
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

