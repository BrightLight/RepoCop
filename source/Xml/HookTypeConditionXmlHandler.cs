// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookTypeConditionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
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
