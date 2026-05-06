// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailInstructionTests.cs" company="Silverseed.de">
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
  using NUnit.Framework;

  [TestFixture]
  public class FailInstructionTests
  {
    [Test]
    public void ExecuteReturnsFalse()
    {
      var instruction = new FailInstruction("Error message");
      var result = instruction.Execute();
      Assert.That(result, Is.False);
    }

    [Test]
    public void MessageIsStored()
    {
      var message = "Test error message";
      var instruction = new FailInstruction(message);
      Assert.That(instruction.Message, Is.EqualTo(message));
    }
  }
}
