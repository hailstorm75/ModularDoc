using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using MarkDoc.Helpers;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib
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
    public bool IsOperator { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal MethodDef(IResolver resolver, dnlib.DotNet.MethodDef source)
      : base(resolver, source, ResolveName(source, out var isOperator))
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      IsOperator = isOperator;
      IsAsync = ResolveAsync(source);
      Inheritance = ResolveInheritance(source);
      Generics = ResolveGenerics(source).ToReadOnlyCollection();
      Returns = ResolveReturn(source);
    }

    #region Methods

    private static string ResolveOperator(string name, out bool isOperator)
    {
      isOperator = true;

      string Pass(ref bool isOperator)
      {
        isOperator = false;
        return name;
      }

      return name.ToUpperInvariant() switch
      {
        "OP_IMPLICIT" => "",
        "OP_EXPLICIT" => "",
        "OP_ADDITION" => "+",
        "OP_SUBTRACTION" => "-",
        "OP_MULTIPLY" => "*",
        "OP_DIVISION" => "/",
        "OP_MODULUS" => "%",
        "OP_EXCLUSIVEOR" => "^",
        "OP_BITWISEAND" => "&",
        "OP_BITWISEOR" => "|",
        "OP_LOGICALAND" => "&&",
        "OP_LOGICALOR" => "||",
        "OP_LOGICALNOT" => "!",
        "OP_ASSIGN" => "=",
        "OP_LEFTSHIFT" => "<<",
        "OP_RIGHTSHIFT" => ">>",
        "OP_SIGNEDRIGHTSHIFT" => "",
        "OP_UNSIGNEDRIGHTSHIFT" => "",
        "OP_EQUALITY" => "==",
        "OP_GREATERTHAN" => ">",
        "OP_LESSTHAN" => "<",
        "OP_INEQUALITY" => "!=",
        "OP_GREATERTHANOREQUAL" => ">=",
        "OP_LESSTHANOREQUAL" => "<=",
        "OP_MULTIPLICATIONASSIGNMENT" => "*=",
        "OP_SUBTRACTIONASSIGNMENT" => "-=",
        "OP_EXCLUSIVEORASSIGNMENT" => "^=",
        "OP_LEFTSHIFTASSIGNMENT" => "<<=",
        "OP_MODULUSASSIGNMENT" => "%=",
        "OP_ADDITIONASSIGNMENT" => "+=",
        "OP_BITWISEANDASSIGNMENT" => "&=",
        "OP_BITWISEORASSIGNMENT" => "|=",
        "OP_COMMA" => ",",
        "OP_DIVISIONASSIGNMENT" => "/=",
        "OP_DECREMENT" => "--",
        "OP_INCREMENT" => "++",
        "OP_UNARYNEGATION" => "-",
        "OP_UNARYPLUS" => "+",
        "OP_ONESCOMPLEMENT" => "~",
        _ => Pass(ref isOperator)
      };
    }

    private static string ResolveName(dnlib.DotNet.MethodDef source, out bool isOperator)
      => ResolveOperator(source.Name, out isOperator);

    private static bool ResolveAsync(dnlib.DotNet.MethodDef source)
    {
      // TODO: Check
      return source.CustomAttributes.Find(nameof(AsyncStateMachineAttribute)) != null;
    }

    private IResType? ResolveReturn(dnlib.DotNet.MethodDef source)
    {
      if (source.ReturnType.TypeName.Equals("System.Void", StringComparison.InvariantCultureIgnoreCase))
        return null;
      return Resolver.Resolve(source.ReturnType, source.ResolveMethodGenerics());
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & dnlib.DotNet.MethodAttributes.NewSlot) == 0)
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
