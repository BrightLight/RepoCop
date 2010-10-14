// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullInstruction.cs" company="Silverseed.de">
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

namespace Silverseed.SubversionHook
{
  /// <summary>
  /// A null implementation of <see cref="Instruction"/> that does nothing except 
  /// returning <c>true</c> when being executed.
  /// </summary>
  internal class NullInstruction : Instruction
  {
    /// <summary>
    /// Do nothing and return <c>true</c> (for "everything's ok").
    /// </summary>
    /// <returns>Always returns <c>true</c>.</returns>
    protected override bool InternalExecute()
    {
      return true;
    }
  }
}
