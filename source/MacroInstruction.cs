// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MacroInstruction.cs" company="Silverseed.de">
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

namespace Silverseed.RepoCop
{
  using System;

  internal class MacroInstruction : Instruction
  {
    private System.Collections.Generic.List<Instruction> instructions = new System.Collections.Generic.List<Instruction>();

    public void AddInstruction(Instruction instruction)
    {
      this.instructions.Add(instruction);
    }

    protected override bool InternalExecute()
    {
      bool overallResult = true;
      this.instructions.ForEach(x => overallResult &= x.Execute());
      return overallResult;
    }

    public override string ToString()
    {
      var text = base.ToString();
      foreach (var instruction in this.instructions)
      {
        text += Environment.NewLine + "  " + instruction;
      }

      return text;
    }
  }
}