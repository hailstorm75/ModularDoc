using System;
using System.Collections.Generic;
using System.Linq;
using ModularDoc.Helpers;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class ArgumentTests
  {
    #region Data providers

    public static IEnumerable<object?[]> GetArgumentNameData()
    {
      static IEnumerable<object?[]> DataProvider(IInterface? type, Func<string, string> getter)
      {
        yield return new object?[] {type, getter(Constants.ARGUMENT_ONE), 0, "a"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_TWO), 0, "a"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_TWO), 1, "b"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 0, "a"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 1,"b"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 2, "c"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 3, "d"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 4, "e"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_PARAMATERS), 0, "a"};
      }

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.ARGUMENTS_NAMESPACE, Constants.ARGUMENTS_CLASS);
        var data = DataProvider(parent, Constants.GetMethod).Concat(DataProvider(parent, Constants.GetDelegate));
        foreach (object?[] item in data)
          yield return item;
      }
    }

    public static IEnumerable<object?[]> GetArgumentTypeData()
    {
      static IEnumerable<object?[]> DataProvider(IInterface? type, Func<string, string> getter)
      {
        const string intType = "int";

        yield return new object?[] {type, getter(Constants.ARGUMENT_ONE), 0, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_TWO), 0, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_TWO), 1, "bool"};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 0, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 1, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 2, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 3, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 4, intType};
        yield return new object?[] {type, getter(Constants.ARGUMENT_PARAMATERS), 0, intType};
      }

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.ARGUMENTS_NAMESPACE, Constants.ARGUMENTS_CLASS);
        var data = DataProvider(parent, Constants.GetMethod).Concat(DataProvider(parent, Constants.GetDelegate));
        foreach (object?[] item in data)
          yield return item;
      }
    }

    public static IEnumerable<object?[]> GetArgumentModifierData()
    {
      static IEnumerable<object?[]> DataProvider(IInterface? type, Func<string, string> getter)
      {
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 0, ArgumentType.Normal};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 1, ArgumentType.In};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 2, ArgumentType.Out};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 3, ArgumentType.Ref};
        yield return new object?[] {type, getter(Constants.ARGUMENT_MODIFIERS), 4, ArgumentType.Optional};
        yield return new object?[] {type, getter(Constants.ARGUMENT_PARAMATERS), 0, ArgumentType.Param};
      }

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.ARGUMENTS_NAMESPACE, Constants.ARGUMENTS_CLASS);
        var data = DataProvider(parent, Constants.GetMethod).Concat(DataProvider(parent, Constants.GetDelegate));
        foreach (object?[] item in data)
          yield return item;
      }
    }

    private static IArgument GetArgument(IInterface type, string name, int index)
    {
      if (name.StartsWith("Method", StringComparison.InvariantCultureIgnoreCase))
        return type.Methods.FindMember(name).Arguments.ElementAtOrDefault(index) ?? throw new KeyNotFoundException();
      if (name.StartsWith("Delegate", StringComparison.InvariantCultureIgnoreCase))
        return type.Delegates.FindMember(name).Arguments.ElementAtOrDefault(index) ?? throw new KeyNotFoundException();

      throw new NotSupportedException();
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IArgument))]
    [MemberData(nameof(GetArgumentNameData))]
    public void ValidateArgumentNames(IInterface type, string memberName, int argumentIndex, string argumentName)
    {
      var argument = GetArgument(type, memberName, argumentIndex);

      Assert.Equal(argumentName, argument.Name);
    }

    [Theory]
    [Trait("Category", nameof(IArgument))]
    [MemberData(nameof(GetArgumentTypeData))]
    public void ValidateArgumentTypes(IInterface type, string memberName, int argumentIndex, string argumentType)
    {
      var argument = GetArgument(type, memberName, argumentIndex);

      Assert.Equal(argumentType, argument.Type.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IArgument))]
    [MemberData(nameof(GetArgumentModifierData))]
    public void ValidateArgumentModifiers(IInterface type, string memberName, int argumentIndex, ArgumentType argumentType)
    {
      var argument = GetArgument(type, memberName, argumentIndex);

      Assert.Equal(argumentType, argument.Keyword);
    }
  }
}