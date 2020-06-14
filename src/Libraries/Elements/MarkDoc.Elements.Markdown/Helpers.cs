using System.Text.RegularExpressions;

namespace MarkDoc.Elements.Markdown
{
  internal static class Helpers
  {
    private static readonly Regex REGEX_BRACES = new Regex("<|>");
    private static readonly MatchEvaluator EVAL_BRACES =
      match => match.Value switch
      {
        "<" => "&lt;",
        ">" => "&gt;",
        _ => match.Value
      };
    private static readonly Regex REGEX_NEWLINES = new Regex("\r\n|\n");
    private static readonly MatchEvaluator EVAL_NEWLINES =
      match => match.Value switch
      {
        "\r\n" => "<br>",
        "\n" => "<br>",
        _ => match.Value
      };

    public static string ToHeading(this string heading, int level)
      => $"{new string('#', level + 1)} {heading}";

    public static string CleanInvalid(this string text)
      => REGEX_BRACES.Replace(text, EVAL_BRACES);

    public static string ReplaceNewline(this string text)
      => REGEX_NEWLINES.Replace(text, EVAL_NEWLINES);
  }
}
