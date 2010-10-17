// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using System.Collections.Generic;
  using Silverseed.ComponentModel.Xml;

  public abstract class InstructionXmlHandler : NullXmlHandler
  {
    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      var newInstruction = this.CreateInstruction(attributes);
      if (ObjectFactory.Instance.ObjectStack.Count > 0)
      {
        var macroInstruction = ObjectFactory.Instance.ObjectStack.Peek() as MacroInstruction;
        if (macroInstruction != null)
        {
          macroInstruction.AddInstruction(newInstruction);
        }
      }

      ObjectFactory.Instance.ObjectStack.Push(newInstruction);

      base.ProcessStartElement(name, attributes);
    }

    public override void ProcessEndElement(string name)
    {
      ObjectFactory.Instance.ObjectStack.Pop();

      base.ProcessEndElement(name);
    }

    protected abstract Instruction CreateInstruction(Dictionary<string, string> attributes);
  }
}
