﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookManagerTests.cs" company="Silverseed.de">
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
  using System;
  using System.IO;
  using System.Text;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using VerifyNUnit;

  /// <summary>
  /// Unit tests for the <see cref="HookManager"/> class.
  /// </summary>
  [TestFixture]
  public class HookManagerTests
  {
    /// <summary>
    /// Tests that a capability condition is read correctly from an XML configuration.
    /// </summary>
    [Test]
    public void CapabilityIsReadCorrectly()
    {
      var config = Resources.Resources.CapabilityConfigTest;
      
      // read XML from file as stream
      var byteArray = Encoding.UTF8.GetBytes(config);
      var stream = new MemoryStream(byteArray);
      
      var instruction = HookManager.ReadHookConfiguration(stream);
      Assert.That(instruction, Is.Not.Null);
      var macroInstruction = instruction as MacroInstruction;
      Assert.That(macroInstruction, Is.Not.Null);
      var firstInstruction = macroInstruction.Instructions[0];
      var capabilityCondition= firstInstruction.Condition as CapabilityCondition;
      Assert.That(capabilityCondition, Is.Not.Null);
      Assert.That(capabilityCondition.Capability, Is.EqualTo("MergeInfo"));
    }

    /// <summary>
    /// Tests that a TimeWindow condition is read correctly from an XML configuration.
    /// </summary>
    [Test]
    public void TimeWindowConditionIsReadCorrectly()
    {
      var config = Resources.Resources.TimeWindowConditionConfigTest;

      // read XML from file as stream
      var byteArray = Encoding.UTF8.GetBytes(config);
      var stream = new MemoryStream(byteArray);

      var instruction = HookManager.ReadHookConfiguration(stream);
      Assert.That(instruction, Is.Not.Null);
      var macroInstruction = instruction as MacroInstruction;
      Assert.That(macroInstruction, Is.Not.Null);
      var firstInstruction = macroInstruction.Instructions[0];
      var timeWindowCondition = firstInstruction.Condition as TimeWindowCondition;
      Assert.That(timeWindowCondition, Is.Not.Null);
      Assert.That(timeWindowCondition.StartTime, Is.EqualTo(new DateTime(2022, 2, 1, 8, 30, 0)));
      Assert.That(timeWindowCondition.EndTime, Is.EqualTo(new DateTime(2022, 2, 1, 11, 00, 0)));
    }
    
    /// <summary>
         /// Verify that a configuration that contains all possible features can be read correctly.
         /// </summary>
    [Test]
    public Task ReadFullFeatureConfig()
    {
      // arrange
      var config = Resources.Resources.FullFeatureConfigTest;

      // read XML from file as stream
      var byteArray = Encoding.UTF8.GetBytes(config);
      var stream = new MemoryStream(byteArray);

      // act
      var instructions = HookManager.ReadHookConfiguration(stream);

      // assert
      Assert.That(instructions, Is.Not.Null);
      return Verifier.Verify(instructions.ToString())
        .UseDirectory("VerifiedResults");
    }
  }
}