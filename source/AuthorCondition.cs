// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorCondition.cs" company="Silverseed.de">
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
  using System.Linq;

  /// <summary>
  /// A condition that checks if the author of a change matches a given author(s).
  /// </summary>
  internal class AuthorCondition : RepositoryInfoHubCondition
  {
    private readonly List<string> authors = new List<string>();

    public AuthorCondition(string author)
    {
      this.authors.Add(author);
    }

    public AuthorCondition(IEnumerable<string> authors)
    {
      this.authors.AddRange(authors);
    }

    protected override void UpdateState(IRepoChangeInfo repoChangeInfo)
    {
      var stringComparison = this.DetermineStringComparison;
      this.State = this.authors.Exists( x => string.Equals(repoChangeInfo.Author, x, stringComparison));
    }

    public ICollection<string> Authors
    {
      get
      {
        return this.authors.AsReadOnly();
      }

      set
      {
        this.authors.Clear();
        this.authors.AddRange(value);
      }
    }

    public bool CaseSensitive { get; set; }

    private StringComparison DetermineStringComparison => this.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="AuthorCondition"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="AuthorCondition"/>.</returns>
    public override string ToString()
    {
      return string.Format($"{nameof(AuthorCondition)} ({this.authors.Count} author(s): {string.Join(";", this.authors)}; {nameof(CaseSensitive)}: {this.CaseSensitive})");
    }
  }
}
