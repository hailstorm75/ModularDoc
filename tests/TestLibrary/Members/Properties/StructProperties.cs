// ReSharper disable All
namespace TestLibrary.Members.Properties
{
  public struct StructProperties
  {
    private static int MY_NUMBER = 42;
    
    public string PropertyGetSet { get; set; }
    public string PropertyGet { get; }
    public string PropertySet { private get; set; }
    public readonly int PropertyReadonly => default;
    public readonly int PropertyFullReadonly
    {
      get => MY_NUMBER;
      set => MY_NUMBER = value;
    }
    public int PropertyFullReadonlyGet
    {
      readonly get => MY_NUMBER;
      set => MY_NUMBER = value;
    }
  }
}