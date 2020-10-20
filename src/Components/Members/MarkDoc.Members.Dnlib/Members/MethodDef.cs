using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Helpers;
using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;
using IMethod = MarkDoc.Members.Members.IMethod;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing method members
  /// </summary>
  [DebuggerDisplay(nameof(MethodDef) + (": {" + nameof(Name) + "}"))]
  public class MethodDef
    : ConstructorDef, IMethod
  {
    #region Properties

    /// <inheritdoc />
    public bool IsAsync { get; }

    /// <inheritdoc />
    public MemberInheritance Inheritance { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> Generics { get; }

    /// <inheritdoc />
    public IResType? Returns { get; }

    /// <inheritdoc />
    public OperatorType Operator { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal MethodDef(Resolver resolver, dnlib.DotNet.MethodDef source)
      : base(resolver, source, ResolveOperator(source, resolver, out var isOperator))
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Operator = isOperator;
      IsAsync = ResolveAsync(source);
      Inheritance = ResolveInheritance(source);
      Generics = ResolveGenerics(source, resolver);
      Returns = ResolveReturn(source);
    }

    #region Methods

    private static string ResolveOperator(dnlib.DotNet.MethodDef source, Resolver resolver, out OperatorType @operator)
    {
      // Assume that the source is a normal operator
      @operator = OperatorType.Normal;

      static string RetrieveConverterName(Resolver resolver, dnlib.DotNet.MethodDef input)
        => resolver.Resolve(input.ReturnType).DisplayName;

      switch (source.Name.ToUpperInvariant())
      {
        case "OP_IMPLICIT":
          @operator = OperatorType.Implicit;
          return RetrieveConverterName(resolver, source);
        case "OP_EXPLICIT":
          @operator = OperatorType.Explicit;
          return RetrieveConverterName(resolver, source);
        case "OP_ADDITION":
          return "+";
        case "OP_SUBTRACTION":
          return "-";
        case "OP_MULTIPLY":
          return "*";
        case "OP_DIVISION":
          return "/";
        case "OP_MODULUS":
          return "%";
        case "OP_EXCLUSIVEOR":
          return "^";
        case "OP_BITWISEAND":
          return "&";
        case "OP_BITWISEOR":
          return "|";
        case "OP_LOGICALNOT":
          return "!";
        case "OP_LEFTSHIFT":
          return "<<";
        case "OP_RIGHTSHIFT":
          return ">>";
        case "OP_EQUALITY":
          return "==";
        case "OP_GREATERTHAN":
          return ">";
        case "OP_LESSTHAN":
          return "<";
        case "OP_INEQUALITY":
          return "!=";
        case "OP_GREATERTHANOREQUAL":
          return ">=";
        case "OP_LESSTHANOREQUAL":
          return "<=";
        case "OP_DECREMENT":
          return "--";
        case "OP_INCREMENT":
          return "++";
        case "OP_UNARYNEGATION":
          return "-";
        case "OP_UNARYPLUS":
          return "+";
        case "OP_ONESCOMPLEMENT":
          return "~";
        default:
          @operator = OperatorType.None;
          return source.Name;
      }
    }

    private static bool ResolveAsync(dnlib.DotNet.MethodDef source)
      => source.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name.String.Equals(nameof(AsyncStateMachineAttribute), StringComparison.InvariantCulture)) != null;

    private IResType? ResolveReturn(dnlib.DotNet.MethodDef source)
    {
      if (source.ReturnType.TypeName.Equals("Void", StringComparison.InvariantCultureIgnoreCase))
        return null;
      return Resolver.Resolve(source.ReturnType, dynamicsMap: source.ParamDefs.Count - Arguments.Count == 1 ? source.ParamDefs.First().GetDynamicTypes(source.ReturnType) : null, generics: source.ResolveMethodGenerics());
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & MethodAttributes.NewSlot) == 0)
        return MemberInheritance.Override;
      if (source.IsAbstract)
        return MemberInheritance.Abstract;
      if (source.IsVirtual)
        return MemberInheritance.Virtual;

      return MemberInheritance.Normal;
    }

    private static IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> ResolveGenerics(dnlib.DotNet.MethodDef source, Resolver resolver)
    {
      IResType ResolveType(GenericParamConstraint x)
        => resolver.Resolve(x.Constraint.ToTypeSig());

      return source.HasGenericParameters
        ? source.GenericParameters.ToDictionary(x => x.Name.String, param => param.GenericParamConstraints.Select(ResolveType).ToReadOnlyCollection())
        : new Dictionary<string, IReadOnlyCollection<IResType>>();
    }

    #endregion
  }
}
