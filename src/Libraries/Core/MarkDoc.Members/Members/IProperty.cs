using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;

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
    /// Property get accessor type
    /// </summary>
    AccessorType? GetAccessor { get; }

    /// <summary>
    /// Property set accessor type
    /// </summary>
    AccessorType? SetAccessor { get; }
  }
}
