﻿using System;
using System.Collections.Generic;
using System.IO;

namespace MarkDoc.Documentation.Xml
{
  public struct DocSettings
    : IDocSettings
  {
    private IReadOnlyCollection<string> m_paths;

    #region Properties

    /// <inheritdoc />
    public readonly Guid Id { get; }

    /// <inheritdoc />
    public readonly string Name { get; }

    /// <inheritdoc />
    public readonly string Description { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Paths
    {
      readonly get => m_paths;
      set
      {
        foreach (var path in value)
          ThrowIfInvalidPath(path);

        m_paths = value;
      }
    }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public DocSettings(IEnumerable<string> paths)
    {
      var pathsList = new LinkedList<string>();

      m_paths = pathsList;
      Id = new Guid("0D5688E6-AF55-4F06-9786-69C04C5D7674");
      Name = "XML Documentation parser";
      Description = "Parses .NET code documentation from XML files";

      foreach (var path in paths)
      {
        ThrowIfInvalidPath(path);
        pathsList.AddLast(path);
      }
    }

    private static void ThrowIfInvalidPath(string? path)
    {
      if (string.IsNullOrEmpty(path?.Trim()))
        throw new ArgumentNullException(nameof(path));

      var extension = Path.GetExtension(path);
      if (string.IsNullOrEmpty(extension))
        throw new ArgumentException("Provided path '{path}' is invalid");

      if (extension.Equals(".xml", StringComparison.InvariantCultureIgnoreCase))
        throw new ArgumentException($"Provided path '{path}' is of an invalid type");

      var info = new FileInfo(path);
      if (!info.Exists)
        throw new FileNotFoundException($"Provided path '{path}' points to a file that does not exist");
    }
  }
}