// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineInstructionXmlHandler.cs" company="Silverseed.de">
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
