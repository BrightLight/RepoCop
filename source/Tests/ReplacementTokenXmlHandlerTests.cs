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
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Moq;
  using NUnit.Framework;
  using Silverseed.RepoCop.Xml;

  [TestFixture]
  public class ReplacementTokenXmlHandlerTests
  {
    [Test]
    public void SimpleValueReplacement()
    {
      RepositoryInfoHub.Flush();
      var replacementTokenXmlHandler = new ReplacementTokenXmlHandler();

      var attributes = new Dictionary<string, string>();
      const string constantTokenValue = "constant text";
      attributes.Add("TokenName", "Foo");
      attributes.Add("Value", constantTokenValue);
      replacementTokenXmlHandler.ProcessStartElement("ReplacementToken", attributes);
      Assert.That(RepositoryInfoHub.Instance.ParseTokens("#Foo#"), Is.EqualTo(constantTokenValue));
    }

    [Test]
    public void RegExExtractValueWithExistingTokenAsValue()
    {
      RepositoryInfoHub.Flush();
      var replacementTokenXmlHandler = new ReplacementTokenXmlHandler();

      var attributes = new Dictionary<string, string>();
      attributes.Add("TokenName", "Foo");
      attributes.Add("Value", "#logmessage#");
      attributes.Add("RegExPattern", @"\[Review #(\d+)\]");
      replacementTokenXmlHandler.ProcessStartElement("ReplacementToken", attributes);
      var repoChangeInfo = Mock.Of<IRepoChangeInfo>(x =>
        x.LogMessage == "This commit updates [Review #123] by fixing stuff"
        && x.AffectedItems == new List<IRepoAffectedItem>());
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo;
      Assert.That(RepositoryInfoHub.Instance.ParseTokens("#Foo#"), Is.EqualTo("123"));
    }

    [Test]
    public void RegExReplacementWithExistingTokenAsValue()
    {
      RepositoryInfoHub.Flush();
      var replacementTokenXmlHandler = new ReplacementTokenXmlHandler();

      const string Separator = ":";
      var attributes = new Dictionary<string, string>();
      attributes.Add("TokenName", "affectedpathswithseparator");
      attributes.Add("Value", "#affectedpaths#");
      attributes.Add("RegExPattern", Environment.NewLine);
      attributes.Add("Replacement", Separator);
      replacementTokenXmlHandler.ProcessStartElement("ReplacementToken", attributes);
      var affectedItems = new List<IRepoAffectedItem>();
      var affectedItem1 = Mock.Of<IRepoAffectedItem>(x =>
        x.Action == RepositoryItemAction.Add
        && x.Path == @"c:\foo\addedfile.txt"
        && x.NodeKind == RepositoryItemNodeKind.File);
      var affectedItem2 = Mock.Of<IRepoAffectedItem>(x =>
        x.Action == RepositoryItemAction.Add
        && x.Path == @"c:\foo\bar"
        && x.NodeKind == RepositoryItemNodeKind.Directory);
      var affectedItem3 = Mock.Of<IRepoAffectedItem>(x =>
        x.Action == RepositoryItemAction.Add
        && x.Path == @"c:\foo\bar\demo.txt"
        && x.NodeKind == RepositoryItemNodeKind.File);
      affectedItems.Add(affectedItem1);
      affectedItems.Add(affectedItem2);
      affectedItems.Add(affectedItem3);

      var expectedTokenValue = affectedItem1.Path + Separator + affectedItem2.Path + Separator + affectedItem3.Path;

      var repoChangeInfo = Mock.Of<IRepoChangeInfo>(x => 
        x.LogMessage == "This commit updates [Review #123] by fixing stuff"
        && x.AffectedItems == affectedItems);
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo;
      Assert.That(RepositoryInfoHub.Instance.ParseTokens("#affectedpathswithseparator#"), Is.EqualTo(expectedTokenValue));
    }
  }
}
