using System.Collections.Generic;

namespace ModularDoc.Helpers
{
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