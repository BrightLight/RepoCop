// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineInstructionTests.cs" company="Silverseed.de">
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
  using System.Diagnostics;
  using System.IO;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Silverseed.RepoCop.Subversion;
  using VerifyNUnit;

  /// <summary>
  /// Unit tests for the <see cref="CommandLineInstruction"/> class.
  /// </summary>
  [TestFixture]
  public class CommandLineInstructionTests
  {
    private const string BatchFileName = "EchoParams.bat";

    [Test]
    public void ExpectedExitCode0Test()
    {
      var commandLineInstruction = new CommandLineInstruction
      {
        FileName = BuildFullFileNameWithPath(BatchFileName),
        Arguments = "0 TextParam ErrorParam",
      };
      var result = commandLineInstruction.Execute();
      Assert.That(result, Is.True);
    }

    [Test]
    public void ExitCodeDiffersFromExpectedExitCodeMustFail()
    {
      var commandLineInstruction = new CommandLineInstruction
      {
        FileName = BuildFullFileNameWithPath(BatchFileName),
        Arguments = "5 TextParam",
      };

      var result = commandLineInstruction.Execute();
      Assert.That(result, Is.False);
    }

    [Test]
    public void ExpectedExitCodeDiffersFrom0Test()
    {
      var batchFile = BuildFullFileNameWithPath(BatchFileName);
      var commandLineInstruction = new CommandLineInstruction
      {
        FileName = batchFile,
        Arguments = "22 TextParam",
        ExpectedExitCode = 22,
      };
      var result = commandLineInstruction.Execute();
      Assert.That(result, Is.True);
    }

    [Test]
    public void StandardOutputIsLogged()
    {
      const string standardParameterText = "TextParam";
      var memoryStream = new MemoryStream();
      var streamWriter = new StreamWriter(memoryStream);
      var commandLineInstruction = new CommandLineInstruction(streamWriter, null)
      {
        FileName = BuildFullFileNameWithPath(BatchFileName),
        Arguments = "0 " + standardParameterText,
      };
      var result = commandLineInstruction.Execute();
      Assert.That(result, Is.True);
      streamWriter.Flush();
      memoryStream.Position = 0;
      var streamReader = new StreamReader(memoryStream);
      Assert.That(streamReader.ReadToEnd(), Is.EqualTo(standardParameterText));
    }

    [Test]
    public void StandardErrorIsLogged()
    {
      const string standardParameterText = "TextParam";
      const string errorParameterText = "\"An error has occured\"";
      var memoryStream = new MemoryStream();
      var streamWriter = new StreamWriter(memoryStream);
      var commandLineInstruction = new CommandLineInstruction(null, streamWriter)
      {
        FileName = BuildFullFileNameWithPath(BatchFileName),
        Arguments = "0 " + standardParameterText + " " + errorParameterText,
      };

      var result = commandLineInstruction.Execute();
      Assert.That(result, Is.True);
      streamWriter.Flush();
      memoryStream.Position = 0;
      var streamReader = new StreamReader(memoryStream);
      Assert.That(streamReader.ReadToEnd().Trim(), Is.EqualTo(errorParameterText));
    }

    [Test]
    public void NewLinesAreReplacedWithEmptySpace()
    {
      var commandLineInstruction = new CommandLineInstructionTest
      {
        FileName = "Foo",
        Arguments = "first line" + Environment.NewLine + "second line" + Environment.NewLine + "third line",
      };
      var processStartInfo = commandLineInstruction.CreateProcessStartInfo();
      Assert.That(processStartInfo, Is.Not.Null);
      Assert.That(processStartInfo.Arguments, Is.EqualTo("first line second line third line"));
    }

    /// <summary>
    /// Confirm that the <see cref="CommandLineInstruction"/> class can handle the case where the affected files are written to a file.
    /// </summary>
    [Test]
    public Task TokensWithAtPrefixWriteContentIntoFile()
    {
      // arrange
      List<IRepoAffectedItem> affectedItems =
      [
        new SvnLookRepoAffectedItem(RepositoryItemAction.Add, RepositoryItemNodeKind.File, "trunk/AB/MyFile.txt"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Delete, RepositoryItemNodeKind.File, "trunk/AB/MyFile2.txt"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Modify, RepositoryItemNodeKind.File, "trunk/resources/image1.png"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Modify, RepositoryItemNodeKind.File, "trunk/resources/image2.png"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Modify, RepositoryItemNodeKind.File, "trunk/resources/image3.png"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Modify, RepositoryItemNodeKind.File, "trunk/resources/image4.png"),
          new SvnLookRepoAffectedItem(RepositoryItemAction.Modify, RepositoryItemNodeKind.File, "trunk/resources/image5.png"),
        ];
      List<string> capabilities = ["MergeInfo"];
      RepositoryInfoHub.Instance.RepoChangeInfo = new SvnLookRepoChangeInfo(HookType.PostCommit, "AB", "My first commit", 13, new DateTime(2022, 10, 1), affectedItems, capabilities);

      var commandLineInstruction = new CommandLineInstructionTest
      {
        FileName = "LogAllCommits.bat",
        Arguments = "--author #author# --revision #revision# --affectedfiles \"#@affectedfiles#\"",
      };

      // act
      var processStartInfo = commandLineInstruction.CreateProcessStartInfo();

      // assert
      Assert.That(processStartInfo, Is.Not.Null);
      var expectedFileName = Path.GetTempPath() + "affectedfiles-13";
      Assert.That(processStartInfo.Arguments, Is.EqualTo($"--author AB --revision 13 --affectedfiles \"@{expectedFileName}\""));
      Assert.That(expectedFileName, Does.Exist);
      try
      {
        var fileContent = File.ReadAllText(expectedFileName);
        return Verifier.Verify(fileContent)
          .UseDirectory("VerifiedResults");
      }
      finally
      {
        File.Delete(expectedFileName);
      }
    }

    /// <summary>
    /// Adds the directory that contains the test assembly to the <paramref name="fileName"/>. 
    /// </summary>
    /// <param name="fileName">The name of a file that is used by tests.</param>
    /// <returns>The <paramref name="fileName"/> with the test directory added.</returns>
    private string BuildFullFileNameWithPath(string fileName)
    {
      var testDirectory = TestContext.CurrentContext.TestDirectory;
      var fileNameWithPath = Path.Combine(testDirectory, fileName);
      return fileNameWithPath;
    }

    private class CommandLineInstructionTest : CommandLineInstruction
    {
      protected override bool InternalExecute()
      {
        return base.InternalExecute();
      }

      new public ProcessStartInfo CreateProcessStartInfo()
      {
        return base.CreateProcessStartInfo();
      }
    }
  }
}
