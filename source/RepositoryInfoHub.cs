// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryInfoHub.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Globalization;
  using System.Text;

  /// <summary>
  /// A hub that stores the information about changes to the repository
  /// and notifies registered instances about these changes.
  /// </summary>
  public class RepositoryInfoHub : INotifyPropertyChanged
  {
    /// <summary>
    /// The replacement tokens supported by this <see cref="RepositoryInfoHub"/>.
    /// </summary>
    private readonly Dictionary<string, Func<string>> tokenDictionary = new Dictionary<string, Func<string>>();

    /// <summary>
    /// The singleton instance.
    /// </summary>
    private static RepositoryInfoHub instance;

    /// <summary>
    /// The latest change to the repository.
    /// </summary>
    private IRepoChangeInfo repoChangeInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryInfoHub"/> class.
    /// </summary>
    public RepositoryInfoHub()
    {
      this.tokenDictionary.Add("#author#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Author : string.Empty);
      this.tokenDictionary.Add("#logmessage#", () => this.repoChangeInfo != null ? this.repoChangeInfo.LogMessage : string.Empty);
      this.tokenDictionary.Add("#prevrevision#", () => this.repoChangeInfo != null ? (this.repoChangeInfo.Revision - 1).ToString(CultureInfo.InvariantCulture) : string.Empty);
      this.tokenDictionary.Add("#revision#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Revision.ToString(CultureInfo.InvariantCulture) : string.Empty);
      this.tokenDictionary.Add("#nextrevision#", () => this.repoChangeInfo != null ? (this.repoChangeInfo.Revision + 1).ToString(CultureInfo.InvariantCulture) : string.Empty);
      this.tokenDictionary.Add("#time#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Time.ToString(CultureInfo.CurrentCulture) : string.Empty);
      this.tokenDictionary.Add("#affectedfiles#", () => this.GetAffectedPaths(RepositoryItemNodeKind.File, Environment.NewLine));
      this.tokenDictionary.Add("#affectedfilesdetailed#", () => this.GetAffectedPaths(RepositoryItemNodeKind.File, Environment.NewLine, true));
      this.tokenDictionary.Add("#affectedpaths#", () => this.GetAffectedPaths(RepositoryItemNodeKind.Unknown, Environment.NewLine));
      this.tokenDictionary.Add("#affectedpathsdetailed#", () => this.GetAffectedPaths(RepositoryItemNodeKind.Unknown, Environment.NewLine, true));
    }

    #region INotifyPropertyChanged Members

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    /// <summary>
    /// Gets the singleton instance of <see cref="RepositoryInfoHub"/>.
    /// </summary>
    public static RepositoryInfoHub Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new RepositoryInfoHub();
        }

        return instance;
      }
    }

    /// <summary>
    /// Gets or sets the latest change (commit) to the repository.
    /// </summary>
    public IRepoChangeInfo RepoChangeInfo
    {
      get
      {
        return this.repoChangeInfo;
      }

      set
      {
        if (this.repoChangeInfo != value)
        {
          this.repoChangeInfo = value;
          this.OnNotifyPropertyChanged("RepoChangeInfo");
        }
      }
    }

    /// <summary>
    /// Replaces all known tokens in the specified <paramref name="rawText"/> with their
    /// current value and returns this "expanded" text.
    /// </summary>
    /// <param name="rawText">A text possibly containing replacement tokens that should now be replaced with their actual values.</param>
    /// <returns>The specified <paramref name="rawText"/> with all known replacement tokens replaced with their actual values.</returns>
    public string ParseTokens(string rawText)
    {
      var newText = rawText;
      foreach (var token in this.tokenDictionary)
      {
        if (newText.Contains(token.Key))
        {
          newText = newText.Replace(token.Key, token.Value());
        }
      }

      return newText;
    }

    /// <summary>
    /// Registers a <paramref name="valueFunc"/> for the specified replacement <paramref name="token"/>.
    /// </summary>
    /// <param name="token">The name of the replacement token that is registered.</param>
    /// <param name="valueFunc">A function that returns the replacement value for the specified <paramref name="token"/>.</param>
    public void AddToken(string token, Func<string> valueFunc)
    {
      this.tokenDictionary.Add(token, valueFunc);
    }

    internal static void Flush()
    {
      instance = null;
    }

    /// <summary>
    /// Called when a property of this instance has changed.
    /// </summary>
    /// <param name="propertyName">The name of the property that was changed.</param>
    private void OnNotifyPropertyChanged(string propertyName)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string GetAffectedPaths(RepositoryItemNodeKind nodeKind, string separator, bool detailed = false)
    {
      if (this.repoChangeInfo == null)
      {
        return string.Empty;
      }

      var stringBuilder = new StringBuilder();
      foreach (var item in this.repoChangeInfo.AffectedItems)
      {
        // files that are not related to a repository action were parsed from merge information and contain the original file that wasn't actually changed by the commit
        if (item.Action == RepositoryItemAction.None)
        {
          continue;
        }

        if ((nodeKind == RepositoryItemNodeKind.Unknown)
          || (item.NodeKind == nodeKind))
        {
          if (stringBuilder.Length > 0)
          {
            stringBuilder.Append(separator);
          }


          if (detailed)
          {
            stringBuilder.Append($"{item.Path}|{item.Action}|{item.NodeKind}");
          }
          else
          {
            stringBuilder.Append(item.Path);
          }
        }
      }

      return stringBuilder.ToString();
    }
  }
}
