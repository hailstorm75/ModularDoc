namespace TestLibrary.Classes
{
  public class ClassComplexInheritance
    : A, C
  {

  }

  public interface A : B {}
  public interface B {}
  public interface C : D, E {}
  public interface D : F {}
  public interface E {}
  public interface F {}
}