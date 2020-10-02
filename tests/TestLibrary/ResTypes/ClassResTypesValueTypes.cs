// ReSharper disable All

using System.Runtime.Remoting;

#pragma warning disable 8618

namespace TestLibrary.ResTypes
{
  public class ClassResTypesValueTypes
  {
    private string m_refString = string.Empty;
    public ref string MethodRefString() => ref m_refString;
    public byte MethodByte() => default;
    public sbyte MethodSbyte() => default;
    public char MethodChar() => default;
    public bool MethodBool() => default;
    public string MethodString() => "";
    public object MethodObject(int a) => new object();
    public short MethodShort() => default;
    public ushort MethodUShort() => default;
    public int MethodInt() => default;
    public uint MethodUInt() => default;
    public long MethodLong() => default;
    public ulong MethodULong() => default;
    public float MethodFloat() => default;
    public double MethodDouble() => default;
    public decimal MethodDecimal() => default;
    public dynamic MethodDynamic(int a) => "";
    // public void MethodDynamicArg(dynamic a) { }
    // public void MethodDynamicArg(dynamic a, int b) { }
    // public void MethodDynamicArg(int a, dynamic b) { }
    // public void MethodDynamicArg2Dyn(dynamic a, dynamic b) { }
    //
    // public object PropertyObjectGetSet { get; set; }
    // public object PropertyObjectGet { get; }
    // public object PropertyObjectSet { private get; set; }
    // public dynamic PropertyDynamicGetSet { get; set; }
    // public dynamic PropertyDynamicGet { get; }
    // public dynamic PropertyDynamicSet { private get; set; }
    //
    // public delegate object DelegateObjectA();
    // public delegate object DelegateObjectB(int a);
    // public delegate dynamic DelegateDynamicA();
    // public delegate dynamic DelegateDynamicB(int a);
  }
}