// ReSharper disable All
namespace TestLibrary.Members.Properties
{
  public struct StructProperties
  {
    public string PropertyGetSet { get; set; }
    public string PropertyGet { get; }
    public string PropertySet { private get; set; }
  }
}