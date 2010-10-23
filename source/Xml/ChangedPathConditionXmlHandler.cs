// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangedPathConditionXmlHandler.cs" company="Silverseed.de">
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
