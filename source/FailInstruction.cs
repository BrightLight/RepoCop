// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailInstruction.cs" company="Silverseed.de">
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

  /// <summary>
  /// An instruction that always fails (and thus aborts) the commit.
  /// </summary>
  internal class FailInstruction : Instruction
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FailInstruction"/> class.
    /// </summary>
    /// <param name="message">The message that will be sent back to the committer when this instruction is executed.</param> 
    public FailInstruction(string message)
    {
      this.Message = message;
    }

    /// <summary>
    /// Executes this instruction.
    /// </summary>
    /// <returns><c>false</c> to indicate that the commit should be aborted.</returns>
    protected override bool InternalExecute()
    {
      Console.Error.WriteLine(RepositoryInfoHub.Instance.ParseTokens(this.Message));
      return false; // this instruction always fails (hence its name)
    }

    /// <summary>
    /// Gets or sets the message that will be sent back to the committer when this instruction is executed.
    /// </summary>
    public string Message { get; set; }
  }
}
