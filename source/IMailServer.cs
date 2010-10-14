// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMailServer.cs" company="Silverseed.de">
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

  /// <summary>
  /// Represents a mail server that can be used to sent mails (e.g. an SMTP server, Exchange server, etc).
  /// </summary>
  public interface IMailServer
  {
    /// <summary>
    /// Gets a unique id to identify this <see cref="IMailServer"/>.
    /// </summary>
    /// <value>A unique id used for internal referencing.</value>
    Guid Id { get; }

    /// <summary>
    /// Gets a name by which this <see cref="SmtpServer"/> instance can be referenced and identified.
    /// </summary>
    /// <value>The name to reference this instance.</value>
    string Name { get; }

    /// <summary>
    /// Sends the specified message.
    /// </summary>
    /// <param name="mailMessage">A <see cref="MailMessage"/> that contains the message to send.</param>
    /// <returns><c>True</c> if the message could be successfully sent. Otherwise <c>False</c>.</returns>
    bool SendMail(MailMessage mailMessage);
  }
}