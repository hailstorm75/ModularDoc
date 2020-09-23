namespace TestLibrary.Members.Methods
{
  public abstract class ClassMethodsAbstract
    : ClassMethodsBase
  {
    public void MethodNormal() { }
    public abstract void MethodAbstract();
    public virtual void MethodVirtual() { }
    public override void MethodOverride() { }
  }
}