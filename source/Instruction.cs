// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Instruction.cs" company="Silverseed.de">
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
  using Silverseed.Core.Conditions;

  public abstract class Instruction
  {
    public ICondition Condition { get; set; }

    /// <summary>
    /// Executes this instruction.
    /// </summary>
    /// <returns><c>True</c> if no errors occured. <c>False</c> if something went wrong.</returns>
    public bool Execute()
    {
      if ((this.Condition == null) || this.Condition.State)
      {
        return this.InternalExecute();
      }

      return true;
    }

    protected abstract bool InternalExecute();

    public override string ToString()
    {
      var text = base.ToString() + Environment.NewLine + "  " + (this.Condition != null ? this.Condition.ToString() : "<no condition>");
      return text;
    }
  }
}