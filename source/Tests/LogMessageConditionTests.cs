// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMessageConditionTests.cs" company="Silverseed.de">
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
  using System.Text.RegularExpressions;
  using Moq;
  using NUnit.Framework;

  /// <summary>
  /// Unit tests for the <see cref="LogMessageCondition"/> class.
  /// </summary>
  [TestFixture]
  public class LogMessageConditionTests
  {
    /// <summary>
    /// Checks if a log message matches the condition's regular expression.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="logMessage">The log message to test.</param>
    /// <param name="options">The regular expression options.</param>
    /// <returns><c>true</c> if the log message matches the pattern; otherwise, <c>false</c>.</returns>
    [TestCase(".*", "Any message", RegexOptions.None, ExpectedResult = true)]
    [TestCase("fix", "This is a fix", RegexOptions.None, ExpectedResult = true)]
    [TestCase("FIX", "This is a fix", RegexOptions.None, ExpectedResult = false)]
    [TestCase("FIX", "This is a fix", RegexOptions.IgnoreCase, ExpectedResult = true)]
    [TestCase("^Start", "Start of message", RegexOptions.None, ExpectedResult = true)]
    [TestCase("^Start", "The Start of message", RegexOptions.None, ExpectedResult = false)]
    public bool LogMessageMatches(string pattern, string logMessage, RegexOptions options)
    {
      var logMessageCondition = new LogMessageCondition(pattern, options);

      var repoChangeInfo = new Mock<IRepoChangeInfo>();
      repoChangeInfo.SetupGet(x => x.LogMessage).Returns(logMessage);
      repoChangeInfo.SetupGet(x => x.ClientCapabilities).Returns(new string[0]);
      repoChangeInfo.SetupGet(x => x.AffectedItems).Returns(new IRepoAffectedItem[0]);
      repoChangeInfo.SetupGet(x => x.HookType).Returns(HookType.Undefined);
      repoChangeInfo.SetupGet(x => x.Author).Returns(string.Empty);
      repoChangeInfo.SetupGet(x => x.Time).Returns(DateTime.Now);
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo.Object;

      return logMessageCondition.State;
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
