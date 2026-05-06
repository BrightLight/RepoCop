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

  /// <summary>
  /// A condition that checks if any of the changed paths in a repository change match a given regular expression.
  /// </summary>
  internal class ChangedPathCondition : RepositoryInfoHubCondition
  {
    private readonly Regex changedPathRegExPattern;

    private readonly List<RepositoryItemAction> validActions = new List<RepositoryItemAction>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangedPathCondition"/> class.
    /// </summary>
    /// <param name="path">The regular expression pattern to match against the changed paths.</param>
    public ChangedPathCondition(string path)
    {
      this.changedPathRegExPattern = new Regex(path, RegexOptions.Compiled);      
    }

    /// <summary>
    /// Gets or sets the repository actions that are valid for this condition.
    /// </summary>
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

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ChangedPathCondition"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current condition.</returns>
    public override string ToString()
    {
      return string.Format($"{nameof(ChangedPathCondition)} ({this.validActions.Count} action(s): {string.Join(";", this.validActions)}; PathRegEx: [{this.changedPathRegExPattern}])");
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
      return (this.validActions.Count == 0 && item.Action != RepositoryItemAction.None) || this.validActions.Contains(item.Action);
    }

    private bool CheckPath(IRepoAffectedItem item)
    {
      return this.changedPathRegExPattern.IsMatch(item.Path);
    }
  }
}
