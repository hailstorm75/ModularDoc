namespace MarkDoc.Generator.Basic

open System
open MarkDoc.Generator
open MarkDoc.Members.Types
open MarkDoc.Elements

type TypeComposer2(creator, docResolver, memberResolver, linker) = 
  let m_tools: Tools = { linker = linker; creator = creator; docResolver = docResolver; typeResolver = memberResolver }

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
        let page = Page(ComposerHelpers.composeContent input m_tools, "TODO", 0)
        ElementHelpers.initialize page m_tools :?> IPage