using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MarkDoc.Core;
using MarkDoc.Helpers;

namespace MarkDoc.MVVM.Helpers
{
  /// <summary>
  /// Helper class for managing transitions between views
  /// </summary>
  [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
  [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
  public class NavigationManager
  {
    private readonly Dictionary<string, Type> m_types;
    private string m_previousView = string.Empty;

    public event EventHandler<ViewData>? NavigationChanged;

    /// <summary>
    /// The currently displayed page
    /// </summary>
    public string CurrentPage { get; private set; } = string.Empty;

    /// <summary>
    /// Static constructor
    /// </summary>
    public NavigationManager()
      => m_types = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Navigation event data
    /// </summary>
    public readonly struct ViewData
    {
      #region Properties

      /// <summary>
      /// View name
      /// </summary>
      public string Name { get; }

      /// <summary>
      /// Has navigation arguments
      /// </summary>
      public bool HasArguments { get; }

      /// <summary>
      /// Named arguments
      /// </summary>
      public IReadOnlyDictionary<string, string> NamedArguments { get; }

      /// <summary>
      /// Nameless arguments
      /// </summary>
      public IReadOnlyCollection<string> Arguments { get; }

      /// <summary>
      /// Determines whether the target view is hte previous one
      /// </summary>
      public bool IsNavigatingBack { get; }

      #endregion

      /// <summary>
      /// Constructor for view without arguments
      /// </summary>
      public ViewData(string name, bool isPrevious = false)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = false;
        NamedArguments = new Dictionary<string, string>();
        Arguments = Array.Empty<string>();
        IsNavigatingBack = isPrevious;
      }

      /// <summary>
      /// Constructor for view with arguments
      /// </summary>
      public ViewData(string name, IEnumerable<string> arguments, bool isPrevious = false)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = true;
        NamedArguments = new Dictionary<string, string>();
        Arguments = arguments.ToReadOnlyCollection();
        IsNavigatingBack = isPrevious;
      }

      /// <summary>
      /// Constructor for view with named arguments
      /// </summary>
      public ViewData(string name, IEnumerable<KeyValuePair<string, string>> arguments, bool isPrevious = false)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = true;
        NamedArguments = arguments.ToDictionary(arg => arg.Key, arg => arg.Value);
        Arguments = Array.Empty<string>();
        IsNavigatingBack = isPrevious;
      }
    }

    /// <summary>
    /// Register a new view to the manager
    /// </summary>
    /// <typeparam name="T">View type</typeparam>
    /// <param name="key">Key under which the view is to be registered and later referenced when navigating to it</param>
    public void Register<T>(string key)
    {
      if (m_types.ContainsKey(key))
        throw new DuplicateNameException($"The manager cannot have two entries with the same name: {key}");

      m_types.Add(key, typeof(T));
    }

    /// <summary>
    /// Navigate to a different view of a given <paramref name="name"/>
    /// </summary>
    /// <param name="name">Name of the view to navigate to</param>
    /// <returns>True if the navigation request was successful</returns>
    public void NavigateTo(string name)
    {
      if (!m_types.ContainsKey(name))
        return;

      if (NavigationChanged is null)
        throw new EntryPointNotFoundException("The application is not configured correctly as there is no-one to send the request to");

      NavigationChanged.Invoke(this, new ViewData(name, m_previousView.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
      m_previousView = CurrentPage;
      CurrentPage = name;
    }

    public void NavigateTo(string name, params string[] arguments)
    {
      if (!m_types.ContainsKey(name))
        return;

      if (NavigationChanged is null)
        throw new EntryPointNotFoundException("The application is not configured correctly as there is no-one to send the request to");

      NavigationChanged.Invoke(this, new ViewData(name, arguments, m_previousView.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
      m_previousView = CurrentPage;
      CurrentPage = name;
    }

    public void NavigateTo(string name, IReadOnlyDictionary<string, string> arguments)
    {
      if (!m_types.ContainsKey(name))
        return;

      if (NavigationChanged is null)
        throw new EntryPointNotFoundException("The application is not configured correctly as there is no-one to send the request to");

      NavigationChanged.Invoke(this, new ViewData(name, arguments, m_previousView.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
      m_previousView = CurrentPage;
      CurrentPage = name;
    }

    /// <summary>
    /// Retrieves a view of the given <paramref name="name"/>
    /// </summary>
    /// <param name="name">Name of the view to retrieve</param>
    /// <returns>Retrieved view instance</returns>
    public IView ResolveView(string name)
    {
      if (!m_types.TryGetValue(name, out var view))
        throw new KeyNotFoundException($"No views with the name {name} are registered in the manager");

      // ReSharper disable once AssignNullToNotNullAttribute
      return TypeResolver.ResolveView(view);
    }
  }
}
