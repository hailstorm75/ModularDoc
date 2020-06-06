using System.Text.RegularExpressions;

namespace MarkDoc.Elements.Markdown
{
  internal static class Helpers
  {
    private static readonly Regex m_regexBraces = new Regex("<|>");
    private static readonly MatchEvaluator m_evalBraces =
      match => match.Value switch
      {
        "<" => "&lt;",
        ">" => "&gt;",
        _ => match.Value
      };
    private static readonly Regex m_regexNewlines = new Regex("\r\n|\n");
    private static readonly MatchEvaluator m_evalNewlines =
      match => match.Value switch
      {
        "\r\n" => "<br>",
        "\n" => "<br>",
        _ => match.Value
      };

    public static string ToHeading(this string heading, int level)
      => $"{new string('#', level + 1)} {heading}";

    public static string CleanInvalid(this string text)
      => m_regexBraces.Replace(text, m_evalBraces);

    public static string ReplaceNewline(this string text)
      => m_regexNewlines.Replace(text, m_evalNewlines);
  }
}
