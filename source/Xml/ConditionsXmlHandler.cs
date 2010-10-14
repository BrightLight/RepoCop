using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silverseed.ComponentModel;

namespace Silverseed.SubversionHook.Xml
{
  internal class ConditionsXmlHandler : ConditionXmlHandler<CombinedCondition>
  {
    protected override CombinedCondition CreateCondition(Dictionary<string, string> attributes)
    {
      string combinationType;
      if (attributes.TryGetValue("Type", out combinationType))
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
  }
}
