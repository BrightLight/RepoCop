using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silverseed.ComponentModel;

namespace Silverseed.SubversionHook.Xml
{
  internal class LogMessageConditionXmlHandler : ConditionXmlHandler<LogMessageCondition>
  {
    protected override LogMessageCondition CreateCondition(Dictionary<string, string> attributes)
    {
      string logMessageRegExPattern;
      attributes.TryGetValue("LogMessageRegExPattern", out logMessageRegExPattern);
      return new LogMessageCondition(logMessageRegExPattern);
    }
  }
}
