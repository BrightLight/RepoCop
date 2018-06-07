// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionsXmlHandler.cs" company="Silverseed.de">
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
  using System;
  using System.Collections.Generic;
  using Silverseed.Core.Conditions;

  internal class ConditionsXmlHandler : ConditionXmlHandler<CompositeCondition>
  {
    protected override CompositeCondition CreateCondition(Dictionary<string, string> attributes)
    {
      if (attributes.TryGetValue("Type", out var combinationType))
      {
        switch (combinationType)
        {
          case "Or":
            return new OrCondition(false);
          case "And":
            return new AndCondition(false);
        }
      }

      return new AndCondition(false);
    }

    public override void ProcessEndElement(string name)
    {
      ObjectFactory.Instance.ObjectStack.Pop();
      
      base.ProcessEndElement(name);
    }
  }
}
