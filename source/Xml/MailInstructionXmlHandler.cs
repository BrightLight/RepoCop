// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailInstructionXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.SubversionHook.Xml
{
  using System;
  using System.Collections.Generic;
  using Silverseed.ComponentModel;

  internal class MailInstructionXmlHandler : InstructionXmlHandler
  {
    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      var mailInstruction = new MailInstruction();
      mailInstruction.Body = attributes.GetValueOrDefault("Body", String.Empty);
      mailInstruction.BodyTemplateFile = attributes.GetValueOrDefault("BodyTemplateFile", String.Empty);      
      mailInstruction.BccMailAddresses = attributes.GetValueOrDefault("BccMailAddresses", String.Empty);
      mailInstruction.CcMailAddresses = attributes.GetValueOrDefault("CcMailAddresses", String.Empty);
      mailInstruction.FromMailAddress = attributes.GetValueOrDefault("FromMailAddress", String.Empty);
      mailInstruction.ReplyToMailAddress = attributes.GetValueOrDefault("ReplyToMailAddress", String.Empty);
      mailInstruction.Subject = attributes.GetValueOrDefault("Subject", String.Empty);
      mailInstruction.ToMailAddresses = attributes.GetValueOrDefault("ToMailAddresses", String.Empty);
      return mailInstruction;
    }
  }
}
