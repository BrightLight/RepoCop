// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorConditionTests.cs" company="Silverseed.de">
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
  using System.Collections.Generic;
  using NUnit.Framework;
  using Rhino.Mocks;

  /// <summary>
  /// Unit tests for the <see cref="AuthorCondition"/> class.
  /// </summary>
  [TestFixture]
  public class AuthorConditionTests
  {
    [TestCase("AB;CD;EF", "CD", true, Result = true)]
    [TestCase("AB;CD;EF", "cd", true, Result = false)]
    [TestCase("AB;CD;EF", "CD", false, Result = true)]
    [TestCase("AB;CD;EF", "cd", false, Result = true)]
    public bool FindAuthorRespectsCasingSetting(string authorsSeparatedBySemicolon, string compareToAuthor, bool caseSensitive)
    {
      var authors = authorsSeparatedBySemicolon.Split(';');
      var authorCondition = new TestableAuthorCondition(authors);
      authorCondition.CaseSensitive = caseSensitive;

      var repoChangeInfo = MockRepository.GenerateStub<IRepoChangeInfo>();
      repoChangeInfo.Stub(x => x.Author).Return(compareToAuthor);
      authorCondition.ManuallyUpdateState(repoChangeInfo);

      return authorCondition.State;
    }

    private class TestableAuthorCondition : AuthorCondition
    {
      public void ManuallyUpdateState(IRepoChangeInfo repoChangeInfo)
      {
        this.UpdateState(repoChangeInfo);
      }

      public TestableAuthorCondition(IEnumerable<string> authors)
        : base(authors)
      {
      }
    }
  }
}
