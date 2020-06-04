using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Markdown
{
  internal static class Helpers
  {
    public static string ToHeading(this string heading, int level)
      => $"{new string('#', level + 1)} {heading}";
  }
}
