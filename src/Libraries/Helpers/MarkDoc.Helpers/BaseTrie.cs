using System;
using System.Collections.Generic;

namespace MarkDoc.Helpers
{
  public abstract class BaseTrie<T>
    where T : notnull
  {
    protected readonly TrieNode m_root;

    protected BaseTrie()
      => m_root = new TrieNode();

    /// <summary>
    /// Trie root node
    /// </summary>
    public TrieNode Root => m_root;

    public class TrieNode
    {
      private readonly Dictionary<T, TrieNode> m_roots;

      public IReadOnlyDictionary<T, TrieNode> Nodes
        => m_roots;

      public TrieNode()
        => m_roots = new Dictionary<T, TrieNode>();

      public TrieNode Add(T item)
      {
        if (m_roots.ContainsKey(item))
          return m_roots[item];

        var node = new TrieNode();
        m_roots.Add(item, node);

        return node;
      }
    }

    public void AddRange(IEnumerable<T> items)
    {
      if (items is null)
        throw new ArgumentNullException(nameof(items));

      foreach (var item in items)
        AddItem(item);
    }

    protected abstract IEnumerable<T> Split(T item);

    protected void AddItem(T item)
    {
      var split = Split(item);

      var root = m_root;
      foreach (var ns in split)
        root = root.Add(ns);
    }
  }
}