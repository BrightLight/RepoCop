// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SvnLookTests.cs" company="Soloplan GmbH">
// //   Copyright (c) Soloplan GmbH. All rights reserved.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Tests
{
  using System;
  using System.Linq;
  using NUnit.Framework;
  using Silverseed.RepoCop.Subversion;

  [TestFixture]
  public class SvnLookTests
  {
    [Test]
    public void TestDateTimeOffsetParsing()
    {
      var offsetString = SvnLook.ParseDateTimeOffsetString("2018-08-01 19:21:10 +0200 (Mi, 01 Aug 2018)");
      Assert.True(DateTimeOffset.TryParse(offsetString, out var offset));
      Assert.That(offset, Is.EqualTo(new DateTimeOffset(2018, 8, 1, 19, 21, 10, new TimeSpan(0, 2, 0, 0))));
    }

    [Test]
    public void TestReplacedItemPathParsing()
    {
      var path = SvnLook.ParseReplacedPath("     (from trunk/vendors/baker/bread.txt:r123)");
      Assert.That(path, Is.EqualTo("trunk/vendors/baker/bread.txt"));
    }

    [Test]
    public void TestItemActionParsing([Values("A ", "D ", "U ", "_U", "UU")] string svnAction)
    {
      Assert.That(SvnLook.ParseAction(svnAction), Is.Not.EqualTo(RepositoryItemAction.None));
    }

    [Test]
    public void TestEvaluateNodeKind()
    {
      Assert.That(SvnLook.EvaluateNodeKind("/test/"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.EvaluateNodeKind("test/"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.EvaluateNodeKind("test\\"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.EvaluateNodeKind("\\test\\test\\"), Is.EqualTo(RepositoryItemNodeKind.Directory));

      Assert.That(SvnLook.EvaluateNodeKind("/test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("test/test.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("/test/.test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("\\test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("\\test\\_"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("\\test\\_.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.EvaluateNodeKind("\\test\\.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
    }

    [Test]
    public void TestAffectedItems()
    {
      var changedString =
@"
A + trunk/vendors/baker/toast.txt
    (von trunk/vendors/baker/bread.txt:r63)
D   trunk/vendors/baker/bread.txt";

      var items = SvnLook.ParseAffectedItems(changedString);
      Assert.True(items.Count == 2);
      Assert.True(items.Any(x => x.Path == "trunk/vendors/baker/toast.txt" && x.Action == RepositoryItemAction.Add && x.NodeKind == RepositoryItemNodeKind.File));
      Assert.True(items.Any(x => x.Path == "trunk/vendors/baker/bread.txt" && x.Action == RepositoryItemAction.Replace && x.NodeKind == RepositoryItemNodeKind.File));
    }
  }
}