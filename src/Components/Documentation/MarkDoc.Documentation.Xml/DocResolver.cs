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
using Cache =
  System.Collections.Concurrent.ConcurrentDictionary<string, (MarkDoc.Documentation.IDocElement? type,
    System.Collections.Concurrent.ConcurrentDictionary<string, MarkDoc.Documentation.IDocMember>? members)>;

namespace MarkDoc.Documentation.Xml
{
  /// <summary>
  /// Documentation resolver
  /// </summary>
  public class DocResolver
    : IDocResolver
  {
    #region Fields

    private readonly Cache m_documentation;
    private readonly IResolver m_typeResolver;

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="typeResolver">Injected type resolver</param>
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
        // Prepare the type to cache
        var toAdd =
          (new DocElement(key, element, this, m_typeResolver), new ConcurrentDictionary<string, IDocMember>());
        // Cache the type
        m_documentation.AddOrUpdate(key, toAdd, (_, y) => Update(y, toAdd));
      }

      static string RetrieveName(XElement element)
        => element.Attributes()
          .First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase))
          .Value;

      static bool TypeGrouper(XElement element)
        => RetrieveName(element).First().Equals('T');

      void CacheMember(XElement element)
      {
        // Get the member name
        var name = RetrieveName(element);

        void Cache(string key)
        {
          string ProcessName()
          {
            // Get the member name without the identifier key
            var memberNameRaw = name[(3 + key.Length)..];

            // If the member is not a method..
            if (!name.First().Equals('M'))
              // return the member name
              return memberNameRaw;
            // if the method has no arguments..
            if (!memberNameRaw.EndsWith(')'))
              // and empty braces to the raw name
              memberNameRaw += "()";
            // otherwise..
            else
            {
              // find the method braces
              var braceIndex = memberNameRaw.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
              // get the method name
              var memberName = memberNameRaw.Remove(braceIndex);
              // find the generics
              var genericIndex = memberName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
              // if there are no generics..
              if (genericIndex != -1)
                // remove the generics
                memberNameRaw = memberName.Remove(genericIndex) + memberNameRaw.Substring(braceIndex);
            }

            // Return the processed name
            return memberNameRaw;
          }

          // Prepare the member to cache
          var toAdd = new DocMember(ProcessName(), key[0], element);
          // If the member is cached..
          if (m_documentation.ContainsKey(key))
            // add the member to the cache
            m_documentation[key].members?.AddOrUpdate(toAdd.Name, toAdd, (_, y) => y ?? toAdd);
          // Otherwise..
          else
          {
            var dict = new ConcurrentDictionary<string, IDocMember>();
            dict.TryAdd(ProcessName(), toAdd);

            // Cache the member
            m_documentation.AddOrUpdate(key, (null, dict), (_, y) => Update(y, (null, dict)));
          }
        }

        static int ReverseIndexOf(string value, char search, int from)
        {
          // For every character in reverse order..
          for (var i = from - 1; i >= 0; i--)
            // if the character is matched..
            if (value[i].Equals(search))
              // return its index
              return i;

          // Not found
          return -1;
        }

        // If the member is a method..
        if (name[0].Equals('M'))
        {
          // find the method brace
          var brace = name.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
          // if there is a brace..
          if (brace != -1)
          {
            // cache the method
            Cache(name[2..ReverseIndexOf(name, '.', brace)]);
            // exit
            return;
          }
        }

        // Cache the member
        Cache(name[2..name.LastIndexOf('.')]);
      }

      // Open the XML documentation file
      using var file = File.OpenText(path);
      // Load the XML documentation file
      var doc = await XDocument.LoadAsync(file, LoadOptions.None, default).ConfigureAwait(false);
      // For every documentation item grouped by
      foreach (var group in doc.XPathSelectElements("doc/members/member").GroupBy(TypeGrouper))
        // Depending on whether the group is a type..
        switch (@group.Key)
        {
          // Is a type
          case true:
            // for every documented type..
            foreach (var item in @group)
              // cache the type
              CacheType(RetrieveName(item)[2..], item);
            break;
          // Is not a type
          case false:
            // for every documented member..
            foreach (var item in @group)
              // cache the member
              CacheMember(item);
            break;
        }
    }

    /// <inheritdoc />
    public bool TryFindType(IType type, out IDocElement? resultType)
    {
      // If the type is null..
      if (type is null)
        // throw an exception
        throw new ArgumentNullException(nameof(type));

      // Assume no type is found
      resultType = null;

      // If the type is unknown..
      if (!m_documentation.TryGetValue(type.RawName, out var value))
        // the type is not found
        return false;

      // If the type is not documented..
      if (value.type is null)
      {
        // prepare an empty type for caching
        var doc = new DocElement(type.RawName, this, m_typeResolver);
        // cache the type
        m_documentation.AddOrUpdate(type.RawName, (resultType, value.members), (x, y) => (doc, y.members));

        // Return the empty type
        resultType = doc;
      }
      // Otherwise..
      else
        // return the found type
        resultType = value.type;

      // Return whether the type is found
      return resultType != null;
    }

    internal bool TryFindMembers(string type, out IReadOnlyDictionary<string, IDocMember>? resultMembers)
    {
      // Assume that the member is not found
      resultMembers = null;

      // If the member declaring type is unknown..
      if (!m_documentation.TryGetValue(type, out var value))
        // the member is not found
        return false;

      // Return the found member
      resultMembers = value.members;

      // Return whether the member is found
      return resultMembers != null;
    }

    #endregion
  }
}