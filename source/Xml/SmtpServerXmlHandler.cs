// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmtpServerXmlHandler.cs" company="Silverseed.de">
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
  using Silverseed.Core.Xml;

  internal class SmtpServerXmlHandler : NullXmlHandler
  {
    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      base.ProcessStartElement(name, attributes);

      int.TryParse(attributes.GetValueOrDefault("Port", string.Empty), out var port);
      var newSmtpServer = new SmtpServer(attributes.GetValueOrDefault("Name", null), attributes.GetValueOrDefault("Host", null), port);
      MailManager.Instance.RegisterMailServer(newSmtpServer);
    }
  }
}
