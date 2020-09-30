using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using MarkDoc.Members.ResolvedTypes;
using IType = MarkDoc.Members.Types.IType;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Base class for all resolved types
  /// </summary>
  [DebuggerDisplay(nameof(ResType) + ": {" + nameof(DisplayName) + "}")]
  public class ResType
    : IResType
  {
    #region Properties

    /// <summary>
    /// Type resolver
    /// </summary>
    protected IResolver Resolver { get; }

    /// <inheritdoc />
    public string DisplayName { get; }

    /// <inheritdoc />
    public string DocumentationName { get; }

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    /// <inheritdoc />
    public bool IsByRef { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="isByRef"></param>
    internal ResType(IResolver resolver, TypeSig source, bool isByRef = false)
      : this(resolver, source, ResolveName(source), ResolveDocName(source), source.FullName, isByRef) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="displayName">Type display name</param>
    /// <param name="docName">Type documentation name</param>
    /// <param name="rawName">Type raw name</param>
    /// <param name="isByRef"></param>
    protected ResType(IResolver resolver, TypeSig source, string displayName, string docName, string rawName, bool isByRef)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));
      // If the raw name is null..
      if (rawName is null)
        // throw an exception
        throw new ArgumentNullException(nameof(rawName));

      Resolver = resolver;
      DocumentationName = docName;
      RawName = ProcessRawName(rawName);
      DisplayName = displayName;
      TypeNamespace = source.Namespace;
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.PublicationOnly);
      IsByRef = isByRef;
    }

    #region Methods

    private static string ProcessRawName(string name)
    {
      // Find the index of generic types
      var genericIndex = name.LastIndexOf('<');
      // If an index was found..
      return genericIndex != -1
        // remove the generics and return the result
        ? name.Remove(genericIndex)
        // otherwise return as is
        : name;
    }

    protected static string ResolveDocName(TypeSig source)
    {
      static IEnumerable<string> RetrieveNested(dnlib.DotNet.IType? source)
      {
        static string ReformatGenerics(string value)
        {
          // Find the index of generics
          var genericsIndex = value.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
          // If there are no generics..
          if (genericsIndex == -1)
            // return as is
            return value;

          // Otherwise remove the old generics
          var name = value.Remove(genericsIndex);
          // If the number of generics is present..
          return int.TryParse(value.Substring(genericsIndex + 1), out var number)
            // generate a new format for the generics and return the result
            ? $"{name}{{{string.Join(",", Enumerable.Repeat('`', number).Select((x, i) => $"{x}{i}"))}}}"
            // otherwise return as is
            : name;
        }

        // If the source is null..
        if (source is null)
          // exit
          yield break;

        // Get the nested types of given type parent
        var nested = RetrieveNested(source.ScopeType.DeclaringType);
        // For every nested type..
        foreach (var item in nested)
          // return it
          yield return item;

        // If there is no parent..
        if (source.ScopeType.DeclaringType is null)
          // return the namespace
          yield return source.Namespace;

        // Return the generics
        yield return ReformatGenerics(source.Name);
      }

      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // If the source is not nested..
      if (!source.FullName.Contains('/', StringComparison.InvariantCultureIgnoreCase))
        // return its full name
        return source.FullName;

      // Otherwise create a new name for the nested source
      return $"{string.Join(".", RetrieveNested(source.ScopeType))}";
    }

    protected static string ResolveName(dnlib.DotNet.IType source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Get the source name
      var name = source.Name.String;
      // Find the generics
      var genericsIndex = name.IndexOf('`', StringComparison.InvariantCulture);

      // If there are generics..
      return genericsIndex != -1
        // remove them and return the result
        ? name.Remove(genericsIndex)
        // otherwise return as is
        : name;
    }

    #endregion
  }
}
