using System;
using System.Collections.Generic;

namespace MarkDoc.Helpers
{
  public abstract class BaseTrie<T>
    where T : notnull
  {
    protected readonly TrieNode<T> m_root;

    protected BaseTrie()
      => m_root = new TrieNode<T>();

    /// <summary>
    /// Trie root node
    /// </summary>
    public TrieNode<T> Root => m_root;

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

  public class TrieNode<T> where T : notnull
  {
    private readonly Dictionary<T, TrieNode<T>> m_roots;

    public IReadOnlyDictionary<T, TrieNode<T>> Nodes
      => m_roots;

    public TrieNode()
      => m_roots = new Dictionary<T, TrieNode<T>>();

    public TrieNode<T> Add(T item)
    {
      if (m_roots.ContainsKey(item))
        return m_roots[item];

      var node = new TrieNode<T>();
      m_roots.Add(item, node);

      return node;
    }
  }
}