// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CapabilityCondition.cs" company="Silverseed.de">
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
  using System.Linq;

  /// <summary>
  /// A condition that is met if the client capabilities contain a specific capability.
  /// </summary>
  internal class CapabilityCondition : RepositoryInfoHubCondition
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CapabilityCondition"/> class.
    /// </summary>
    public CapabilityCondition()
    {
    }

    /// <summary>
    /// Updates the state of this condition by checking if the client capabilities contain
    /// the capability specified in the constructor.
    /// </summary>
    /// <param name="repoChangeInfo">Information about the commit.</param>
    protected override void UpdateState(IRepoChangeInfo repoChangeInfo)
    {
      State = repoChangeInfo.ClientCapabilities.Contains(this.Capability, StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Gets the capability that must be present in the client capabilities.
    /// </summary>
    public string Capability { get; init; }
  }
}
