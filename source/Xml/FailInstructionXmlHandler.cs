// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailInstructionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using System.Collections.Generic;

  internal class FailInstructionXmlHandler : InstructionXmlHandler
  {
    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      string message;
      attributes.TryGetValue("Message", out message);
      return new FailInstruction(message);
    }
  }
}
