using MarkDoc.Linkers;
using MarkDoc.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, (MarkDoc.Documentation.IDocElement? type, System.Collections.Concurrent.ConcurrentDictionary<string, MarkDoc.Documentation.IDocElement>? members)>;

namespace MarkDoc.Documentation.Xml
{
  public class DocResolver
    : IDocResolver
  {
    #region Fields

    private static readonly Cache m_documentation = new Cache();

    #endregion

    /// <inheritdoc />
    public async Task Resolve(string path)
    {
      static (A?, B?) Update<A, B>((A? a, B? b) existing, (A? a, B? b) toAdd)
        where A : class
        where B : class
        => (existing.a ?? toAdd.a, existing.b ?? toAdd.b);

      static string CacheType(string key, XElement element)
      {
        if (m_documentation.ContainsKey(key))
        {
          var (type, _) = m_documentation[key];
          type = new DocElement(element);
        }
        else
        {
          var toAdd = (new DocElement(element), new ConcurrentDictionary<string, IDocElement>());
          m_documentation.AddOrUpdate(key, toAdd, (_, y) => Update(y, toAdd));
        }

        return key;
      }

      static string RetrieveName(XElement element)
        => element.Attributes().First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase)).Value;

      static bool TypeGrouper(XElement element)
        => RetrieveName(element).First().Equals('T');

      static void CacheMember(XElement element)
      {
        static string CacheMember(string key, string name, XElement element)
        {
          var toAdd = new DocElement(element);
          if (m_documentation.ContainsKey(key))
            m_documentation[key].members?.AddOrUpdate(name[(3 + key.Length)..], toAdd, (_, y) => y ?? toAdd);
          else
          {
            var dict = new ConcurrentDictionary<string, IDocElement>();
            dict.TryAdd(name[(3 + key.Length)..], toAdd);

            m_documentation.AddOrUpdate(key, (null, dict), (_, y) => Update(y, (null, dict)));
          }

          return key;
        }

        static int ReverseIndexOf(string value, char search, int from)
        {
          for (int i = from - 1; i >= 0; i--)
          {
            if (value[i].Equals(search))
              return i;
          }

          return -1;
        }

        var name = RetrieveName(element);
        if (name[0].Equals('M'))
        {
          var brace = name.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
          if (brace != -1)
          {
            CacheMember(name[2..ReverseIndexOf(name, '.', brace)], name, element);
            return;
          }
        }

        CacheMember(name[2..name.LastIndexOf('.')], name, element);
      }

      using var file = File.OpenText(path);
      var doc = await XDocument.LoadAsync(file, LoadOptions.None, default).ConfigureAwait(false);
      foreach (var group in doc.XPathSelectElements("doc/members/member").GroupBy(TypeGrouper))
      {
        switch (group.Key)
        {
          case true:
            foreach (var item in group)
              CacheType(RetrieveName(item)[2..], item);
            break;
          case false:
            foreach (var item in group)
              CacheMember(item);
            break;
        }
      }
    }

    /// <inheritdoc />
    public bool TryFindType(IType type, out IDocElement? resultType, out IReadOnlyDictionary<string, IDocElement>? resultMembers)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));

      resultType = null;
      resultMembers = null;
      if (!m_documentation.TryGetValue(type.RawName, out var value))
        return false;

      resultType = value.type;
      resultMembers = value.members;

      return resultType != null;
    }
  }
}
