using System;

namespace TestLibrary.Classes
{
  public class InheritingClassWithBaseEmpty
    : ClassTypePublicAbstract
  {
    /// <inheritdoc />
    public override string AbstractProperty { get; set; } = null!;

    /// <inheritdoc />
    public override event EventHandler AbstractEvent = null!;

    /// <inheritdoc />
    public override void AbstractMethod()
    {
      throw new NotImplementedException();
    }

  }
}
