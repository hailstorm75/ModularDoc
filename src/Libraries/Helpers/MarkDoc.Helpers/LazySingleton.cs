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
    private const int m_maxInitializedCount = 1;

    /// <summary>
    /// Lazily creates instance
    /// </summary>
    private static readonly Lazy<T> m_lazyInstance = new Lazy<T>(() => new T(), LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current initialized count number
    /// </summary>
    private int m_currentInitializedCount = 0;

    #endregion

    #region Properties

#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// Lazy instance
    /// </summary>
    public static T Instance
      => m_lazyInstance.Value;
#pragma warning restore CA1000 // Do not declare static members on generic types

    #endregion

    protected LazySingleton()
      => Initialization();

    #region Methods

    private void Initialization()
    {
      if (m_maxInitializedCount == Interlocked.Add(ref m_currentInitializedCount, m_maxInitializedCount))
        InitialInConstructor();
    }

    protected virtual void InitialInConstructor() { }

    #endregion
  }
}
