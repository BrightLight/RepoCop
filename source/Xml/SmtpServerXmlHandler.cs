// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmtpServerXmlHandler.cs" company="Silverseed.de">
//    (c) 2010 Markus Hastreiter @ Silverseed.de
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Silverseed.SubversionHook.Xml
{
  using System;
  using System.Collections.Generic;
  using Silverseed.ComponentModel;
  using Silverseed.ComponentModel.Xml;

  internal class SmtpServerXmlHandler : NullXmlHandler
  {
    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      base.ProcessStartElement(name, attributes);

      int port;
      Int32.TryParse(attributes.GetValueOrDefault("Port", String.Empty), out port);
      var newSmtpServer = new SmtpServer(attributes.GetValueOrDefault("Name", null), attributes.GetValueOrDefault("Host", null), port);
      MailManager.Instance.RegisterMailServer(newSmtpServer);
    }
  }
}
