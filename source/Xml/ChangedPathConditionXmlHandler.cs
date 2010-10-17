// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangedPathConditionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using System;
  using System.Collections.Generic;
  using Silverseed.ComponentModel;

  internal class ChangedPathConditionXmlHandler : ConditionXmlHandler<ChangedPathCondition>
  {
    protected override ChangedPathCondition CreateCondition(Dictionary<string, string> attributes)
    {
      string changedPathRegExPattern;
      attributes.TryGetValue("ChangedPathRegExPattern", out changedPathRegExPattern);
      if (String.IsNullOrEmpty(changedPathRegExPattern))
      {
        changedPathRegExPattern = ".*";
      }

      var changedPathCondition = new ChangedPathCondition(changedPathRegExPattern);

      string actionAttribute;
      if (attributes.TryGetValue("Action", out actionAttribute))
      {
        var actions = actionAttribute.Split('+');
        foreach (var actionText in actions)
        {
          switch (actionText)
          {
            case "Add":
              changedPathCondition.Actions.Add(RepositoryItemAction.Add);
              break;
            case "Delete":
              changedPathCondition.Actions.Add(RepositoryItemAction.Delete);
              break;
            case "Modify":
              changedPathCondition.Actions.Add(RepositoryItemAction.Modifiy);
              break;
            case "Replace":
              changedPathCondition.Actions.Add(RepositoryItemAction.Replace);
              break;
            default:
              //// log unkown action
              break;
          }
        }
      }

      return changedPathCondition;
    }
  }
}
