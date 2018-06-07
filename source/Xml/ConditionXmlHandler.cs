// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionXmlHandler.cs" company="Silverseed.de">
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
  using Silverseed.Core.Conditions;
  using Silverseed.Core.Xml;

  public abstract class ConditionXmlHandler<T> : NullXmlHandler
    where T : ICondition
  {
    protected T Condition { get; set; }

    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      this.Condition = this.CreateCondition(attributes);
      ICondition condition = this.Condition;
      string negate;
      if (attributes.TryGetValue("Negate", out negate))
      {
        if (negate == "True")
        {
          condition = this.Condition.Not;
        }
      }

      switch (ObjectFactory.Instance.ObjectStack.Peek())
      {
        case Instruction instruction:
          instruction.Condition = condition;
          break;
        case CompositeCondition compositeCondition:
          compositeCondition.Add(condition);
          break;
      }

      if (condition is CompositeCondition)
      {
        ObjectFactory.Instance.ObjectStack.Push(condition);
      }

      base.ProcessStartElement(name, attributes);
    }

    protected abstract T CreateCondition(Dictionary<string, string> attributes);
  }
}
