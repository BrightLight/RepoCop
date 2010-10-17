// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineInstructionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using System;
  using System.Collections.Generic;
  using Silverseed.ComponentModel;

  internal class CommandLineInstructionXmlHandler : InstructionXmlHandler
  {
    private CommandLineInstruction executeInstruction;

    public override void ProcessText(string value)
    {
      if (this.executeInstruction != null)
      {
        this.executeInstruction.FileName = value;
      }
    }

    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      this.executeInstruction = new CommandLineInstruction();
      this.executeInstruction.FileName = attributes.GetValueOrDefault("FileName", String.Empty);
      this.executeInstruction.Arguments = attributes.GetValueOrDefault("Arguments", String.Empty);
      
      int timeoutInMilliseconds;
      if (Int32.TryParse(attributes.GetValueOrDefault("TimeoutInMilliseconds", String.Empty), out timeoutInMilliseconds))
      {
        this.executeInstruction.TimeoutInMilliseconds = timeoutInMilliseconds;
      }

      int expectedExitCode;
      if (Int32.TryParse(attributes.GetValueOrDefault("ExpectedExitCode", String.Empty), out expectedExitCode))
      {
        this.executeInstruction.ExpectedExitCode = expectedExitCode;
      }

      return this.executeInstruction;
    }
  }
}
