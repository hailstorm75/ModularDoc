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
  /// <summary>
  /// Element documentation
  /// </summary>
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

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Documented element name</param>
    /// <param name="source">Documentation source</param>
    /// <param name="docResolver">Documentation resolver</param>
    /// <param name="typeResolver">Type resolver</param>
    public DocElement(string name, XElement source, DocResolver docResolver, IResolver typeResolver)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Name = name;
      Documentation = new DocumentationContent(source);
      Members = new Lazy<IReadOnlyDictionary<string, IDocMember>>(() => RetrieveMembers(docResolver, typeResolver), LazyThreadSafetyMode.PublicationOnly);
    }

    /// <summary>
    /// Empty constructor
    /// </summary>
    /// <param name="name">Documented element name</param>
    /// <param name="docResolver">Documentation resolver</param>
    /// <param name="typeResolver">Type resolver</param>
    internal DocElement(string name, DocResolver docResolver, IResolver typeResolver)
    {
      Name = name;
      Documentation = new DocumentationContent(new Dictionary<TagType, IReadOnlyCollection<ITag>>());
      Members = new Lazy<IReadOnlyDictionary<string, IDocMember>>(() => RetrieveMembers(docResolver, typeResolver), LazyThreadSafetyMode.PublicationOnly);
    }

    #endregion

    #region Methods

    private IReadOnlyDictionary<string, IDocMember> RetrieveMembers(DocResolver docResolver, IResolver typeResolver)
    {
      // If there is no documentation for this element..
      if (!docResolver.TryFindMembers(Name, out var memberDocs) || memberDocs is null)
        // return empty members
        return new Dictionary<string, IDocMember>();

      // Prepare the main cache
      var result = new Dictionary<string, IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>>>(memberDocs.Count);
      // For every member grouped by whether it has an inheritdoc or not..
      foreach (var items in memberDocs.GroupBy(x => x.Value.Documentation.HasInheritDoc))
      {
        // if the items have inheritdoc and the type is known and the type is not an enum..
        if (items.Key && typeResolver.TryFindType(Name, out var type) && type is IInterface interfaceDef)
        {
          // prepare a temporary cache for the inheritdoc items
          var temps = items.ToDictionary(pair => pair.Value.DisplayName, pair => (pair.Key, new Dictionary<TagType, LinkedList<ITag>>()));
          // resolve the items inheritdoc
          ProcessInheritDoc(temps, interfaceDef, temps.Select(pair => pair.Key).ToArray(), memberDocs, docResolver);

          // for every temporarily cached documentation..
          foreach (var (_, (key, tags)) in temps)
            // move it to the main cache
            result.Add(key, tags.ToDictionary(pair => pair.Key, pair => pair.Value.ToReadOnlyCollection()));
        }
        // otherwise..
        else
          // for every item..
          foreach (var (key, value) in items)
            // cache the item
            result.Add(key, value.Documentation.Tags);
      }

      IEnumerable<IDocMember> ProcessCache(IReadOnlyDictionary<string, IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>>> cache)
      {
        // For every cached member..
        foreach (var (key, value) in cache)
        {
          // select the member
          var member = memberDocs[key];

          // return the prepared member
          yield return new DocMember(member.RawName, member.DisplayName, member.Type, new DocumentationContent(value));
        }
      }

      // Materialize the processed cache to a dictionary
      return ProcessCache(result).ToDictionary(x => x.RawName);
    }

    private static void ProcessInheritDoc(IReadOnlyDictionary<string, (string rawName, Dictionary<TagType, LinkedList<ITag>> tags)> cache, IInterface type, string[] names, IReadOnlyDictionary<string, IDocMember> memberDocs, IDocResolver docResolver)
    {
      void CacheTags(IEnumerable<string> keys, IReadOnlyDictionary<string, IDocMember> members)
      {
        var reCached = members.ToDictionary(x => x.Value.DisplayName, x => x.Value);

        static string ProcessName(string input)
        {
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

          var brace = input.IndexOf('(', StringComparison.InvariantCultureIgnoreCase);
          var max = brace == -1
            ? input.Length - 1
            : brace;

          var dot = ReverseIndexOf(input, '.', max);
          if (dot == -1 || dot >= max)
            return input;
          var res = input.Substring(dot + 1);
          return res;
        }

        // For every name..
        foreach (var name in keys.Select(ProcessName))
        {
          // if there is no matching member by name..
          if (!reCached.TryGetValue(name, out var member))
            // continue to the next member name
            continue;

          // select the previously cached tags
          var cachedTags = cache[name].tags;
          // select the previously cached tags of the same tag type
          var except = new HashSet<TagType>(cachedTags.Select(x => x.Key).Where(x => SINGLE.Contains(x)));

          bool Filter(KeyValuePair<TagType, IReadOnlyCollection<ITag>> tag)
            => tag.Key != TagType.Inheritdoc && !except.Contains(tag.Key);

          (TagType key, IEnumerable<ITag> tags) Process(KeyValuePair<TagType, IReadOnlyCollection<ITag>> tag)
          {
            // If the tag type cannot have multiple occurrences or isn't cached yet..
            if (!MULTIPLE.Contains(tag.Key) || !cachedTags.TryGetValue(tag.Key, out var existing))
              // return new tags to cache
              return (tag.Key, tag.Value);

            // Create a filter based on existing tag references
            var filter = new HashSet<string>(existing!.Select(x => x.Reference));
            // Create a list of tags to add which contain unique references
            var toAdd = tag.Value.Where(x => !filter.Contains(x.Reference));

            // Return new tags to cache
            return (tag.Key, toAdd);
          }

          // Prepare the sequence of tags to be cached
          var tags = member!.Documentation.Tags
            // Filter out unwanted tags
            .Where(Filter)
            // Select the tags to cache
            .Select(Process);

          // For every tag..
          foreach (var (key, enumerable) in tags)
            // if its type is already cached..
            if (cachedTags.ContainsKey(key))
              // append it to the existing cached collection
              cachedTags[key].AddRange(enumerable);
            // otherwise..
            else
              // cache it
              cachedTags.Add(key, enumerable.ToLinkedList());
        }
      }

      CacheTags(names, memberDocs);

      static string ProcessReferences(KeyValuePair<string, IDocMember> input)
      {
        // TODO: Test
        var key = input.Value.Documentation.InheritDocRef[2..];
        return key.EndsWith(input.Key, StringComparison.InvariantCulture)
          ? key.Remove(key.LastIndexOf('.'))
          : key;
      }

      var withReferencesTable = memberDocs
        // Filter out members without inheritdoc
        .Where(x => x.Value.Documentation.HasInheritDoc && !string.IsNullOrEmpty(x.Value.Documentation.InheritDocRef))
        .ToLookup(ProcessReferences, x => x.Key);
      var withReferences = withReferencesTable
        // Flatten the sequence
        .SelectMany(Linq.GroupValues)
        // Materialize sequence to a collection
        .ToReadOnlyCollection();

      // If a base class exists..
      var baseClass = type is IClass classDef && classDef.BaseClass?.Reference.Value != null
        // return it
        ? new[] { classDef.BaseClass.Reference.Value }
        // otherwise return an empty
        : Enumerable.Empty<IType>();

      var sources = type.InheritedInterfaces
        // Select type references
        .Select(x => x.Reference.Value)
        // Exclude types with no references
        .WhereNotNull()
        // Append the base class to the sequence
        .Concat(baseClass)
        // Exclude Enums
        .OfType<IInterface>()
        // Materialize the sequence to a dictionary
        .ToDictionary(x => x.RawName);

      // For every source of inheritance..
      foreach (var (key, value) in sources)
      {
        // if the inherited type is unknown..
        if (!docResolver.TryFindType(value, out var sourceType) || sourceType is null)
          // continue to the next source
          continue;

        // Otherwise cache the source members
        CacheTags(names.Except(withReferences.Except(withReferencesTable[key])), sourceType.Members.Value);
      }
    }

    #endregion
  }
}
