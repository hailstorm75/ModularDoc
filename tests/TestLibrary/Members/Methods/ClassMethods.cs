using System.Threading.Tasks;

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
  }
}