using System;
using System.Threading;

namespace MarkDoc.Helpers
{
  /// <summary>
  /// Lazy Singleton
  /// </summary>
  /// <typeparam name="T">Generic instance type</typeparam>
  public abstract class LazySingleton<T>
    where T : LazySingleton<T>, new()
  {
    #region Fields

    /// <summary>
    /// Interlocked maximum initialized number
    /// </summary>
    private const int MAX_INITIALIZED_COUNT = 1;

    /// <summary>
    /// Lazily creates instance
    /// </summary>
    private static readonly Lazy<T> LAZY_INSTANCE = new Lazy<T>(() => new T(), LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current initialized count number
    /// </summary>
    private int m_currentInitializedCount;

    #endregion

    #region Properties

#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// Lazy instance
    /// </summary>
    public static T Instance
      => LAZY_INSTANCE.Value;
#pragma warning restore CA1000 // Do not declare static members on generic types

    #endregion

    protected LazySingleton()
      => Initialization();

    #region Methods

    private void Initialization()
    {
      if (MAX_INITIALIZED_COUNT == Interlocked.Add(ref m_currentInitializedCount, MAX_INITIALIZED_COUNT))
        InitialInConstructor();
    }

    protected virtual void InitialInConstructor() { }

    #endregion
  }
}
