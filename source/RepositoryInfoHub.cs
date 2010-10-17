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

  public class RepositoryInfoHub : INotifyPropertyChanged
  {
    private static RepositoryInfoHub instance;

    private IRepoChangeInfo repoChangeInfo;

    private readonly Dictionary<string, Func<string>> tokenDictionary = new Dictionary<string, Func<string>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryInfoHub"/> class.
    /// </summary>
    public RepositoryInfoHub()
    {
      this.tokenDictionary.Add("#author#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Author : String.Empty);
      this.tokenDictionary.Add("#logmessage#", () => this.repoChangeInfo != null ? this.repoChangeInfo.LogMessage : String.Empty);
      this.tokenDictionary.Add("#revision#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Revision.ToString(CultureInfo.InvariantCulture) : String.Empty);
      this.tokenDictionary.Add("#time#", () => this.repoChangeInfo != null ? this.repoChangeInfo.Time.ToString(CultureInfo.CurrentCulture) : String.Empty);
      this.tokenDictionary.Add("#affectedfiles#", () => this.GetAffectedPaths(RepositoryItemNodeKind.File, Environment.NewLine));
      this.tokenDictionary.Add("#affectedpaths#", () => this.GetAffectedPaths(RepositoryItemNodeKind.Unknown, Environment.NewLine));
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

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

    public string ParseTokens(string rawText)
    {
      string newText = rawText;
      foreach (var token in this.tokenDictionary)
      {
        newText = newText.Replace(token.Key, token.Value());
      }

      return newText;
    }

    private void OnNotifyPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    private string GetAffectedPaths(RepositoryItemNodeKind nodeKind, string separator)
    {
      if (this.repoChangeInfo == null)
      {
        return String.Empty;
      }

      var stringBuilder = new StringBuilder();
      foreach (var item in this.repoChangeInfo.AffectedItems)
      {
        if ((nodeKind == RepositoryItemNodeKind.Unknown) 
          || (item.NodeKind == nodeKind))
        {
          if (stringBuilder.Length > 0)
          {
            stringBuilder.Append(separator);
          }

          stringBuilder.Append(item.Path);
        }
      }

      return stringBuilder.ToString();
    }
  }
}
