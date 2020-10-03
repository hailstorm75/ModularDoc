namespace TestLibrary.Members.Arguments
{
  public class ClassArguments
  {
    public void MethodOneArg(int a) { }
    public delegate void DelegateOneArg(int a);

    public void MethodTwoArg(int a, bool b) { }
    public delegate void DelegateTwoArg(int a, bool b);

    public void MethodModArgs(int a, in int b, out int c, ref int d, int e = 0) => c = 0;
    public delegate void DelegateModArgs(int a, in int b, out int c, ref int d, int e = 0);

    public void MethodParamArg(params int[] a) { }
    public delegate void DelegateParamArg(params int[] a);
  }
}