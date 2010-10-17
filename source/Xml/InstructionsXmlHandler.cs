﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silverseed.RepoCop.Xml
{
  internal class InstructionsXmlHandler: InstructionXmlHandler
  {
    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      return new MacroInstruction();
    }
  }
}
