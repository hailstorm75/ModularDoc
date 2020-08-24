using System.Text.RegularExpressions;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Helper class for elements
  /// </summary>
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

    /// <summary>
    /// Converts given <paramref name="heading"/> string to a markdown heading of a given <paramref name="level"/>
    /// </summary>
    /// <param name="heading">Heading to use</param>
    /// <param name="level">Heading level to set</param>
    /// <returns>Markdown heading</returns>
    public static string ToHeading(this string heading, int level)
      => $"{new string('#', level + 1)} {heading}";

    /// <summary>
    /// Removes invalid characters from given <paramref name="text"/>
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <returns>Processed text</returns>
    public static string CleanInvalid(this string text)
      => REGEX_BRACES.Replace(text, EVAL_BRACES);

    /// <summary>
    /// Replaces the OS newlines with markdown line breaks in given <paramref name="text"/>
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <returns>Processed text</returns>
    public static string ReplaceNewline(this string text)
      => REGEX_NEWLINES.Replace(text, EVAL_NEWLINES);
  }
}
