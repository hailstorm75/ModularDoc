using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing delegate members
  /// </summary>
  [DebuggerDisplay(nameof(DelegateDef) + (": {" + nameof(Name) + "}"))]
  public class DelegateDef
    : IDelegate
  {
    #region Properties

    /// <inheritdoc />
    public bool IsStatic { get; } = false;

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IArgument> Arguments { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Generics { get; }

    /// <inheritdoc />
    public IResType? Returns { get; }

    #endregion

    internal DelegateDef(IResolver resolver, TypeDef source)
    {
      var method = source.Methods.First(x => x.Name.Equals("Invoke"));
      Name = ResolveName(source);
      Arguments = ResolveArguments(resolver, method).ToReadOnlyCollection();
      Generics = ResolveGenerics(source).ToReadOnlyCollection();
      Returns = ResolveReturn(resolver, method);
      Accessor = ResolveAccessor(source);
      RawName = source.ReflectionFullName.Replace('+', '.');
    }

    private static IEnumerable<string> ResolveGenerics(ITypeOrMethodDef source)
      => !source.HasGenericParameters
           ? Enumerable.Empty<string>()
           : source.GenericParameters.Select(x => x.Name.String);

    private static IEnumerable<IArgument> ResolveArguments(IResolver resolver, dnlib.DotNet.MethodDef method)
      => method.Parameters
        // Filter out invalid arguments
        .Where(parameter => !string.IsNullOrEmpty(parameter.Name))
        // Initialize the arguments
        .Select(parameter => new ArgumentDef(resolver, parameter, method.ResolveMethodGenerics()));

    private static IResType? ResolveReturn(IResolver resolver, dnlib.DotNet.MethodDef method)
      => !method.ReturnType.TypeName.Equals("Void", StringComparison.InvariantCultureIgnoreCase)
          ? resolver.Resolve(method.ReturnType, method.ResolveMethodGenerics())
          : null;

    private static AccessorType ResolveAccessor(TypeDef @delegate)
      => @delegate.Visibility switch
      {
        TypeAttributes.Public => AccessorType.Public,
        TypeAttributes.NestedFamily => AccessorType.Protected,
        _ => AccessorType.Internal
      };

    private static string ResolveName(ITypeDefOrRef source)
    {
      // Remove the namespace from the source name
      var namespaceCut = CutNamespace(source);
      // If the source is not generic..
      if (source is ITypeOrMethodDef type && !type.HasGenericParameters)
        // return the name as is
        return namespaceCut;

      // Otherwise find the generics
      var genericsIndex = namespaceCut.IndexOf('`', StringComparison.InvariantCulture);
      // If none were found..
      if (genericsIndex == -1)
        // return the name as is
        return namespaceCut;
      // Otherwise cut the generics and return the result
      return namespaceCut.Remove(genericsIndex);
    }

    private static string CutNamespace(ITypeDefOrRef source)
      => source.FullName.Substring(source.FullName.LastIndexOf('/') + 1);
  }
}