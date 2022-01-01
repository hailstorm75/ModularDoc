using MarkDoc.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using MarkDoc.Core;
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
    private readonly IDocSettings m_settings;
    private readonly IMarkDocLogger m_logger;
    private readonly IDefiniteProcess m_processLogger;

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="typeResolver">Type resolver instance</param>
    /// <param name="settings">Documentation resolver settings</param>
    /// <param name="logger">Operation logger instance</param>
    /// <param name="processLogger">Instance of process logger</param>
    public DocResolver(IResolver typeResolver, IDocSettings settings, IMarkDocLogger logger, IDefiniteProcess processLogger)
    {
      m_typeResolver = typeResolver;
      m_settings = settings;
      m_logger = logger;
      m_processLogger = processLogger;
      m_documentation = new Cache();
    }

    #region Methods

    public async Task ResolveAsync()
    {
      m_processLogger.State = IProcess.ProcessState.Running;

      await Task.WhenAll(m_settings.Paths.Select(ResolveAsync)).ConfigureAwait(false);

      m_processLogger.State = IProcess.ProcessState.Success;
    }

    /// <inheritdoc />
    public async Task ResolveAsync(string path)
    {
      m_logger.Info($"Processing file: '{path}'");

      // Open the XML documentation file
      using var file = File.OpenText(path);
      // Load the XML documentation file
      var doc = await XDocument.LoadAsync(file, LoadOptions.None, default).ConfigureAwait(false);
      // For every documentation item grouped by
      foreach (var group in doc.XPathSelectElements("doc/members/member").GroupBy(TypeGrouper))
      {
        // Depending on whether the group is a type or member, choose the corresponding caching method
        Func<XElement, string> cache = group.Key switch
        {
          true => CacheType,
          false => CacheMember
        };

        // for every documented item..
        foreach (var item in @group)
        {
          // cache the item
          var itemName = cache(item);
          // log cached item name
          m_logger.Info($"Cached item '{itemName}' from file '{path}'");
        }
      }

      m_processLogger.IncreaseCompletion();
    }

    private string CacheMember(XElement element)
    {
      // Get the member name
      var name = RetrieveName(element);

      // If the member is a method..
      if (name[0].Equals('M'))
      {
        // find the method brace
        var brace = name.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
        // if there is a brace..
        if (brace != -1)
          // cache the method
          return Cache(name[..ReverseIndexOf(name, '.', brace)], name, element);
      }

      // Cache the member
      return Cache(name[..name.LastIndexOf('.')], name, element);
    }

    private string CacheType(XElement element)
    {
      // Get the member name
      var name = RetrieveName(element)[2..];

      // Prepare the type to cache
      var toAdd = (new DocElement(name, element, this, m_typeResolver), new ConcurrentDictionary<string, IDocMember>());
      // Cache the type
      m_documentation.AddOrUpdate(name, toAdd, (_, y) => Update(y, toAdd));

      return name;
    }

    private static bool TypeGrouper(XElement element)
      => RetrieveName(element).First().Equals('T');

    private static string RetrieveName(XElement element)
      => element.Attributes()
        .First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase))
        .Value;

    private string Cache(string key, string name, XElement element)
    {
      string ProcessName(string memberNameRaw)
      {
        // If the member is not a method..
        if (!name.First().Equals('M'))
          // return the member name
          return memberNameRaw;
        // if the method has no arguments..
        if (!memberNameRaw.EndsWith(')'))
          // and empty braces to the raw name
          memberNameRaw += "()";

        // Return the processed name
        return memberNameRaw;
      }

      // Prepare the member to cache
      var toAdd = new DocMember(ProcessName(name[2..]), ProcessName(name[(1 + key.Length)..]), key[0], element);
      // If the member is cached..
      if (m_documentation.ContainsKey(key[2..]))
        // add the member to the cache
        m_documentation[key[2..]].members?.AddOrUpdate(toAdd.RawName, toAdd, (_, y) => y);
      // Otherwise..
      else
      {
        var dict = new ConcurrentDictionary<string, IDocMember>();
        dict.TryAdd(toAdd.RawName, toAdd);

        // Cache the member
        m_documentation.AddOrUpdate(key[2..], (null, dict), (_, y) => Update(y, (null, dict)));
      }

      return toAdd.DisplayName;
    }

    private static int ReverseIndexOf(string value, char search, int @from)
    {
      // For every character in reverse order..
      for (var i = @from - 1; i >= 0; i--)
        // if the character is matched..
        if (value[i].Equals(search))
          // return its index
          return i;

      // Not found
      return -1;
    }

    private static (TA?, TB?) Update<TA, TB>((TA? a, TB? b) existing, (TA? a, TB? b) toAdd)
      where TA : class where TB : class
      => (existing.a ?? toAdd.a, existing.b ?? toAdd.b);

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
        m_documentation.AddOrUpdate(type.RawName, (resultType, value.members), (_, y) => (doc, y.members));

        // Return the empty type
        resultType = doc;

        m_logger.Warning($"No documentation found for type '{doc.Name}'");
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