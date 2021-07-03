using System;

namespace TestDocLibrary
{
  /// <summary>
  /// Class summary
  /// </summary>
  /// <remarks>
  /// Class remarks
  /// </remarks>
  /// <example>
  /// Class example
  /// </example>
  /// <typeparam name="TC">Class type param</typeparam>
  public class SimpleClass<TC>
  {
    /// <summary>
    /// Property summary
    /// </summary>
    /// <value>
    /// Property value
    /// </value>
    public int SimpleProperty { get; set; }

    /// <summary>
    /// Method summary
    /// </summary>
    /// <remarks>
    /// Method remarks
    /// </remarks>
    /// <example>
    /// Method example
    /// </example>
    /// <typeparam name="T">Method type param</typeparam>
    /// <param name="param">Method param</param>
    /// <returns>
    /// Method return
    /// </returns>
    /// <exception cref="ArgumentException">Argument exception</exception>
    /// <exception>Unknown exception</exception>
    /// <seealso cref="SimpleClass{TC}"/>
    public int SimpleMethod<T>(int param)
      => throw new NotImplementedException();
  }
}