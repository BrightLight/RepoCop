// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangedPathConditionTests.cs" company="Silverseed.de">
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
  /// Unit tests for the <see cref="ChangedPathCondition"/> class.
  /// </summary>
  [TestFixture]
  public class ChangedPathConditionTests
  {
    /// <summary>
    /// Checks if a changed path matches the condition's regular expression and action.
    /// </summary>
    /// <param name="pattern">The regular expression pattern for the path.</param>
    /// <param name="action">The repository action.</param>
    /// <returns><c>true</c> if the path matches; otherwise, <c>false</c>.</returns>
    [TestCase(".*", RepositoryItemAction.Add, ExpectedResult = true)]
    [TestCase("source/.*", RepositoryItemAction.Modify, ExpectedResult = true)]
    [TestCase("source/.*", RepositoryItemAction.Delete, ExpectedResult = true)]
    [TestCase("docs/.*", RepositoryItemAction.Modify, ExpectedResult = false)]
    public bool PathMatches(string pattern, RepositoryItemAction action)
    {
      var condition = new ChangedPathCondition(pattern);
      var affectedItem = new Mock<IRepoAffectedItem>();
      affectedItem.SetupGet(x => x.Path).Returns("source/File.cs");
      affectedItem.SetupGet(x => x.Action).Returns(action);

      var repoChangeInfo = new Mock<IRepoChangeInfo>();
      repoChangeInfo.SetupGet(x => x.AffectedItems).Returns(new[] { affectedItem.Object });
      repoChangeInfo.SetupGet(x => x.ClientCapabilities).Returns(new string[0]);
      repoChangeInfo.SetupGet(x => x.LogMessage).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.HookType).Returns(HookType.Undefined);
      repoChangeInfo.SetupGet(x => x.Author).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.Time).Returns(DateTime.Now);
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo.Object;
      return condition.State;
    }

    /// <summary>
    /// Checks if the condition correctly filters by repository actions.
    /// </summary>
    [Test]
    public void ActionRestrictionMatches()
    {
      var condition = new ChangedPathCondition(".*");
      condition.Actions = new[] { RepositoryItemAction.Add };
      
      var affectedItem = new Mock<IRepoAffectedItem>();
      affectedItem.SetupGet(x => x.Path).Returns("any/path");
      affectedItem.SetupGet(x => x.Action).Returns(RepositoryItemAction.Modify);

      var repoChangeInfo = new Mock<IRepoChangeInfo>();
      repoChangeInfo.SetupGet(x => x.AffectedItems).Returns(new[] { affectedItem.Object });
      repoChangeInfo.SetupGet(x => x.ClientCapabilities).Returns(new string[0]);
      repoChangeInfo.SetupGet(x => x.LogMessage).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.HookType).Returns(HookType.Undefined);
      repoChangeInfo.SetupGet(x => x.Author).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.Time).Returns(DateTime.Now);

      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo.Object;
      Assert.That(condition.State, Is.False);

      RepositoryInfoHub.Flush();
      var condition2 = new ChangedPathCondition(".*");
      condition2.Actions = new[] { RepositoryItemAction.Add };

      affectedItem.SetupGet(x => x.Action).Returns(RepositoryItemAction.Add);
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo.Object;
      Assert.That(condition2.State, Is.True);
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
