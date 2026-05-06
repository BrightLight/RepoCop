// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MacroInstructionTests.cs" company="Silverseed.de">
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
  public class MacroInstructionTests
  {
    [Test]
    public void ExecuteAllSuccessfulReturnsTrue()
    {
      var macro = new MacroInstruction();
      macro.AddInstruction(new NullInstruction());
      macro.AddInstruction(new NullInstruction());
      var result = macro.Execute();
      Assert.That(result, Is.True);
    }

    [Test]
    public void ExecuteOneFailedReturnsFalse()
    {
      var macro = new MacroInstruction();
      macro.AddInstruction(new NullInstruction());
      macro.AddInstruction(new FailInstruction("Fail"));
      var result = macro.Execute();
      Assert.That(result, Is.False);
    }

    [Test]
    public void InstructionsAreStored()
    {
      var macro = new MacroInstruction();
      var instruction1 = new NullInstruction();
      var instruction2 = new NullInstruction();
      macro.AddInstruction(instruction1);
      macro.AddInstruction(instruction2);
      Assert.That(macro.Instructions, Has.Count.EqualTo(2));
      Assert.That(macro.Instructions[0], Is.SameAs(instruction1));
      Assert.That(macro.Instructions[1], Is.SameAs(instruction2));
    }
  }
}
