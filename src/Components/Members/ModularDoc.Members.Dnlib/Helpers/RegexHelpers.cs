using System.Text.RegularExpressions;

namespace ModularDoc.Members.Dnlib.Helpers;

public static class RegexHelpers
{
  public static readonly Regex FILE_ACCESSOR_REGEX = new (@"^\<\w+\>.{65}__(?<typeName>.*)");
}