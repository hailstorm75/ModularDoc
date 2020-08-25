// ReSharper disable All
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
    private enum NestedEnumPrivate
    {
      FieldA,
      FieldB
    }
  }
}
