using System;

namespace TestLibrary.ResTypes
{
  public class ClassResTuples
  {
    public Tuple<short> MethodOneTuple() => new Tuple<short>(default);
    public Tuple<short, int> MethodTwoTuple() => new Tuple<short, int>(default, default);
    public Tuple<short, int, long> MethodThreeTuple() => new Tuple<short, int, long>(default, default, default);
    public Tuple<short, int, long, byte> MethodFourTuple() => new Tuple<short, int, long, byte>(default, default, default, default);

    public (short a, int b) MethodTwoValueTuple() => (default, default);
    public (short a, int b, long c) MethodThreeValueTuple() => (default, default, default);
    public (short a, int b, long c, byte d) MethodFourValueTuple() => (default, default, default, default);

    public (object a1, (dynamic a2, object b2) b1, dynamic c1) MethodComplexValueTuple() => (new object(), (new object(), new object()), new object());
    public Tuple<object, Tuple<dynamic, object>, dynamic> MethodComplexTuple() => Tuple.Create(new object(), Tuple.Create(new object(), new object()), new object());
  }
}