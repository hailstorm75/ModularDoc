// ReSharper disable All

using System;

#pragma warning disable 67
namespace TestLibrary.Records
{
  public record RecordParent
  {
    public record NestedRecordPublic
    {
    }

    internal record NestedRecordInternal
    {
    }

    protected record NestedRecordProtected
    {
    }

    protected internal record NestedRecordProtectedInternal
    {
    }

    public record NestedGenericRecord<T1, T2>
      where T2 : IDisposable
    {
    }
  }
}