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

namespace Silverseed.SubversionHook
{
  using System.Collections.Generic;

  internal class AuthorCondition : SubversionInfoHubCondition
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
      State = this.authors.Contains(repoChangeInfo.Author);
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
  }
}
