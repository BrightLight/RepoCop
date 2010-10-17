// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookConfigServiceLocator.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.RepoCop.Xml
{
  using Silverseed.ComponentModel;
  using Silverseed.ComponentModel.Xml;
  
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
