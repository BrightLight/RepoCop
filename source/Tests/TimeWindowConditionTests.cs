// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeWindowConditionTests.cs" company="Silverseed.de">
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

namespace Silverseed.RepoCop.Tests
{
  using System;
  using Microsoft.Extensions.Time.Testing;
  using NUnit.Framework;

  /// <summary>
  /// Unit tests for the <see cref="TimeWindowCondition"/> class.
  /// </summary>
  [TestFixture(TestOf = typeof(TimeWindowCondition))]
  internal class TimeWindowConditionTests
  {
    [TestCase("2022-01-01 12:00:00", "2022-01-01 18:00:00", "2022-01-01 15:00:00", ExpectedResult = true, Description = "Current time within time window")]
    [TestCase("2022-01-01 12:00:00", "2022-01-01 18:00:00", "2022-01-01 12:00:00", ExpectedResult = true, Description = "Current time is exactly the start of the time window")]
    [TestCase("2022-01-01 12:00:00", "2022-01-01 18:00:00", "2022-01-01 18:00:00", ExpectedResult = true, Description = "Current time is exactly the end of the time window")]
    [TestCase("2022-01-01 08:00:00", "2022-01-02 08:00:00", "2022-01-02 01:00:00", ExpectedResult = true, Description = "Current time within multi-day time window")]
    [TestCase("2022-01-01 12:00:00", "2022-01-01 18:00:00", "2022-01-01 11:59:59", ExpectedResult = false, Description = "Current time is shortly before time window")]
    [TestCase("2022-01-01 12:00:00", "2022-01-01 18:00:00", "2022-01-01 18:00:01", ExpectedResult = false, Description = "Current time is shortly after time window")]
    public bool UpdatesTheStateCorrectly(DateTime startTime, DateTime endTime, DateTime currentTime)
    {
      // Arrange
      var timeProviderMock = new FakeTimeProvider(currentTime);
      var timeWindowCondition = new TestableTimeWindowCondition(timeProviderMock)
      {
        StartTime = startTime,
        EndTime = endTime,
      };

      // Act
      timeWindowCondition.ManuallyUpdateState(null);

      // Assert
      return timeWindowCondition.State;
    }

    /// <summary>
    /// A testable version of the <see cref="TimeWindowCondition"/> class that allows
    /// to manually update the state of the condition.
    /// </summary>
    /// <param name="timeProvider">The time provider to use for getting the current time.</param>
    private class TestableTimeWindowCondition(TimeProvider timeProvider)
      : TimeWindowCondition(timeProvider)
    {
      /// <summary>
      /// Manually updates the state of this condition by checking if the current time is within the time window.
      /// </summary>
      /// <param name="repoChangeInfo">Information about the commit.</param>
      public void ManuallyUpdateState(IRepoChangeInfo repoChangeInfo)
      {
        this.UpdateState(repoChangeInfo);
      }
    }
  }
}
