// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubversionInfoHubCondition.cs" company="Silverseed.de">
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
  using Silverseed.ComponentModel;
  
  internal abstract class SubversionInfoHubCondition : Condition
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SubversionInfoHubCondition"/> class.
    /// </summary>
    protected SubversionInfoHubCondition()
      : base(false)
    {
      SubversionInfoHub.Instance.PropertyChanged += this.SubversionInfoHub_PropertyChanged;
    }

    protected abstract void UpdateState(IRepoChangeInfo repoChangeInfo);

    /// <summary>
    /// Handles the PropertyChanged event of the <see cref="SubversionInfoHub"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
    private void SubversionInfoHub_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      this.UpdateState(SubversionInfoHub.Instance.RepoChangeInfo);
    }
  }
}
