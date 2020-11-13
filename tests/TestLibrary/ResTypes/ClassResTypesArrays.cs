using System;
using System.Collections.Generic;

namespace TestLibrary.ResTypes
{
  public class ClassResTypesArrays
  {
    #region Arrays

    public string[] Method1DArray() => new string[]{};
    public string[][] Method2DJaggedArray() => new string[1][];
    public string[,] Method2DArray() => new string[1,1];
    public string[][][] Method3DJaggedArray() => new string[1][][];
    public string[,,] Method3DArray() => new string[1,1,1];

    #endregion

    #region Tuples

    public Tuple<short>[] MethodOneArrTuple() => default!;
    public Tuple<short, int>[] MethodTwoArrTuple() => default!;
    public Tuple<short, int, long>[] MethodThreeArrTuple() => default!;
    public Tuple<short, int, long, byte>[] MethodFourArrTuple() => default!;

    public (short a, int b)[] MethodTwoValueArrTuple() => default!;
    public (short a, int b, long c)[] MethodThreeValueArrTuple() => default!;
    public (short a, int b, long c, byte d)[] MethodFourValueArrTuple() => default!;

    public (object a1, (dynamic a2, object b2) b1, dynamic c1)[] MethodComplexValueArrTuple() => default!;
    public Tuple<object, Tuple<dynamic, object>, dynamic>[] MethodComplexArrTuple() => default!;

    public (object[] a1, (dynamic[] a2, object[] b2)[] b1, dynamic[] c1)[] MethodComplexValueArrTuple2() => default!;
    public Tuple<object[], Tuple<dynamic[], object[]>[], dynamic[]>[] MethodComplexArrTuple2() => default!;

    #endregion

    #region Generics

    public IEnumerable<string>[] MethodGenericStringArr() => default!;
    public IReadOnlyDictionary<object, dynamic>[] MethodGenericObjDynArr() => default!;
    public IReadOnlyDictionary<dynamic, object>[] MethodGenericDynObjArr() => default!;
    public Func<int, string, bool>[] MethodGenericIntStringBoolArr() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, dynamic>>[] MethodGenericComplexArrAReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, dynamic>>[] MethodGenericComplexArrBReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, dynamic>>[] MethodGenericComplexArrCReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, dynamic>>[] MethodGenericComplexArrDReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, object>>[] MethodGenericComplexArrEReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, object>>[] MethodGenericComplexArrFReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, object>>[] MethodGenericComplexArrGReturn() => default!;
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, object>>[] MethodGenericComplexArrHReturn() => default!;

    #endregion
  }
}