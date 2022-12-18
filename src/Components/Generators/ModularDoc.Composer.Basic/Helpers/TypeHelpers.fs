namespace ModularDoc.Composer.Basic

open System.Linq
open ModularDoc.Members.ResolvedTypes
open ModularDoc.Members.Members
open ModularDoc.Members.Types
open System

module internal TypeHelpers =
  /// <summary>
  /// Gets the name of the given <paramref name="input"/>
  /// </summary>
  /// <param name="input">Input type to process</param>
  /// <returns>Converted type name to a string</returns>
  let getTypeName (input: IType) =
    let returnGenerics generics =
      let joinGenerics (i: seq<string>) =
        "<" + String.Join(", ", i) + ">"

      // If there are generics..
      if not (Seq.isEmpty generics) then
        // return the type name with generics
        input.Name + joinGenerics generics
      // Otherwise..
      else
        // return the type name as is
        input.Name

    let processWithVariance (input: IInterface) =
      // Get the generics with variances
      let generics =
        input.Generics
        // Retrieve the generic type names and variances
        |> Seq.map (fun x -> (x.Value.ToTuple() |> fst |> StringConverters.varianceStr) + " " + x.Key)
      returnGenerics generics
    let processWithoutInheritance (input: 'M when 'M :> IInterface) =
      // Get the generics without variances
      let generics =
        input.Generics
        // Retrieve the generic type names
        |> Seq.map (fun x -> x.Key)
      returnGenerics generics

    // Get the input name based on its name
    match input with
    | :? IInterface as x ->
      match x with
      | :? IClass as y -> processWithoutInheritance y
      | :? IStruct as y -> processWithoutInheritance y
      | _ -> processWithVariance x
    | _ -> input.Name

  let private tryFindMember (input: IType) (memberFull: string) (memberCut: string) =
    let toMember (input: 'M when 'M :> IMember) =
      // Cast to a member type
      input :> IMember
    let findMember (input: 'M seq when 'M :> IMember) =
      input
      // Cast to member types
      |> Seq.map toMember
      // Find a matching member based on its name
      |> Seq.tryFind (fun x -> x.Name.Equals(memberCut))
    // Find the member based on the declaring parent
    match input with
    | :? IInterface as i ->
      // Find the member based on the member key
      match memberFull.[0] with
      | 'M' -> i.Methods |> findMember
      | 'P' -> i.Properties |> findMember
      | 'E' -> i.Events |> findMember
      | _ -> None
    | :? IEnum as e -> e.Fields |> findMember
    | _ -> None

  /// <summary>
  /// Processes the given <paramref name="reference"/> into a linked type name
  /// </summary>
  /// <param name="input">Reference declaring source</param>
  /// <param name="reference">String reference to a <see cref="IType"/> or <see cref="IMember"/></param>
  /// <param name="tools">Tools for processing the given reference</param>
  /// <returns>Converted type name to a string</returns>
  let processReference (input: IType) (reference: string) tools =
    let typeReference (reference: string) =
      let mutable result: IType = null
      // If the reference type is known..
      if tools.typeResolver.TryFindType(reference.[2..], &result) then
        // return the referenced type name and with a link to the known type
        LinkContent ((getTypeName result) |> Normal, lazy(tools.linker.CreateLink(input, result)))
      // Otherwise..
      else
        let slice = reference.AsSpan(reference.LastIndexOf('.') + 1)
        // try to find generics
        let index = slice.IndexOf('`')
        // if generics were found..
        if index <> -1 then
          let generateGenerics = 
            // Find the number of generics
            seq [ 1 .. (slice.Slice(index + 1).ToString() |> int) ]
            // Compose the generic names
            |> Seq.map (fun x -> "T" + x.ToString())

          // return the referenced type with generics
          String.Format("{0}<{1}>", slice.Slice(0, slice.IndexOf('`')).ToString(), TextHelpers.normal (String.Join(", ", generateGenerics)) tools) |> Normal
        // otherwise..
        else
          // return the referenced type as is without generics
          slice.ToString() |> Normal

    let memberReference cutter =
      // Get the member reference without the namespace
      let memberString: string = cutter()
      // Get the declaring type referencee
      let typeString = reference.[..reference.Length - memberString.Length - 2]
      // Get the known declaring type name
      let typeRef = typeReference typeString

      // Create an anchor to the documented member
      let memberAnchor = 
        let mutable result: IType = null
        // If the declaring type is known..
        if tools.typeResolver.TryFindType(typeString.[2..], &result) then
          // get the member from the declaring type
          let mem = tryFindMember result reference memberString
          // if the member is known..
          if Option.isSome mem then
            // create an anchor to the member and return it
            LinkContent (Normal memberString, tools.linker.CreateAnchor(input, mem |> Option.get))
          // otherwise..
          else
            // return the member name as is
            memberString |> Normal
        // Otherwise..
        else
          // return the member name as is
          memberString |> Normal

      // Return the member with its declaring type
      (seq [ typeRef; memberAnchor ], ".") |> JoinedText

    let cutMethod() = 
      // Remove the namespace and declaring type
      if reference.IndexOf('(') <> -1 then
        reference.Substring(reference.AsSpan(0, reference.IndexOf('(')).LastIndexOf('.') + 1)
      else
        reference.Substring(reference.AsSpan().LastIndexOf('.') + 1)
    let cutMember() = 
      // Remove the namespace and declaring type
      reference.Substring(reference.LastIndexOf('.') + 1)

    // Process the reference based on its key
    match reference.[0] with
    | 'T' -> typeReference reference
    | 'E' -> typeReference reference
    | 'M' -> memberReference cutMethod
    | 'P' -> memberReference cutMember
    | 'F' -> memberReference cutMember
    | _ -> reference.Substring(2) |> Normal

  /// <summary>
  /// Gets the name of the given <paramref name="item"/>
  /// </summary>
  /// <remarks>
  /// Additionally wraps the name in a link if the type is known
  /// </remarks>
  /// <param name="source">Resolved type referencing source</param>
  /// <param name="item">Resolved type to process</param>
  /// <param name="tools">Tools for processing the resolved type</param>
  /// <returns>Formatted display name of the processed <paramref name="item"/></returns>
  let rec processResType source (item: IResType) tools =
    let tryLink (item: IResType) =
      // Link to the known resolved type
      let link = tools.linker.CreateLink(source, item)
      // If the link is valid..
      if not (String.IsNullOrEmpty link) then
        // return the name wrapped into a link
        (InlineCode item.DisplayName, lazy link) |> LinkContent
      // Otherwise..
      else
        // return without a link
        InlineCode item.DisplayName

    // Process the resolved type based on whether it is generic or not
    match item with
    | :? IResTuple as tup ->
      (seq [
        "(" |> Normal;
        (
          tup.Fields
          |> Seq.map (fun tupleField ->
                       (seq [
                         let t = tupleField.ToTuple()
                         yield processResType source (t |> snd) tools
                         if fst t |> String.IsNullOrEmpty |> not then
                           yield " " + fst t |> Normal
                       ], "") |> JoinedText
                     ),
          ", "
        ) |> JoinedText;
        ")" |> Normal
      ], "") |> JoinedText
    | :? IResGeneric as generic ->
      // Compose the resolved type signature
      (seq [
        tryLink generic;
        "<" |> Normal;
        (generic.Generics |> Seq.map (fun n -> processResType source n tools), ", ") |> JoinedText;
        ">" |> Normal
      ], "") |> JoinedText
    | :? IResArray as arr ->
      let braces = if arr.IsJagged then
                     String.Join("", Enumerable.Repeat("[]", arr.Dimension))
                   else
                     String.Format("[{0}]", String.Join("", Enumerable.Repeat(",", arr.Dimension-1)))
      
      (seq [ processResType source arr.ArrayType tools; InlineCode braces ], "") |> JoinedText
    | _ -> tryLink item

