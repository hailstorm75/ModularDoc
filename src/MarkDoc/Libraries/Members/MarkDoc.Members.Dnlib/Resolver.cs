using System;

namespace MarkDoc.Members.Dnlib
{
  public class Resolver
    : IResolver
  {
    public IType Resolve(object subject)
    {
      if (!(subject is dnlib.DotNet.TypeDef type))
        throw new InvalidOperationException($"Argument type of {subject} is not {nameof(dnlib.DotNet.TypeDef)}.");

      if (type.IsClass)
        return new ClassDef(type);
      if (type.IsInterface)
        return new InterfaceDef(type);
      if (type.IsEnum)
        return new EnumDef(type);

      throw new NotSupportedException("Subject not supported");
    }
  }
}
