using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Helpers;
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
    public bool IsStatic => false;

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IArgument> Arguments { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> Generics { get; }

    /// <inheritdoc />
    public IResType? Returns { get; }

    #endregion

    internal DelegateDef(Resolver resolver, TypeDef source)
    {
      var method = source.Methods.First(x => x.Name.Equals("Invoke"));
      Name = ResolveName(source);
      Arguments = ResolveArguments(resolver, method).ToReadOnlyCollection();
      Generics = ResolveGenerics(source, resolver);
      Returns = ResolveReturn(resolver, method);
      Accessor = ResolveAccessor(source);
      RawName = ResolveRawName(source, method);
    }

    private static string ResolveRawName(TypeDef source, dnlib.DotNet.MethodDef method)
      => source.ReflectionFullName.Replace('+', '.') + $"({string.Join(",", method.Parameters.WhereNotNull(x => x.ParamDef).Select(arg => arg.Type.FullName).Where(item => !string.IsNullOrEmpty(item)))})";

    private static IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> ResolveGenerics(TypeDef source, Resolver resolver)
    {
      IResType ResolveType(GenericParamConstraint x)
        => resolver.Resolve(x.Constraint.ToTypeSig());

      return source.HasGenericParameters
        ? source.GenericParameters.ToDictionary(x => x.Name.String, param => param.GenericParamConstraints.Select(ResolveType).ToReadOnlyCollection())
        : new Dictionary<string, IReadOnlyCollection<IResType>>();
    }

    private static IEnumerable<IArgument> ResolveArguments(Resolver resolver, dnlib.DotNet.MethodDef method)
      => method.Parameters
        // Filter out invalid arguments
        .Where(parameter => !string.IsNullOrEmpty(parameter.Name))
        // Initialize the arguments
        .Select(parameter => new ArgumentDef(resolver, parameter, method.ResolveMethodGenerics()));

    private IResType? ResolveReturn(Resolver resolver, dnlib.DotNet.MethodDef method)
      => !method.ReturnType.TypeName.Equals("Void", StringComparison.InvariantCultureIgnoreCase)
        ? resolver.Resolve(method.ReturnType, dynamicsMap: method.ParamDefs.Count - Arguments.Count == 1 ? method.ParamDefs.First().GetDynamicTypes(method.ReturnType) : null, generics: method.ResolveMethodGenerics())
        : null;

    private static AccessorType ResolveAccessor(TypeDef type)
    {
      // If the type is public..
      if (type.Visibility == TypeAttributes.Public || (type.IsNested && type.Visibility == TypeAttributes.NestedPublic))
        // return public
        return AccessorType.Public;
      // If the type is nested protected
      if (type.Visibility == TypeAttributes.NestedFamily)
        // return protected
        return AccessorType.Protected;
      // If the type is nested protected internal
      if (type.Visibility == TypeAttributes.NestedFamORAssem)
        // return protected internal
        return AccessorType.ProtectedInternal;
      // Otherwise return internal
      return AccessorType.Internal;
    }

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