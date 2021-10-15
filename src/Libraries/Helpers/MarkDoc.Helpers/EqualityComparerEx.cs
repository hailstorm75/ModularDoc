using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MarkDoc.Helpers
{
  /// <summary>
  /// Equality comparer helper class
  /// </summary>
  /// <typeparam name="T">Types to compare</typeparam>
  public class EqualityComparerEx<T>
    : IEqualityComparer<T>
  {
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static EqualityComparerEx<T> Create(params Expression<Func<T, object>>[] properties) => new EqualityComparerEx<T>(properties);
#pragma warning restore CA1000 // Do not declare static members on generic types

    private readonly PropertyInfo[] m_properties;
    private EqualityComparerEx(Expression<Func<T, object>>[] properties)
    {
      if (properties is null)
        throw new ArgumentNullException(nameof(properties));

      if (properties.Length == 0)
        throw new ArgumentOutOfRangeException(nameof(properties));

      var length = properties.Length;
      var extractions = new PropertyInfo[length];

      for (var i = 0; i < length; i++)
      {
        var property = properties[i];
        extractions[i] = ExtractProperty(property);
      }

      m_properties = extractions;
    }

    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
      if (ReferenceEquals(x, y))
        return true;

      if (x is null || y is null)
        return false;

      return m_properties.All(property => Equals(property.GetValue(x), property.GetValue(y)));
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
      var hashes = m_properties.Select(pi => pi.GetValue(obj)?.GetHashCode() ?? 0).ToArray();
      return Combine(hashes);
    }

    private static int Combine(int[] hashes)
    {
      int result = 0;
      foreach (var hash in hashes)
      {
        var rol5 = ((uint)result << 5) | ((uint)result >> 27);
        result = ((int)rol5 + result) ^ hash;
      }

      return result;
    }

    private static PropertyInfo ExtractProperty(Expression<Func<T, object>> property)
    {
      if (property.NodeType != ExpressionType.Lambda)
        ThrowEx();

      var body = property.Body;
      if (body.NodeType == ExpressionType.Convert)
        if (body is UnaryExpression unary)
          body = unary.Operand;
        else
          ThrowEx();

      if (!(body is MemberExpression member))
        ThrowEx();

      if (!(member.Member is PropertyInfo pi))
        ThrowEx();

      return pi;

      void ThrowEx()
        => throw new NotSupportedException($"The expression '{property}' isn't supported.");
    }
  }
}
