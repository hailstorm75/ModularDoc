﻿using MarkDoc.Linkers;
using MarkDoc.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, (MarkDoc.Documentation.IDocElement? type, System.Collections.Concurrent.ConcurrentDictionary<string, MarkDoc.Documentation.IDocMember>? members)>;

namespace MarkDoc.Documentation.Xml
{
  public class DocResolver
    : IDocResolver
  {
    #region Fields

    private static readonly Cache m_documentation = new Cache();
    private readonly IResolver m_typeResolver;

    #endregion

    public DocResolver(IResolver typeResolver)
    {
      m_typeResolver = typeResolver;
    }

    /// <inheritdoc />
    public async Task Resolve(string path)
    {
      static (A?, B?) Update<A, B>((A? a, B? b) existing, (A? a, B? b) toAdd)
        where A : class
        where B : class
        => (existing.a ?? toAdd.a, existing.b ?? toAdd.b);

      string CacheType(string key, XElement element)
      {
        if (m_documentation.ContainsKey(key))
        {
          var (type, _) = m_documentation[key];
          type = new DocElement(key, element, this, m_typeResolver);
        }
        else
        {
          var toAdd = (new DocElement(key, element, this, m_typeResolver), new ConcurrentDictionary<string, IDocMember>());
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
          static string ProcessName(string key, string name)
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

          var toAdd = new DocMember(ProcessName(key, name), key[0], element);
          if (m_documentation.ContainsKey(key))
            m_documentation[key].members?.AddOrUpdate(toAdd.Name, toAdd, (_, y) => y ?? toAdd);
          else
          {
            var dict = new ConcurrentDictionary<string, IDocMember>();
            dict.TryAdd(ProcessName(key, name), toAdd);

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
    public bool TryFindType(IType type, out IDocElement? resultType)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));

      return TryFindType(type.RawName, out resultType, out var _);
    }

    internal static bool TryFindType(string type, out IDocElement? resultType, out IReadOnlyDictionary<string, IDocMember>? resultMembers)
    {
      resultType = null;
      resultMembers = null;

      if (!m_documentation.TryGetValue(type, out var value))
        return false;

      resultType = value.type;
      resultMembers = value.members;

      return resultType != null;
    }
  }
}