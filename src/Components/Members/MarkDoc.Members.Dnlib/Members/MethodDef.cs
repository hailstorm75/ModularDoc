using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;
using IMethod = MarkDoc.Members.Members.IMethod;

namespace MarkDoc.Members.Dnlib.Members
{
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
    public IReadOnlyCollection<string> Generics { get; }

    /// <inheritdoc />
    public IResType? Returns { get; }

    /// <inheritdoc />
    public OperatorType Operator { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal MethodDef(IResolver resolver, dnlib.DotNet.MethodDef source)
      : base(resolver, source, ResolveOperator(source, out var isOperator))
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Operator = isOperator;
      IsAsync = ResolveAsync(source);
      Inheritance = ResolveInheritance(source);
      Generics = ResolveGenerics(source).ToReadOnlyCollection();
      Returns = ResolveReturn(source);
    }

    #region Methods

    private static string ResolveOperator(IFullName source, out OperatorType @operator)
    {
      @operator = OperatorType.Normal;

      static string RetrieveConverterName(IFullName input)
      {
        var name = input.FullName.AsSpan().Slice(input.FullName.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1);
        var colonIndex = name.IndexOf(':');

        return name.Slice(0, colonIndex).ToString();
      }

      switch (source.Name.ToUpperInvariant())
      {
        case "OP_IMPLICIT":
          @operator = OperatorType.Implicit;
          return RetrieveConverterName(source);
        case "OP_EXPLICIT":
          @operator = OperatorType.Explicit;
          return RetrieveConverterName(source);
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
        case "OP_LOGICALAND":
          return "&&";
        case "OP_LOGICALOR":
          return "||";
        case "OP_LOGICALNOT":
          return "!";
        case "OP_ASSIGN":
          return "=";
        case "OP_LEFTSHIFT":
          return "<<";
        case "OP_RIGHTSHIFT":
          return ">>";
        case "OP_SIGNEDRIGHTSHIFT":
        case "OP_UNSIGNEDRIGHTSHIFT":
          return "";
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
        case "OP_MULTIPLICATIONASSIGNMENT":
          return "*=";
        case "OP_SUBTRACTIONASSIGNMENT":
          return "-=";
        case "OP_EXCLUSIVEORASSIGNMENT":
          return "^=";
        case "OP_LEFTSHIFTASSIGNMENT":
          return "<<=";
        case "OP_MODULUSASSIGNMENT":
          return "%=";
        case "OP_ADDITIONASSIGNMENT":
          return "+=";
        case "OP_BITWISEANDASSIGNMENT":
          return "&=";
        case "OP_BITWISEORASSIGNMENT":
          return "|=";
        case "OP_COMMA":
          return ",";
        case "OP_DIVISIONASSIGNMENT":
          return "/=";
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
      return Resolver.Resolve(source.ReturnType, source.ResolveMethodGenerics());
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & MethodAttributes.NewSlot) == 0)
        return MemberInheritance.Override;
      if (source.IsVirtual)
        return MemberInheritance.Virtual;
      if (source.IsAbstract)
        return MemberInheritance.Abstract;

      return MemberInheritance.Normal;
    }

    private static IEnumerable<string> ResolveGenerics(dnlib.DotNet.MethodDef source)
      => !source.HasGenericParameters
         ? Enumerable.Empty<string>()
         : source.GenericParameters.Select(x => x.Name.String);

    #endregion
  }
}
