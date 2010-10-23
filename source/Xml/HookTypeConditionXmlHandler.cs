// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookTypeConditionXmlHandler.cs" company="Silverseed.de">
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
  using System.Collections.Generic;
  using Silverseed.ComponentModel;

  internal class HookTypeConditionXmlHandler : ConditionXmlHandler<HookTypeCondition>
  {
    protected override HookTypeCondition CreateCondition(Dictionary<string, string> attributes)
    {
      var hookTypeAsText = attributes.GetValueOrDefault("HookType", string.Empty);
      HookType hookType;
      switch (hookTypeAsText)
      {
        case "StartCommit": 
          hookType = HookType.StartCommit;
          break;
        case "PreCommit": 
          hookType = HookType.PreCommit;
          break;
        case "PostCommit":
          hookType = HookType.PostCommit;
          break;
        default:
          hookType = HookType.Undefined;
          break;
      }

      return new HookTypeCondition() { HookType = hookType };
    }
  }
}
