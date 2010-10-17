// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangedPathCondition.cs" company="Silverseed.de">
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
  using System.Collections.Generic;
  using System.Text.RegularExpressions;

  internal class ChangedPathCondition : RepositoryInfoHubCondition
  {
    private readonly Regex changedPathRegExPattern;

    private readonly List<RepositoryItemAction> validActions = new List<RepositoryItemAction>();

    public ChangedPathCondition(string path)
    {
      this.changedPathRegExPattern = new Regex(path, RegexOptions.Compiled);
    }

    public ICollection<RepositoryItemAction> Actions
    {
      get
      {
        return this.validActions;
      }

      set
      {
        this.validActions.Clear();
        if (value != null)
        {
          this.validActions.AddRange(value);
        }
      }
    }

    protected override void UpdateState(IRepoChangeInfo repoChangeInfo)
    {      
      foreach (var affectedItem in repoChangeInfo.AffectedItems)
      {
        if (this.ItemMeetsCondition(affectedItem))
        {
          State = true;
          return;
        }
      }

      State = false;
    }

    private bool ItemMeetsCondition(IRepoAffectedItem item)
    {
      return this.CheckAction(item) && this.CheckPath(item);
    }

    private bool CheckAction(IRepoAffectedItem item)
    {
      return (this.validActions.Count == 0) || this.validActions.Contains(item.Action);
    }

    private bool CheckPath(IRepoAffectedItem item)
    {
      return this.changedPathRegExPattern.IsMatch(item.Path);
    }
  }
}
