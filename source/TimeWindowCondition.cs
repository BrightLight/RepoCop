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

  /// <summary>
  /// A condition that checks if the current time is within a given time window.
  /// </summary>
  internal class TimeWindowCondition : RepositoryInfoHubCondition
  {
    private readonly TimeProvider timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeWindowCondition"/> class.
    /// </summary>
    /// <param name="timeProvider">The time provider to use for getting the current time.</param>
    public TimeWindowCondition(TimeProvider timeProvider = null)
    {
      this.timeProvider = timeProvider ?? TimeProvider.System;
    }

    /// <summary>
    /// Gets the start time of the time window.
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Gets the end time of the time window.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Updates the state of this condition by checking if the current time is within the time window.
    /// </summary>
    /// <param name="repoChangeInfo">Information about the commit.</param>
    protected override void UpdateState(IRepoChangeInfo repoChangeInfo)
    {
      var currentTime = this.timeProvider.GetLocalNow();
      this.State = currentTime >= this.StartTime && currentTime <= this.EndTime;
    }

    /// <inheritdoc />
    override public string ToString()
    {
      return $"TimeWindowCondition: {this.StartTime} - {this.EndTime}";
    }
  }
}
