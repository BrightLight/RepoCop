using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Silverseed.SubversionHook.Tests
{
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
  }
}
