namespace ModularDoc.Members.ResolvedTypes
{
  /// <summary>
  /// Interface for resolved arrays
  /// </summary>
  public interface IResArray
  {
    /// <summary>
    /// Type of given array
    /// </summary>
    IResType ArrayType { get; }

    /// <summary>
    /// Determines whether the array is a jagged array type
    /// </summary>
    bool IsJagged { get; }

    /// <summary>
    /// Dimension of array
    /// </summary>
    int Dimension { get; }
  }
}
