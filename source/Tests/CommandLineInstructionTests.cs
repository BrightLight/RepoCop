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
  using System.IO;
  using System.Linq;
  using System.Text;
  using NUnit.Framework;
  using System.Diagnostics;

  [TestFixture]
  public class CommandLineInstructionTests
  {
    [Test]
    public void ExpectedExitCode0Test()
    {
      var commandLineInstruction = new CommandLineInstruction();
      commandLineInstruction.FileName = "EchoParams.bat";
      commandLineInstruction.Arguments = "0 TextParam ErrorParam";
      var result = commandLineInstruction.Execute();
      Assert.IsTrue(result);
    }

    [Test]
    public void ExitCodeDiffersFromExpectedExitCodeMustFail()
    {
      var commandLineInstruction = new CommandLineInstruction();
      commandLineInstruction.FileName = "EchoParams.bat";
      commandLineInstruction.Arguments = "5 TextParam";
      var result = commandLineInstruction.Execute();
      Assert.IsFalse(result);
    }

    [Test]
    public void ExpectedExitCodeDiffersFrom0Test()
    {
      var commandLineInstruction = new CommandLineInstruction();
      commandLineInstruction.FileName = "EchoParams.bat";
      commandLineInstruction.Arguments = "22 TextParam";
      commandLineInstruction.ExpectedExitCode = 22;
      var result = commandLineInstruction.Execute();
      Assert.IsTrue(result);
    }

    [Test]
    public void StandardOutputIsLogged()
    {
      const string standardParameterText = "TextParam";
      var memoryStream = new MemoryStream();
      var streamWriter = new StreamWriter(memoryStream);
      var commandLineInstruction = new CommandLineInstruction(streamWriter, null);
      commandLineInstruction.FileName = "EchoParams.bat";
      commandLineInstruction.Arguments = "0 " + standardParameterText;
      var result = commandLineInstruction.Execute();
      Assert.IsTrue(result);
      streamWriter.Flush();
      memoryStream.Position = 0;
      var streamReader = new StreamReader(memoryStream);
      Assert.AreEqual(standardParameterText, streamReader.ReadToEnd());
    }

    [Test]
    public void StandardErrorIsLogged()
    {
      const string standardParameterText = "TextParam";
      const string errorParameterText = "\"An error has occured\"";
      var memoryStream = new MemoryStream();
      var streamWriter = new StreamWriter(memoryStream);
      var commandLineInstruction = new CommandLineInstruction(null, streamWriter);
      commandLineInstruction.FileName = "EchoParams.bat";
      commandLineInstruction.Arguments = "0 " + standardParameterText + " " + errorParameterText;
      var result = commandLineInstruction.Execute();
      Assert.IsTrue(result);
      streamWriter.Flush();
      memoryStream.Position = 0;
      var streamReader = new StreamReader(memoryStream);
      Assert.AreEqual(errorParameterText, streamReader.ReadToEnd().Trim());
    }

    [Test]
    public void NewLinesAreReplacedWithEmptySpace()
    {
      var commandLineInstruction = new CommandLineInstructionTest();
      commandLineInstruction.FileName = "Foo";
      commandLineInstruction.Arguments = "first line" + Environment.NewLine + "second line" + Environment.NewLine + "third line";
      var processStartInfo = commandLineInstruction.CreateProcessStartInfo();
      Assert.That(processStartInfo, Is.Not.Null);
      Assert.That(processStartInfo.Arguments, Is.EqualTo("first line second line third line"));
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
