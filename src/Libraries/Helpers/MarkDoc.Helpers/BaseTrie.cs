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
}