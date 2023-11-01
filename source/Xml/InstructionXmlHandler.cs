// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionXmlHandler.cs" company="Silverseed.de">
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
  using System.Collections.Generic;
  using Silverseed.Core.Xml;

  public abstract class InstructionXmlHandler : NullXmlHandler
  {
    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      var newInstruction = this.CreateInstruction(attributes);
      if (ObjectFactory.Instance.ObjectStack.Count > 0)
      {
        if (ObjectFactory.Instance.ObjectStack.Peek() is MacroInstruction macroInstruction)
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
