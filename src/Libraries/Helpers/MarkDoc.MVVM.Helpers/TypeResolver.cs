using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using MarkDoc.ViewModels;
using MarkDoc.Views;

namespace MarkDoc.MVVM.Helpers
{
  /// <summary>
  /// Helper class for resolving types
  /// </summary>
  [SuppressMessage("ReSharper","MemberCanBePrivate.Global")]
  public static class TypeResolver
  {
    #region Fields

    private static bool m_isInitialized;
    private static IContainer? m_container;

    #endregion

    /// <summary>
    /// Initializes the type model resolver
    /// </summary>
    /// <param name="container">DI container instance</param>
    public static void Initialize(IContainer container)
    {
      if (m_isInitialized)
        throw new NotSupportedException("Resolver was initialized before");

      m_container = container;
      m_isInitialized = true;
    }

    /// <summary>
    /// Resolves instance based on the given type
    /// </summary>
    /// <typeparam name="T">Type to resolve</typeparam>
    /// <returns>Resolved type instance</returns>
    /// <exception cref="NotSupportedException">The given type is not registered</exception>
    [JetBrains.Annotations.NotNull]
    public static T Resolve<T>()
    {
      if (!m_isInitialized)
        throw new NotSupportedException("Resolver was not initialized");
      if (m_container is null)
        throw new NotSupportedException("Resolver was invalidly initialized");

      using var scope = m_container.BeginLifetimeScope();
#pragma warning disable 8714
      return scope.Resolve<T>() ?? throw new InvalidOperationException("Given type is not registered");
#pragma warning restore 8714
    }

    /// <summary>
    /// Retrieves an instance of the given <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to resolve</param>
    /// <returns>Resolved instance</returns>
    /// <exception cref="NotSupportedException">The given type is not registered</exception>
    public static object Resolve(Type type)
    {
      if (!m_isInitialized)
        throw new NotSupportedException("Resolver was not initialized");
      if (m_container is null)
        throw new NotSupportedException("Resolver was invalidly initialized");

      using var scope = m_container.BeginLifetimeScope();
      return scope.Resolve(type);
    }

    /// <summary>
    /// Resolves an instance of the view
    /// </summary>
    /// <param name="type">View interface type</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">The given type is not valid</exception>
    public static IView<IViewModel> ResolveView(Type type)
    {
      if (!type.IsInterface)
        throw new NotSupportedException("Generic type is not an interface");

      return (IView<IViewModel>)Resolve(type);
    }

    /// <summary>
    /// Resolves an instance of the view model
    /// </summary>
    /// <param name="type">View model interface type</param>
    /// <returns>Resolved view model instance</returns>
    /// <exception cref="NotSupportedException">The given type is not valid</exception>
    public static IViewModel ResolveViewModel(Type type)
    {
      if (type != typeof(IViewModel))
        throw new NotSupportedException($"The given {type} must inherit a {nameof(IViewModel)}");
      if (!type.IsInterface)
        throw new NotSupportedException("Generic type is not an interface");

      return (IViewModel)Resolve(type);
    }

    /// <summary>
    /// Resolves an instance of the view model
    /// </summary>
    /// <typeparam name="TViewModel">View model interface instance</typeparam>
    /// <returns>Resolved view model instance</returns>
    [JetBrains.Annotations.NotNull]
    public static TViewModel ResolveViewModel<TViewModel>()
      where TViewModel : IViewModel
    {
      if (!typeof(TViewModel).IsInterface)
        throw new NotSupportedException("Generic type is not an interface");

      return Resolve<TViewModel>();
    }
  }
}