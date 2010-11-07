// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookConfigServiceLocator.cs" company="Silverseed.de">
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
  using Silverseed.Core;
  using Silverseed.Core.Xml;
  
  /// <summary>
  /// Provides <see cref="IXmlHandler"/>s that can handle elements of of the repository hook XML configuration.
  /// </summary>
  internal class HookConfigServiceLocator : ServiceLocator<IXmlHandler>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HookConfigServiceLocator"/> class.
    /// </summary>
    public HookConfigServiceLocator()
    {
      this.RegisterHandler("Conditions", typeof(ConditionsXmlHandler));
      this.RegisterHandler("LogMessageCondition", typeof(LogMessageConditionXmlHandler));
      this.RegisterHandler("AuthorCondition", typeof(AuthorConditionXmlHandler));
      this.RegisterHandler("ChangedPathCondition", typeof(ChangedPathConditionXmlHandler));
      this.RegisterHandler("HookTypeCondition", typeof(HookTypeConditionXmlHandler));
      this.RegisterHandler("Instructions", typeof(InstructionsXmlHandler));
      this.RegisterHandler("CommandLineInstruction", typeof(CommandLineInstructionXmlHandler));
      this.RegisterHandler("FailInstruction", typeof(FailInstructionXmlHandler));
      this.RegisterHandler("MailInstruction", typeof(MailInstructionXmlHandler));
      this.RegisterHandler("SmtpServer", typeof(SmtpServerXmlHandler));
    }
  }
}
