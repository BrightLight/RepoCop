// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebInstructionTests.cs" company="Silverseed.de">
//    (c) 2020 Markus Hastreiter @ Silverseed.de
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
  public class WebInstructionTests
  {
    [Test]
    public void ExecuteReturnsFalseForInvalidUrl()
    {
      var instruction = new WebInstruction
      {
        Url = "invalid-url",
        HttpMethod = "GET",
        ContentType = "text/plain",
        Content = "test"
      };
      var result = instruction.Execute();
      Assert.That(result, Is.False);
    }

    [Test]
    public void PropertiesAreStored()
    {
      var instruction = new WebInstruction
      {
        Url = "http://example.com",
        HttpMethod = "POST",
        ContentType = "application/json",
        Content = "{\"key\":\"value\"}"
      };

      Assert.That(instruction.Url, Is.EqualTo("http://example.com"));
      Assert.That(instruction.HttpMethod, Is.EqualTo("POST"));
      Assert.That(instruction.ContentType, Is.EqualTo("application/json"));
      Assert.That(instruction.Content, Is.EqualTo("{\"key\":\"value\"}"));
    }
  }
}
