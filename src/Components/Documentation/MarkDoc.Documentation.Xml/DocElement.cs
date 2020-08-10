using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Threading;
using System.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Members;
using MarkDoc.Helpers;
using MarkDoc.Members.Types;
using static MarkDoc.Documentation.Tags.ITag;

namespace MarkDoc.Documentation.Xml
{
  public class DocElement
    : IDocElement
  {
    #region Fields

    private static readonly HashSet<TagType> SINGLE = new HashSet<TagType>
    {
      TagType.Summary,
      TagType.Value,
      TagType.Remarks,
      TagType.Returns,
      TagType.Example
    };
    private static readonly HashSet<TagType> MULTIPLE = new HashSet<TagType>
    {
      TagType.Exception,
      TagType.Param,
      TagType.Typeparam,
      TagType.Seealso,
    };

    #endregion

    #region Properties

    /// <inheritdoc />
    public IDocumentation Documentation { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public Lazy<IReadOnlyDictionary<string, IDocMember>> Members { get; }

    #endregion

    public DocElement(string name, XElement source, DocResolver docResolver, IResolver typeResolver)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Name = name;
      Documentation = new DocumentationContent(source);
      Members = new Lazy<IReadOnlyDictionary<string, IDocMember>>(() => RetrieveMembers(docResolver, typeResolver), LazyThreadSafetyMode.PublicationOnly);
    }

    internal DocElement(string name, DocResolver docResolver, IResolver typeResolver)
    {
      Name = name;
      Documentation = new DocumentationContent(new Dictionary<TagType, IReadOnlyCollection<ITag>>());
      Members = new Lazy<IReadOnlyDictionary<string, IDocMember>>(() => RetrieveMembers(docResolver, typeResolver), LazyThreadSafetyMode.PublicationOnly);
    }

    #region Methods

    private IReadOnlyDictionary<string, IDocMember> RetrieveMembers(DocResolver docResolver, IResolver typeResolver)
    {
      if (!docResolver.TryFindMembers(Name, out var memberDocs) || memberDocs is null)
        return new Dictionary<string, IDocMember>();

      var result = new Dictionary<string, IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>>>(memberDocs.Count);
      foreach (var item in memberDocs.GroupBy(x => x.Value.Documentation.HasInheritDoc))
      {
        if (item.Key && typeResolver.TryFindType(Name, out var type) && type is IInterface interfaceDef)
        {
          var temps = item.ToDictionary(x => x.Key, x => new Dictionary<TagType, LinkedList<ITag>>());
          ProcessInheritDoc(temps, interfaceDef, temps.Select(x => x.Key).ToArray(), memberDocs, docResolver);

          // Cache collected documentation
          foreach (var (key, value) in temps)
            result.Add(key, value.ToDictionary(x => x.Key, x => x.Value.ToReadOnlyCollection()));
        }
        else
          foreach (var (key, value) in item.Select(Linq.XtoX))
            result.Add(key, value.Documentation.Tags);
      }

      IEnumerable<IDocMember> ProcessCache(IReadOnlyDictionary<string, IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>>> cache)
      {
        foreach (var (key, value) in cache)
        {
          var member = memberDocs[key];

          yield return new DocMember(member.Name, member.Type, new DocumentationContent(value));
        }
      }

      return ProcessCache(result).ToDictionary(x => x.Name);
    }

    private static void ProcessInheritDoc(IReadOnlyDictionary<string, Dictionary<TagType, LinkedList<ITag>>> cache, IInterface type, string[] names, IReadOnlyDictionary<string, IDocMember> memberDocs, IDocResolver docResolver)
    {
      void CacheTags(IEnumerable<string> keys, IReadOnlyDictionary<string, IDocMember> members)
      {
        foreach (var name in keys)
        {
          if (!members.TryGetValue(name, out var member)) continue;

          var cachedTags = cache[name];
          var except = new HashSet<TagType>(cachedTags.Select(x => x.Key).Where(x => SINGLE.Contains(x)));

          bool Filter(KeyValuePair<TagType, IReadOnlyCollection<ITag>> tag)
            => tag.Key != TagType.Inheritdoc && !except.Contains(tag.Key);

          (TagType key, IEnumerable<ITag> tags) Process(KeyValuePair<TagType, IReadOnlyCollection<ITag>> tag)
          {
            if (!MULTIPLE.Contains(tag.Key) || !cachedTags.TryGetValue(tag.Key, out var existing)) return (tag.Key, tag.Value);

            var filter = new HashSet<string>(existing.Select(x => x.Reference));
            var toAdd = tag.Value.Where(x => !filter.Contains(x.Reference));

            return (tag.Key, toAdd);
          }

          var tags = member.Documentation.Tags.Where(Filter).Select(Process);
          foreach (var (key, enumerable) in tags)
          {
            if (cachedTags.ContainsKey(key))
              cachedTags[key].AddRange(enumerable);
            else
              cachedTags.Add(key, enumerable.ToLinkedList());
          }
        }
      }

      CacheTags(names, memberDocs);

      var baseClass = (type is IClass classDef && classDef.BaseClass?.Reference.Value != null)
        ? new[] { classDef.BaseClass.Reference.Value }
        : Enumerable.Empty<IType>();

      var sources = type.InheritedInterfaces
        .Select(x => x.Reference.Value)
        .WhereNotNull()
        .Concat(baseClass)
        .OfType<IInterface>()
        .ToDictionary(x => x.RawName);

      static string ProcessReferences(KeyValuePair<string, IDocMember> input)
      {
        // TODO: Test
        var key = input.Value.Documentation.InheritDocRef[2..];
        if (key.EndsWith(input.Key, StringComparison.InvariantCulture))
          key = key.Remove(key.LastIndexOf('.'));

        return key;
      }


      var withReferencesTable = memberDocs
        .Where(x => x.Value.Documentation.HasInheritDoc && !string.IsNullOrEmpty(x.Value.Documentation.InheritDocRef))
        .ToLookup(ProcessReferences, x => x.Key);
      var withReferences = withReferencesTable
        .SelectMany(Linq.GroupValues)
        .ToReadOnlyCollection();

      foreach (var (key, value) in sources)
      {
        if (!docResolver.TryFindType(value, out var sourceType) || sourceType is null) continue;

        CacheTags(names.Except(withReferences.Except(withReferencesTable[key])), sourceType.Members.Value);
      }
    }

    #endregion
  }
}
