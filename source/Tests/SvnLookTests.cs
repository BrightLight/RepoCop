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

  /// <summary>
  /// Tests for the <see cref="SvnLook"/> class.
  /// </summary>
  [TestFixture]
  public class SvnLookTests
  {
    [Test]
    public void TestDateTimeOffsetParsing()
    {
      var offsetString = SvnLook.ParseDateTimeOffsetString("2018-08-01 19:21:10 +0200 (Mi, 01 Aug 2018)");
      Assert.That(DateTimeOffset.TryParse(offsetString, out var offset), Is.True);
      Assert.That(offset, Is.EqualTo(new DateTimeOffset(2018, 8, 1, 19, 21, 10, new TimeSpan(0, 2, 0, 0))));
    }

    [Test]
    public void TestReplacedItemPathParsing()
    {
      var path = SvnLook.ParseReplacedPath("     (from trunk/vendors/baker/bread.txt:r123)");
      Assert.That(path, Is.EqualTo("trunk/vendors/baker/bread.txt"));
    }

    /// <summary>
    /// Tests the parsing of the action string.
    /// </summary>
    /// <param name="svnAction">The SVN action string.</param>
    /// <returns>The parsed action.</returns>
    [TestCase("A ", ExpectedResult = RepositoryItemAction.Add)]
    [TestCase("D ", ExpectedResult = RepositoryItemAction.Delete)]
    [TestCase("U ", ExpectedResult = RepositoryItemAction.Modify)]
    [TestCase("_U", ExpectedResult = RepositoryItemAction.Modify)]
    [TestCase("UU", ExpectedResult = RepositoryItemAction.Modify)]
    public RepositoryItemAction TestItemActionParsing(string svnAction)
    {
      return SvnLook.ParseAction(svnAction);
    }

    [Test]
    public void TestEvaluateNodeKind()
    {
      Assert.That(SvnLook.DetermineNodeKind("/test/"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.DetermineNodeKind("test/"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.DetermineNodeKind("test\\"), Is.EqualTo(RepositoryItemNodeKind.Directory));
      Assert.That(SvnLook.DetermineNodeKind("\\test\\test\\"), Is.EqualTo(RepositoryItemNodeKind.Directory));

      Assert.That(SvnLook.DetermineNodeKind("/test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("test/test.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("/test/.test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("\\test"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("\\test\\_"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("\\test\\_.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
      Assert.That(SvnLook.DetermineNodeKind("\\test\\.txt"), Is.EqualTo(RepositoryItemNodeKind.File));
    }

    [Test]
    public void TestAffectedItems()
    {
      var changedString = @"
A   trunk/vendors/baker/toast.txt
U   trunk/vendors/baker/bakerman.txt
U   trunk/vendors/baker/oven.txt
A   trunk/vendors/baker/bread.txt";

      var items = SvnLook.ParseAffectedItems(changedString);
      Assert.That(items, Has.Count.EqualTo(4));
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/toast.txt" && x.Action == RepositoryItemAction.Add && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/bakerman.txt" && x.Action == RepositoryItemAction.Modify && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/oven.txt" && x.Action == RepositoryItemAction.Modify && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/bread.txt" && x.Action == RepositoryItemAction.Add && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
    }

    [Test]
    public void TestReplacedItemsAreTracked()
    {
      var changedString = @"
A + trunk/vendors/baker/toast.txt
    (von trunk/vendors/baker/bread.txt:r63)
D   trunk/vendors/baker/bread.txt";

      var items = SvnLook.ParseAffectedItems(changedString);
      Assert.That(items, Has.Count.EqualTo(2));
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/toast.txt" && x.Action == RepositoryItemAction.Add && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/bread.txt" && x.Action == RepositoryItemAction.Replace && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
    }

    [Test]
    public void TestCopySourceIsTracked()
    {
      var changedString = @"
_U  branches/123/vendors/baker/
U   branches/123/vendors/baker/toast.txt
A + branches/123/vendors/baker/bread.txt
    (from trunk/vendors/baker/bread.txt:r111)
U   branches/123/vendors/baker/oven.txt";

      var items = SvnLook.ParseAffectedItems(changedString);
      Assert.That(items, Has.Count.EqualTo(5));
      Assert.That(items.Any(x => x.Path == "/branches/123/vendors/baker/" && x.Action == RepositoryItemAction.Modify && x.NodeKind == RepositoryItemNodeKind.Directory), Is.True);
      Assert.That(items.Any(x => x.Path == "/branches/123/vendors/baker/toast.txt" && x.Action == RepositoryItemAction.Modify && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/branches/123/vendors/baker/bread.txt" && x.Action == RepositoryItemAction.Add && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/trunk/vendors/baker/bread.txt" && x.Action == RepositoryItemAction.None && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
      Assert.That(items.Any(x => x.Path == "/branches/123/vendors/baker/oven.txt" && x.Action == RepositoryItemAction.Modify && x.NodeKind == RepositoryItemNodeKind.File), Is.True);
    }
  }
}