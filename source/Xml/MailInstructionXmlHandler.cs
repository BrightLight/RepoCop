﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailInstructionXmlHandler.cs" company="Silverseed.de">
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
  using Silverseed.Core;

  internal class MailInstructionXmlHandler : InstructionXmlHandler
  {
    protected override Instruction CreateInstruction(Dictionary<string, string> attributes)
    {
      var mailInstruction = new MailInstruction();
      mailInstruction.Body = attributes.GetValueOrDefault("Body", String.Empty);
      var bodyTemplateFile = attributes.GetValueOrDefault("BodyTemplateFile", String.Empty);
      mailInstruction.BodyTemplateFile = Environment.ExpandEnvironmentVariables(bodyTemplateFile);
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
