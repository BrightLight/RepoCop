// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmtpServer.cs" company="Silverseed.de">
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

namespace Silverseed.SubversionHook
{
  using System;
  using System.Net.Mail;
  using log4net;

  /// <summary>
  /// Represents an SMTP server used to send mails.
  /// </summary>
  public class SmtpServer : IMailServer  
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpServer"/> class.
    /// </summary>
    /// <param name="name">A name by which this <see cref="SmtpServer"/> instance can be referenced and identified.</param>
    public SmtpServer(string name)
    {
      this.Id = Guid.NewGuid();
      this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpServer"/> class.
    /// </summary>
    /// <param name="name">A name by which this <see cref="SmtpServer"/> instance can be referenced and identified.</param>
    /// <param name="host">A <see cref="String"/> that contains the name or IP address of the host computer used for SMTP transactions.</param>
    /// <param name="port">The port used for SMTP transactions.</param>
    public SmtpServer(string name, string host, int port)
      : this(name)
    {
      this.Host = host;
      this.Port = port;
    }

    #region IMailServer Members

    /// <summary>
    /// Gets a unique id to identify this <see cref="IMailServer"/>.
    /// </summary>
    /// <value>A unique id used for internal referencing.</value>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets a name by which this <see cref="SmtpServer"/> instance can be referenced and identified.
    /// </summary>
    /// <value>The name to reference this instance.</value>
    public string Name { get; private set; }

    /// <summary>
    /// Gets a string that contains the name or IP address of the host computer used for SMTP transactions.
    /// </summary>
    /// <value>The name or IP address of a computer.</value>
    public string Host { get; private set; }

    /// <summary>
    /// Gets the port used for SMTP transactions.
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// Sends the specified message to an SMTP server for delivery.
    /// </summary>
    /// <param name="mailMessage">A <see cref="MailMessage"/> that contains the message to send.</param>
    /// <returns><c>True</c> if the message could be successfully handed to the SMTP server. Otherwise <c>False</c>.</returns>
    public bool SendMail(MailMessage mailMessage)
    {
      try
      {
        var smtpClient = this.BuildSmtpClient();
        smtpClient.Send(mailMessage);
        return true;
      }
      catch (Exception exception)
      {
        log.Error("An error occurred during an attempt to send an e-mail via smtp.", exception);
        return false;
      }
    }

    #endregion

    /// <summary>
    /// Builds the <see cref="SmtpClient"/> instance that best fits the available configuration data.
    /// </summary>
    /// <returns>A new <see cref="SmtpClient"/> instance.</returns>
    private SmtpClient BuildSmtpClient()
    {
      if (String.IsNullOrEmpty(this.Host))
      {
        return new SmtpClient();
      }

      if (this.Port == 0)
      {
        return new SmtpClient(this.Host);
      }

      return new SmtpClient(this.Host, this.Port);
    }
  }
}