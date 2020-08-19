namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Types
open System
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Members

module TypeHelpers =
  let getTypeName (input: IType) =
    let joinGenerics (i: seq<string>) =
      let partial f x y = f(x , y)
      let generics = i |> partial String.Join ", "
      "<" + generics + ">"
    let processInterface (input: IInterface) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> (x.Value.ToTuple() |> fst |> StringConverters.varianceStr) + " " + x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name
    let processStruct (input: 'M when 'M :> IInterface) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name

    match input with
    | :? IClass as x -> processStruct x
    | :? IStruct as x -> processStruct x
    | :? IInterface as x -> processInterface x
    | _ -> input.Name

  let tryFindMember (input: IType, memberFull: string, memberCut: string) =
    let toMember (a: 'M when 'M :> IMember) =
      a :> IMember
    let findMember (a: 'M seq when 'M :> IMember) =
      a |> Seq.map toMember |> Seq.tryFind (fun x -> x.Name.Equals(memberCut))
    match input with
    | :? IInterface as i ->
      match memberFull.[0] with
      | 'M' -> i.Methods |> findMember
      | 'P' -> i.Properties |> findMember
      | 'E' -> i.Events |> findMember
      | _ -> None
    | :? IEnum as e -> e.Fields |> findMember
    | _ -> None

  let processReference (input: IType, reference: string, tools) =
    let typeReference (reference: string) =
      let mutable result: IType = null
      if tools.typeResolver.TryFindType(reference.[2..], &result) then
        LinkContent ((getTypeName result) |> Normal, lazy(tools.linker.CreateLink(input, result)))
      else
        let slice = reference.AsSpan(reference.LastIndexOf('.') + 1)
        let index = slice.IndexOf('`')
        if index <> -1 then
          let generateGenerics = 
            seq [ 1 .. (slice.Slice(index + 1).ToString() |> int) ]
            |> Seq.map (fun x -> "T" + x.ToString())

          String.Format("{0}<{1}>", slice.Slice(0, slice.IndexOf('`')).ToString(), TextHelpers.normal (String.Join(", ", generateGenerics)) tools) |> Normal
        else
          slice.ToString() |> Normal

    let memberReference cutter =
      let memberString: string = cutter()
      let typeString = reference.[..reference.Length - memberString.Length - 2]
      let typeRef = typeReference typeString

      let memberAnchor = 
        let mutable result: IType = null
        if tools.typeResolver.TryFindType(typeString.[2..], &result) then
          let mem = tryFindMember(result, reference, memberString)
          if Option.isSome mem then
            LinkContent (Normal memberString, tools.linker.CreateAnchor(input, mem |> Option.get))
          else
            memberString |> Normal
        else
          memberString |> Normal

      (seq [ typeRef; memberAnchor ], ".") |> JoinedText

    let cutMethod() = 
      reference.Substring(reference.AsSpan(0, reference.IndexOf('(')).LastIndexOf('.') + 1)
    let cutMember() = 
      reference.Substring(reference.LastIndexOf('.') + 1)

    match reference.[0] with
    | 'T' -> typeReference reference
    | 'E' -> typeReference reference
    | 'M' -> memberReference cutMethod
    | 'P' -> memberReference cutMember
    | 'F' -> memberReference cutMember
    | _ -> reference.Substring(2) |> Normal

  let processResType source (item: IResType) tools =
    let tryLink (item: IResType) =
      let link = tools.linker.CreateLink(source, item)
      if not (String.IsNullOrEmpty link) then
        (InlineCode item.DisplayName, lazy(link)) |> LinkContent
      else
        InlineCode item.DisplayName

    // TODO: Generic arrays?
    match item with
    | :? IResGeneric as generic ->
      let generics = generic.Generics
                     |> Seq.map tryLink
      let content = seq [ tryLink generic; "<" |> Normal; (generics, ", ") |> JoinedText; ">" |> Normal ]
      (content, "") |> JoinedText
    | _ -> tryLink item


