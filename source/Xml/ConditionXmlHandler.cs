// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using System.Collections.Generic;
  using Silverseed.ComponentModel.Conditions;
  using Silverseed.ComponentModel.Xml;

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

      var currentInstruction = ObjectFactory.Instance.ObjectStack.Peek() as Instruction;
      if (currentInstruction != null)
      {
        var combinedCondition = currentInstruction.Condition as CombinedCondition;
        if (combinedCondition != null)
        {
          combinedCondition.Add(condition);
        }
        else
        {
          currentInstruction.Condition = condition;
        }
      }

      base.ProcessStartElement(name, attributes);
    }

    protected abstract T CreateCondition(Dictionary<string, string> attributes);
  }
}
