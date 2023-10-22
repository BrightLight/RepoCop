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
  using Silverseed.Core;

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
      var fileName = attributes.GetValueOrDefault("FileName", string.Empty);
      this.executeInstruction.FileName = Environment.ExpandEnvironmentVariables(fileName);
      var arguments = attributes.GetValueOrDefault("Arguments", string.Empty);
      this.executeInstruction.Arguments = Environment.ExpandEnvironmentVariables(arguments);
      this.executeInstruction.NewLineReplacement = attributes.GetValueOrDefault("NewLineReplacement", string.Empty);
      
      int timeoutInMilliseconds;
      if (int.TryParse(attributes.GetValueOrDefault("TimeoutInMilliseconds", string.Empty), out timeoutInMilliseconds))
      {
        this.executeInstruction.TimeoutInMilliseconds = timeoutInMilliseconds;
      }

      int expectedExitCode;
      if (int.TryParse(attributes.GetValueOrDefault("ExpectedExitCode", string.Empty), out expectedExitCode))
      {
        this.executeInstruction.ExpectedExitCode = expectedExitCode;
      }

      if(Enum.TryParse(attributes.GetValueOrDefault("ExecutionMode", string.Empty), true, out ExecutionMode executionMode))
      {
        this.executeInstruction.ExecutionMode = executionMode;
      }

      return this.executeInstruction;
    }
  }
}
