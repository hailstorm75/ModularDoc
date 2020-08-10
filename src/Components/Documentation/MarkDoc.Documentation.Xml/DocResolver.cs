using MarkDoc.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using MarkDoc.Members.Types;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, (MarkDoc.Documentation.IDocElement? type, System.Collections.Concurrent.ConcurrentDictionary<string, MarkDoc.Documentation.IDocMember>? members)>;

namespace MarkDoc.Documentation.Xml
{
  public class DocResolver
    : IDocResolver
  {
    #region Fields

    private readonly Cache m_documentation;
    private readonly IResolver m_typeResolver;

    #endregion

    public DocResolver(IResolver typeResolver)
    {
      m_typeResolver = typeResolver;
      m_documentation = new Cache();
    }

    #region Methods

    /// <inheritdoc />
    public async Task Resolve(string path)
    {
      static (TA?, TB?) Update<TA, TB>((TA? a, TB? b) existing, (TA? a, TB? b) toAdd)
        where TA : class
        where TB : class
        => (existing.a ?? toAdd.a, existing.b ?? toAdd.b);

      void CacheType(string key, XElement element)
      {
        var toAdd = (new DocElement(key, element, this, m_typeResolver), new ConcurrentDictionary<string, IDocMember>());
        m_documentation.AddOrUpdate(key, toAdd, (_, y) => Update(y, toAdd));
      }

      static string RetrieveName(XElement element)
        => element.Attributes().First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase)).Value;

      static bool TypeGrouper(XElement element)
        => RetrieveName(element).First().Equals('T');

      void CacheMember(XElement element)
      {
        var name = RetrieveName(element);

        void Cache(string key)
        {
          string ProcessName()
          {
            var memberNameRaw = name[(3 + key.Length)..];
            if (name.First().Equals('M'))
            {
              if (!memberNameRaw.EndsWith(')'))
                memberNameRaw += "()";
              else
              {
                var braceIndex = memberNameRaw.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
                var memberName = memberNameRaw.Remove(braceIndex);
                var index = memberName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
                if (index != -1)
                  memberNameRaw = memberName.Remove(index) + memberNameRaw.Substring(braceIndex);
              }
            }
            return memberNameRaw;
          }

          var toAdd = new DocMember(ProcessName(), key[0], element);
          if (m_documentation.ContainsKey(key))
            m_documentation[key].members?.AddOrUpdate(toAdd.Name, toAdd, (_, y) => y ?? toAdd);
          else
          {
            var dict = new ConcurrentDictionary<string, IDocMember>();
            dict.TryAdd(ProcessName(), toAdd);

            m_documentation.AddOrUpdate(key, (null, dict), (_, y) => Update(y, (null, dict)));
          }
        }

        static int ReverseIndexOf(string value, char search, int from)
        {
          for (var i = from - 1; i >= 0; i--)
            if (value[i].Equals(search))
              return i;

          return -1;
        }

        if (name[0].Equals('M'))
        {
          var brace = name.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
          if (brace != -1)
          {
            Cache(name[2..ReverseIndexOf(name, '.', brace)]);
            return;
          }
        }

        Cache(name[2..name.LastIndexOf('.')]);
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
    public bool TryFindType(IType type, out IDocElement? resultType)
    {
      if (type is null)
        throw new ArgumentNullException(nameof(type));

      resultType = null;

      if (!m_documentation.TryGetValue(type.RawName, out var value))
        return false;

      if (value.type is null)
      {
        var doc = new DocElement(type.RawName, this, m_typeResolver);
        m_documentation.AddOrUpdate(type.RawName, (resultType, value.members), (x, y) => (doc, y.members));

        resultType = doc;
      }
      else
        resultType = value.type;

      return resultType != null;
    }

    internal bool TryFindMembers(string type, out IReadOnlyDictionary<string, IDocMember>? resultMembers)
    {
      resultMembers = null;

      if (!m_documentation.TryGetValue(type, out var value))
        return false;

      resultMembers = value.members;

      return resultMembers != null;
    }

    #endregion
  }
}
