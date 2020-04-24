using MarkDoc.Helpers;
using System;

namespace MarkDoc.Members.Dnlib
{
  public class Resolver
    : LazySingleton<Resolver>, IResolver
  {
    public IType Resolve(object subject, object? parent = null)
    {
      if (!(subject is dnlib.DotNet.TypeDef type))
        throw new InvalidOperationException($"Argument type of {subject} is not {nameof(dnlib.DotNet.TypeDef)}.");

      var nestedParent = ResolveParent(parent);

      if (type.IsClass)
        return new ClassDef(type, nestedParent);
      if (type.IsInterface)
        return new InterfaceDef(type, nestedParent);
      if (type.IsEnum)
        return new EnumDef(type, nestedParent);

      throw new NotSupportedException("Subject not supported");
    }

    private static dnlib.DotNet.TypeDef? ResolveParent(object? parent)
    {
      if (parent == null)
        return null;
      if (!(parent is dnlib.DotNet.TypeDef type))
        throw new InvalidOperationException($"Argument type of {parent} is not {nameof(dnlib.DotNet.TypeDef)}.");

      return type;
    }
  }
}
