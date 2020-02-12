// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebInstructionXmlHandler.cs" company="Silverseed.de">
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
namespace Silverseed.RepoCop.Xml
{
  using System.Collections.Generic;
  using Silverseed.Core;

  internal class WebInstructionXmlHandler : InstructionXmlHandler
  {
    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      var webInstruction = new WebInstruction();
      webInstruction.ContentType = attributes.GetValueOrDefault("ContentType", "application/json");
      webInstruction.HttpMethod = attributes.GetValueOrDefault("HttpMethod", "POST");
      webInstruction.Url = attributes.GetValueOrDefault("Url", string.Empty);
      webInstruction.Content = attributes.GetValueOrDefault("Content", string.Empty);
      return webInstruction;
    }
  }
}