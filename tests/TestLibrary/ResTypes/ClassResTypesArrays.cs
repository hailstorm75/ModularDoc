namespace TestLibrary.ResTypes
{
  public class ClassResTypesArrays
  {
    public string[] Method1DArray() => new string[]{};
    public string[][] Method2DJaggedArray() => new string[1][];
    public string[,] Method2DArray() => new string[1,1];
    public string[][][] Method3DJaggedArray() => new string[1][][];
    public string[,,] Method3DArray() => new string[1,1,1];
  }
}