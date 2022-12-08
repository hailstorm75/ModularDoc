using System;
using System.Collections.Generic;
using System.Linq;
using ModularDoc.Helpers.Properties;

namespace ModularDoc.Helpers
{
  public class TrieNamespace
    : BaseTrie<string>
  {
    public TrieNamespace Add(string item)
    {
      AddItem(item);
      return this;
    }

    public bool TryFindKnownNamespace(string item, out string ns)
    {
      var result = new LinkedList<string>();

      var root = m_root;
      var namespaces = Split(item);
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

    protected override IEnumerable<string> Split(string item)
    {
      if (item is null)
        throw new ArgumentNullException(nameof(item));
      if (item.Length == 0)
        throw new ArgumentException(Resources.emptyNamespace);

      var result = item.Split('.', StringSplitOptions.RemoveEmptyEntries);
      if (result.Length == 0)
        throw new ArgumentException(Resources.noNamespacePresent);

      return result;
    }
  }
}
