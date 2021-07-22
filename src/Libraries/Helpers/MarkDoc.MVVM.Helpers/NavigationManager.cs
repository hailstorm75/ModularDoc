using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

      #endregion

      /// <summary>
      /// Constructor for view without arguments
      /// </summary>
      public ViewData(string name)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = false;
        NamedArguments = new Dictionary<string, string>();
        Arguments = Array.Empty<string>();
      }

      /// <summary>
      /// Constructor for view with arguments
      /// </summary>
      public ViewData(string name, IEnumerable<string> arguments)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = true;
        NamedArguments = new Dictionary<string, string>();
        Arguments = arguments.ToReadOnlyCollection();
      }

      /// <summary>
      /// Constructor for view with named arguments
      /// </summary>
      public ViewData(string name, IEnumerable<KeyValuePair<string, string>> arguments)
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HasArguments = true;
        NamedArguments = arguments.ToDictionary(arg => arg.Key, arg => arg.Value);
        Arguments = Array.Empty<string>();
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

      NavigationChanged.Invoke(this, new ViewData(name));
      CurrentPage = name;
    }

    public void NavigateTo(string name, params string[] arguments)
    {
      if (!m_types.ContainsKey(name))
        return;

      if (NavigationChanged is null)
        throw new EntryPointNotFoundException("The application is not configured correctly as there is no-one to send the request to");

      NavigationChanged.Invoke(this, new ViewData(name, arguments));
      CurrentPage = name;
    }

    public void NavigateTo(string name, IReadOnlyDictionary<string, string> arguments)
    {
      if (!m_types.ContainsKey(name))
        return;

      if (NavigationChanged is null)
        throw new EntryPointNotFoundException("The application is not configured correctly as there is no-one to send the request to");

      NavigationChanged.Invoke(this, new ViewData(name, arguments));
      CurrentPage = name;
    }

    /// <summary>
    /// Retrieves a view of the given <paramref name="name"/>
    /// </summary>
    /// <param name="name">Name of the view to retrieve</param>
    /// <returns>Retrieved view instance</returns>
    public object ResolveView(string name)
    {
      if (!m_types.TryGetValue(name, out var view))
        throw new KeyNotFoundException($"No views with the name {name} are registered in the manager");

      return TypeResolver.ResolveView(view!);
    }
  }

}
