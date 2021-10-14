using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Core
{
  public interface IView<out TViewModel>
    : IView
    where TViewModel : IViewModel
  {
    /// <summary>
    /// View model
    /// </summary>
    public TViewModel ViewModel { get; }
  }

  public interface IView
  {
    public void SetArguments(IEnumerable<string> arguments)
      => SetNamedArguments(arguments.ToDictionary(argument => argument, _ => string.Empty));

    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}
