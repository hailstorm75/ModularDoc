using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing property members
  /// </summary>
  [DebuggerDisplay(nameof(PropertyDef) + (": {" + nameof(Name) + "}"))]
  public class PropertyDef
    : MemberDef, IProperty
  {
    #region Properties

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override bool IsStatic { get; }

    /// <inheritdoc />
    public MemberInheritance Inheritance { get; }

    /// <inheritdoc />
    public IResType Type { get; }

    /// <inheritdoc />
    public bool IsReadOnly { get; }

    /// <inheritdoc />
    public override AccessorType Accessor { get; }

    /// <inheritdoc />
    public AccessorType? GetAccessor { get; }

    /// <inheritdoc />
    public AccessorType? SetAccessor { get; }

    /// <inheritdoc />
    public override string RawName { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    private PropertyDef(Resolver resolver, dnlib.DotNet.PropertyDef source, dnlib.DotNet.MethodDef[] methods)
      : base(resolver, source)
    {
      var generics = source.ResolvePropertyGenerics(methods);
      Name = source.Name;
      RawName = ResolveRawName(source);
      IsStatic = methods.First().IsStatic;
      Type = Resolver.Resolve(ResolveType(source), generics, IsDynamic(source));
      Inheritance = ResolveInheritance(methods);
      Accessor = ResolveAccessor(methods);
      GetAccessor = ResolveAccessor(source.GetMethod);
      SetAccessor = ResolveAccessor(source.SetMethod);
      IsReadOnly = (source.GetMethod?
                     .CustomAttributes
                     .Any(x => x.TypeFullName.Equals("System.Runtime.CompilerServices.IsReadOnlyAttribute", StringComparison.InvariantCultureIgnoreCase)) ?? false)
                && (source.GetMethod?
                     .CustomAttributes
                     .All(x => !x.TypeFullName.Equals("System.Runtime.CompilerServices.CompilerGeneratedAttribute", StringComparison.InvariantCultureIgnoreCase)) ?? false);
    }

    #region Methods

    internal static PropertyDef? Initialize(Resolver resolver, dnlib.DotNet.PropertyDef source)
    {
      // Select non-private property methods
      var methods = source.SetMethods.Concat(source.GetMethods).Where(x => !x.IsPrivate).ToArray();

      // If no methods were selected..
      if (methods.Length == 0)
        // then this property is invalid
        return null;
      // Otherwise initialize the property
      return new PropertyDef(resolver, source, methods);
    }

    private static string ResolveRawName(dnlib.DotNet.PropertyDef source)
    {
      var rawName = source.FullName.AsSpan(source.FullName.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1);
      rawName = rawName.Slice(0, rawName.LastIndexOf("(", StringComparison.InvariantCultureIgnoreCase));

      return rawName.ToString()
        .Replace("::", ".", StringComparison.InvariantCultureIgnoreCase)
        .Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
    }

    private static dnlib.DotNet.TypeSig ResolveType(dnlib.DotNet.PropertyDef source)
    {
      // If the property has a getter method..
      if (source.GetMethod != null)
        // retrieve its return type
        return source.GetMethod.ReturnType;
      // If the property has a setter method..
      if (source.SetMethod != null)
        // retrieve its input argument
        return source.SetMethod.Parameters.Last().Type;

      // Property type was not resolved, thus this is not a valid property
      throw new NotSupportedException(Resources.notProperty);
    }

    private static bool IsDynamic(dnlib.DotNet.PropertyDef source)
    {
      // If the property has a getter method..
      if (source.GetMethod != null)
        // retrieve its return type
        return source.GetMethod.ParamDefs.Count != 0;
      // If the property has a setter method..
      if (source.SetMethod != null)
        // retrieve its input argument
        return source.SetMethod.Parameters.Count > 1;

      // Property type was not resolved, thus this is not a valid property
      throw new NotSupportedException(Resources.notProperty);
    }

    private static AccessorType? ResolveAccessor(dnlib.DotNet.MethodDef? method)
    {
      // If the method is null..
      if (method is null)
        // return unresolved accessor
        return null;

      // Map the property method accessor
      return method.Access switch
      {
        dnlib.DotNet.MethodAttributes.Public => AccessorType.Public,
        dnlib.DotNet.MethodAttributes.Family => AccessorType.Protected,
        dnlib.DotNet.MethodAttributes.Assembly => AccessorType.Internal,
        dnlib.DotNet.MethodAttributes.FamORAssem => AccessorType.ProtectedInternal,
        // Unresolved accessor
        _ => null
      };
    }

    private static AccessorType ResolveAccessor(IReadOnlyCollection<dnlib.DotNet.MethodDef> methods)
    {
      // If the property only has one method..
      if (methods.Count == 1)
        // process its accessor and use it as the accessor for the whole property
        return ResolveAccessor(methods.First()) ?? throw new Exception(Resources.accessorNull);

      // Otherwise resolve accessor of both methods
      var accessors = methods.Select(ResolveAccessor).ToArray();

      // If any of the accessors are public..
      if (accessors.Any(x => x.Equals(AccessorType.Public)))
        // then the property is public
        return AccessorType.Public;
      // If any of the accessors are protected..
      if (accessors.Any(x => x.Equals(AccessorType.Protected)))
        // then the property is protected
        return AccessorType.Protected;
      // If any of the accessors are internal
      if (accessors.Any(x => x.Equals(AccessorType.Internal)))
        // then the property is internal
        return AccessorType.Internal;
      if (accessors.Any(x => x.Equals(AccessorType.ProtectedInternal)))
        return AccessorType.ProtectedInternal;

      throw new NotSupportedException(Resources.accessorTypeInvalid);
    }

    private static MemberInheritance ResolveInheritance(IEnumerable<dnlib.DotNet.MethodDef> methods)
    {
      var method = methods.First();
      return (method.IsVirtual, method.IsAbstract) switch
      {
        (_, true) => MemberInheritance.Abstract,
        (true, false) => (method.Attributes & dnlib.DotNet.MethodAttributes.NewSlot) == 0
          ? MemberInheritance.Override
          : MemberInheritance.Virtual,
        _ => MemberInheritance.Normal
      };
    }

    #endregion
  }
}
