namespace TestLibrary.ResTypes
{
  public class ClassResGenericValueTypes<T1, T2>
  {
    public void MethodGenericValueTypes(T1 a, T2 b)
    {
    }

    public void MethodGenericOwnValueTypes<TA, TB>(TA a, TB b)
    {
    }

    public void MethodGenericMixedValueTypes<TA, TB>(T1 a, TA b, T2 c, TB d)
    {
    }
  }
}