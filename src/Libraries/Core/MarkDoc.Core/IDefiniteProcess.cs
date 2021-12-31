namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for definite processes
  /// </summary>
  public interface IDefiniteProcess
    : IProcess
  {
    /// <summary>
    /// Percentage complete
    /// </summary>
    double Complete { get; }

    /// <summary>
    /// Completed parts so far
    /// </summary>
    int Current { get; }

    /// <summary>
    /// Number of parts to be completed
    /// </summary>
    int Max { get; }

    /// <summary>
    /// Increases the number of complete parts
    /// </summary>
    void IncreaseCompletion();
  }
}