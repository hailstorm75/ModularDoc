// ReSharper disable All
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TestLibrary.Enums
{
  public class EnumParent
  {
    public enum NestedEnumPublic
    {
      FieldA,
      FieldB
    }
    protected enum NestedEnumProtected
    {
      FieldA,
      FieldB
    }
    internal enum NestedEnumInternal
    {
      FieldA,
      FieldB
    }
    protected internal enum NestedEnumProtectedInternal
    {
      FieldA,
      FieldB
    }
    private enum NestedEnumPrivate
    {
      FieldA,
      FieldB
    }
  }
}
