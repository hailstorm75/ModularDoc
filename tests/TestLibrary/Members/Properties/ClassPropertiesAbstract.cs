namespace TestLibrary.Members.Properties
{
  public abstract class ClassPropertiesAbstract
    : ClassPropertiesBase
  {
    public string PropertyNormal { get; set; } = null!;
    public virtual string PropertyVirtual { get; set; } = null!;
    public abstract string PropertyAbstract { get; set; }
    public override string PropertyOverride { get; set; } = null!;
  }
}