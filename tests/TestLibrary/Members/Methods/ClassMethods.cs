using System.Text;
using System.Threading.Tasks;
// ReSharper disable All
#pragma warning disable 660,661

namespace TestLibrary.Members.Methods
{
  public class ClassMethods
  {
    public void MethodPublic() { }
    protected void MethodProtected() { }
    protected internal void MethodProtectedInternal() { }
    internal void MethodInternal() { }

    public static void MethodStatic() { }
    public async Task MethodAsync() => await Task.Delay(1);

    public static ClassMethods? operator ^(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator &(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator |(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator %(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator *(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator /(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator +(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator -(ClassMethods? left, ClassMethods? right) => default;
    public static ClassMethods? operator -(ClassMethods? item) => default;
    public static ClassMethods? operator ~(ClassMethods? item) => default;
    public static ClassMethods? operator <<(ClassMethods? item, int a) => default;
    public static ClassMethods? operator >>(ClassMethods? item, int a) => default;
    public static bool operator ==(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator !=(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator >=(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator <=(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator >(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator <(ClassMethods? left, ClassMethods? right) => default;
    public static bool operator !(ClassMethods? item) => default;
    public static ClassMethods? operator ++(ClassMethods? item) => default;
    public static ClassMethods? operator --(ClassMethods? item) => default;
    public static implicit operator int(ClassMethods item) => default;
    public static explicit operator uint(ClassMethods item) => default;
  }
}