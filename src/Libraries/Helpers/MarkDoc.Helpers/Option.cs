using System;

namespace MarkDoc.Helpers
{
  public readonly struct Option<T>
  {
    private readonly T? m_value;

    public bool IsEmpty => m_value is null;

    public bool IsPresent => !IsEmpty;

    private Option(T? value) => m_value = value;

    public static Option<T> Of(T? value)
      => new(value);

    public static Option<T> OfEmpty()
      => new(default);

    public T Get()
    {
      if (IsEmpty)
        throw new Exception();

      return m_value!;
    }
  }
}
