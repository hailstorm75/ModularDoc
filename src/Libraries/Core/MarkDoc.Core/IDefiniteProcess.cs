namespace MarkDoc.Core;

public interface IDefiniteProcess
  : IProcess
{
  double Complete { get; }
  int Current { get; }
  int Max { get; }

  void IncreaseCompletion();
}