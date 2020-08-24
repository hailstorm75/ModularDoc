namespace MarkDoc.Generator.Basic

open MarkDoc.Documentation.Tags
open MarkDoc.Members.Members
open MarkDoc.Members.Types
open MarkDoc.Elements
open MarkDoc.Helpers
open System

/// <summary>
/// Documentation section types for members
/// </summary>
type ContentType =
  /// <summary>
  /// Member summary documentation
  /// </summary>
  | Summary
  /// <summary>
  /// Member remarks documentation
  /// </summary>
  | Remarks
  /// <summary>
  /// Member additional references
  /// </summary>
  | SeeAlso
  /// <summary>
  /// Member example usage
  /// </summary>
  | Example
  /// <summary>
  /// Member return value documentation
  /// </summary>
  | Value
  /// <summary>
  /// Member return documentation
  /// </summary>
  | Returns
  /// <summary>
  /// Member thrown exceptions documentation
  /// </summary>
  | Exceptions
  /// <summary>
  /// Member arguments documentation
  /// </summary>
  | Arguments
  /// <summary>
  /// Member declaration source
  /// </summary>
  | Inheritance

module internal ContentHelpers =
  let private getSingleTag (input: IType) tag (typeMember: IMember) tools =
    let single tag =
      // Compose the tag if it exists into an element sequence
      match tag with
      | Some tag -> TagHelpers.tagFull input tag tools |> Seq.map ElementHelpers.toElement |> SomeHelpers.emptyToNone
      | _ -> None
    // Try to find documentation tags of the specified type
    TagHelpers.findTag input typeMember tag tools
    // Select only a single tag result
    |> Seq.tryExactlyOne
    // Process the single result
    |> single
  let private getExceptions (input: IType) (typeMember: IMember) tools =
    let applyTools input = input tools
    // Get the documented exceptions for this given member
    let exceptions =
      // Find the documentation tag for exceptions
      TagHelpers.findTag input typeMember ITag.TagType.Exception tools
      // Compose each exception into a row of its type and description
      |> Seq.map (fun x -> seq [ TypeHelpers.processReference input x.Reference tools; TagHelpers.tagShort input x tools ] |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement) |> Linq.ToReadOnlyCollection)

    // If there are no exceptions documented..
    if Seq.isEmpty exceptions then
      // return nothing
      None
    // Otherwise..
    else
      // compose a table of exceptions
      seq [ tools.creator.CreateTable(exceptions, TextHelpers.createHeadings (seq [ "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some
  let private getSeeAlso (input: IType) (m: IMember) tools =
    let applyTools input = input tools
    // Get additional references
    let seeAlsos =
      // Find the documentation tag for additional references
      TagHelpers.findTag input m ITag.TagType.Seealso tools
      // Extract the references
      |> Seq.map (fun x -> TagHelpers.tagShort input x tools |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)

    // If there are no additional references..
    if Seq.isEmpty seeAlsos then
      // return nothing
      None
    // Otherwise..
    else
      // compose a list of additional references
      seq [ tools.creator.CreateList(seeAlsos, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some
  let private getArguments (input: IType) (typeMember: IMember) tools = 
    let applyTools input = input tools
    // Get argument documentation
    let argumentDocs =
      // Get documentation for the member arguments
      TagHelpers.findTag input typeMember ITag.TagType.Param tools
      // Couple argument names and their documentation
      |> Seq.map (fun x -> x.Reference, TagHelpers.tagShort input x tools |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)
      // Materialize the sequence to a dictionary
      |> dict

    let processArguments (argument: IArgument) =
      // Get documentation for the given argument
      let description =
        seq [
          let mutable value: IElement = null
          // If there is documentation..
          if argumentDocs.TryGetValue(argument.Name, &value) then
            // return it
            yield value
        ]

      // Get the argument type
      let argumentType = 
        // Get the argument signature parts
        let typeParts = seq [
          // Get the modifier string representation
          let modifier = argument |> StringConverters.argumentTypeStr
          // If the modifier name is valid..
          if not (String.IsNullOrEmpty modifier) then
            // return it
            yield modifier |> InlineCode

          // Return the argument type
          yield TypeHelpers.processResType input argument.Type tools
        ]

        // Compose the signature type parts
        JoinedText (typeParts, " ")

      // Compose the type signature
      let typeSignature =
        // Join the argument type and its name
        seq [ argumentType; argument.Name |> Normal ]
        // Compose the argument into an element
        |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)

      description
      // Join the argument signature name with its documentation
      |> Seq.append typeSignature
      // Materialize the sequence to a collection
      |> Linq.ToReadOnlyCollection

    let generateResult (arguments: IArgument seq) =
      let argumentsProcessed =
        arguments
        // Compose a row of argument information
        |> Seq.map processArguments

      // If there no arguments..
      if (Seq.isEmpty argumentsProcessed) then
        // return nothing
        None
      // Otherwise..
      else
        // compose a documentation table of the member arguments
        seq [ tools.creator.CreateTable(argumentsProcessed, TextHelpers.createHeadings (seq [ "Type"; "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some

    // Process the member arguments based on the member type
    match typeMember with
    | :? IConstructor as x -> generateResult x.Arguments
    | :? IDelegate as x -> generateResult x.Arguments
    | _ -> None
  let private getInheritedFrom (input: IType) (typeMember: IMember) tools = 
    let applyTools input = input tools
    let getInheritance(interfaceType: IInterface) =
      let typeReference (t: IType) = 
        // Get the type name and wrap it into a link to the known type
        (TypeHelpers.getTypeName t |> Normal, tools.linker.CreateLink(input, t)) |> LinkElement

      let mutable result: IInterface = null
      // If the member is inherited..
      if interfaceType.InheritedTypes.Value.TryGetValue(typeMember, &result) then
        Some(seq [ typeReference result |> ElementHelpers.initialize |> applyTools ])
      // Otherwise..
      else
        // return nothing
        None

    // Process the type if it can have inherited types
    match input with
    | :? IInterface as i -> getInheritance i
    | _ -> None

  /// <summary>
  /// Composes <paramref name="content"/> for given member
  /// </summary>
  /// <param name="content">Content to generate<param/>
  /// <param name="input">Declaring type of the documented member<param/>
  /// <returns>Composed content</returns>
  let processContents content input =
    let processContent content =
      // Composes content based on its type
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

    content
    // Compose the requested content
    |> Seq.map processContent
