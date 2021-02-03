using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Members
{
  /// <summary>
  /// Interface for properties
  /// </summary>
  public interface IProperty
    : IMember
  {
    /// <summary>
    /// Property visibility
    /// </summary>
    MemberInheritance Inheritance { get; }

    /// <summary>
    /// Property type
    /// </summary>
    IResType Type { get; }

    /// <summary>
    /// Determines whether the property is readonly
    /// </summary>
    /// <remarks>
    /// Applicable only for <see cref="IStruct"/> properties
    /// </remarks>
    bool IsReadOnly { get; }

    /// <summary>
    /// Property get accessor type
    /// </summary>
    AccessorType? GetAccessor { get; }

    /// <summary>
    /// Property set accessor type
    /// </summary>
    AccessorType? SetAccessor { get; }

    /// <summary>
    /// Is the property setter an init type
    /// </summary>
    bool IsSetInit { get; }
  }
}
