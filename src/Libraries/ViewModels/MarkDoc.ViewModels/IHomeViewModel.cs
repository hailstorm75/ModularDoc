using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  public interface IHomeViewModel
    : IViewModel
  {
    IReadOnlyCollection<IPlugin> Plugins { get; }
  }
}