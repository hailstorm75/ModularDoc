using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Helpers
{
  public class TrieNamespace
  {
    private readonly TrieNode m_root;

    private class TrieNode
    {
      private readonly Dictionary<string, TrieNode> m_roots;

      public IReadOnlyDictionary<string, TrieNode> Nodes
        => m_roots;

      public TrieNode()
        => m_roots = new Dictionary<string, TrieNode>();

      public TrieNode Add(string item)
      {
        if (m_roots.ContainsKey(item))
          return m_roots[item];

        var node = new TrieNode();
        m_roots.Add(item, node);

        return node;
      }
    }

    public TrieNamespace()
      => m_root = new TrieNode();

    public bool TryFindKnownNamespace(string item, out string ns)
    {
      var result = new LinkedList<string>();

      var root = m_root;
      var namespaces = RetrieveNamespaces(item);
      foreach (var @namespace in namespaces)
      {
        if (!root.Nodes.ContainsKey(@namespace))
          break;

        result.AddLast(@namespace);
        root = root.Nodes[@namespace];
      }

      ns = string.Join('.', result);

      return result.Any();
    }

    public TrieNamespace AddRange(IEnumerable<string> items)
    {
      if (items is null)
        throw new ArgumentNullException(nameof(items));

      foreach (var item in items)
        AddItem(item);

      return this;
    }

    public TrieNamespace Add(string item)
    {
      AddItem(item);
      return this;
    }

    private void AddItem(string item)
    {
      var namespaces = RetrieveNamespaces(item);

      var root = m_root;
      foreach (var ns in namespaces ?? Enumerable.Empty<string>())
        root = root.Add(ns);
    }

    private static IEnumerable<string> RetrieveNamespaces(string item)
    {
      if (item is null)
        throw new ArgumentNullException(nameof(item));
      if (string.IsNullOrEmpty(item))
        throw new ArgumentException(); // TODO: Item empty

      var result = item.Split('.', StringSplitOptions.RemoveEmptyEntries);
      if (result.Length == 0)
        throw new ArgumentException(); // TODO: No namespaces

      return result;
    }
  }
}
