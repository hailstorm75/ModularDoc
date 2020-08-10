using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.Members
{
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
      var method = source.Methods.FirstOrDefault(x => x.Name.Equals("Invoke"));
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
        .Where(x => !string.IsNullOrEmpty(x.Name))
        .Select(x => new ArgumentDef(resolver, x, method.ResolveMethodGenerics()))
        .ToReadOnlyCollection();

    private static IResType? ResolveReturn(IResolver resolver, dnlib.DotNet.MethodDef method)
      => method.ReturnType.TypeName.Equals("Void", StringComparison.InvariantCultureIgnoreCase)
          ? null
          : resolver.Resolve(method.ReturnType, method.ResolveMethodGenerics());

    private static AccessorType ResolveAccessor(TypeDef @delegate)
    {
      if (@delegate.Visibility == TypeAttributes.Public)
        return AccessorType.Public;
      if (@delegate.Visibility == TypeAttributes.NestedFamily)
        return AccessorType.Protected;
      return AccessorType.Internal;
    }

    private static string ResolveName(ITypeDefOrRef source)
    {
      var namespaceCut = CutNamespace(source);
      if (source is ITypeOrMethodDef type && !type.HasGenericParameters)
        return namespaceCut;

      var genericsIndex = namespaceCut.IndexOf('`', StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return namespaceCut;
      var genericCut = namespaceCut.Remove(genericsIndex);

      return genericCut;
    }

    private static string CutNamespace(ITypeDefOrRef source)
      => source.FullName.Substring(source.FullName.LastIndexOf('/') + 1);
  }
}