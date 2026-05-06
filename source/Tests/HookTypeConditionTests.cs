// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookTypeConditionTests.cs" company="Silverseed.de">
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
  using Moq;
  using NUnit.Framework;

  /// <summary>
  /// Unit tests for the <see cref="HookTypeCondition"/> class.
  /// </summary>
  [TestFixture]
  public class HookTypeConditionTests
  {
    /// <summary>
    /// Checks if a change in the repository hook type matches this condition.
    /// </summary>
    /// <param name="conditionHookType">The hook type specified in the condition.</param>
    /// <param name="repoHookType">The actual hook type from the repository change.</param>
    /// <returns><c>true</c> if the hook types match; otherwise, <c>false</c>.</returns>
    [TestCase(HookType.Undefined, HookType.PreCommit, ExpectedResult = true)]
    [TestCase(HookType.PreCommit, HookType.PreCommit, ExpectedResult = true)]
    [TestCase(HookType.PostCommit, HookType.PreCommit, ExpectedResult = false)]
    [TestCase(HookType.PostCommit, HookType.PostCommit, ExpectedResult = true)]
    public bool HookTypeMatches(HookType conditionHookType, HookType repoHookType)
    {
      var hookTypeCondition = new HookTypeCondition();
      hookTypeCondition.HookType = conditionHookType;

      var repoChangeInfo = new Mock<IRepoChangeInfo>();
      repoChangeInfo.SetupGet(x => x.HookType).Returns(repoHookType);
      repoChangeInfo.SetupGet(x => x.ClientCapabilities).Returns(new string[0]);
      repoChangeInfo.SetupGet(x => x.AffectedItems).Returns(new IRepoAffectedItem[0]);
      repoChangeInfo.SetupGet(x => x.LogMessage).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.Author).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.Time).Returns(DateTime.Now);
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo.Object;

      return hookTypeCondition.State;
    }

    /// <summary>
    /// Cleans up after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
      RepositoryInfoHub.Flush();
    }
  }
}
