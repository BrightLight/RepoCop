using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silverseed.SubversionHook.Xml
{
  internal class AuthorConditionXmlHandler: ConditionXmlHandler<AuthorCondition>
  {
    private string separator = ";";

    public override void ProcessText(string value)
    {
      if (this.Condition != null)
      {
        this.Condition.Authors = value.Split(this.separator.ToCharArray()).ToList();
      }

      base.ProcessText(value);
    }

    protected override AuthorCondition CreateCondition(Dictionary<string, string> attributes)
    {
      attributes.TryGetValue("Separator", out separator);
      return new AuthorCondition(String.Empty);
    }
  }
}
