using System.Text.RegularExpressions;

namespace MarkDoc.Elements.Markdown
{
  internal static class Helpers
  {
    private static readonly Regex m_regex = new Regex("<|>");
    private static readonly MatchEvaluator eval =
      match => match.Value switch
      {
        "<" => "&lt;",
        ">" => "&gt;",
        _ => match.Value
      };

    public static string ToHeading(this string heading, int level)
      => $"{new string('#', level + 1)} {heading}";

    public static string CleanInvalid(this string text)
      => m_regex.Replace(text, eval);
  }
}
