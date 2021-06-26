// ReSharper disable All
#pragma warning disable 67
using System;

namespace TestLibrary.Records
{
  public record GenericRecord<T1, T2>
    where T2 : IDisposable
  {
  }
}