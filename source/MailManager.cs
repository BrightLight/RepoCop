// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailManager.cs" company="Silverseed.de">
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

namespace Silverseed.RepoCop
{
  using System.Collections.Generic;
  using System.Net.Mail;
  using log4net;

  /// <summary>
  /// This singleton manages the available mail servers.
  /// </summary>
  public class MailManager
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// A list of all mail servers know to this program.
    /// </summary>
    private readonly List<IMailServer> registeredMailServers = new List<IMailServer>();

    /// <summary>
    /// The singleton instance of <see cref="MailManager"/>.
    /// </summary>
    private static MailManager instance;

    /// <summary>
    /// Gets the singleton instance of <see cref="MailManager"/>.
    /// A new instance of <see cref="MailManager"/> is created if none exists yet.
    /// </summary>
    public static MailManager Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new MailManager();
        }

        return instance;
      }
    }

    /// <summary>
    /// Registers the specified <paramref name="mailServer"/>.
    /// </summary>
    /// <param name="mailServer">The new <see cref="IMailServer"/> to register.</param>
    public void RegisterMailServer(IMailServer mailServer)
    {
      this.registeredMailServers.Add(mailServer);
    }

    /// <summary>
    /// Unregisters the specified <paramref name="mailServer"/>.
    /// </summary>
    /// <param name="mailServer">The <see cref="IMailServer"/> to unregister.</param>
    /// <returns><c>True</c> if the specified <paramref name="mailServer"/> was successfully unregistered. 
    /// <c>False</c> if the mail server could not be removed or was never registered after all.</returns>
    public bool UnregisterMailServer(IMailServer mailServer)
    {
      return this.registeredMailServers.Remove(mailServer);
    }

    /// <summary>
    /// Sends the specified message using the default mail server (the mail server that was the first to be registered).
    /// </summary>
    /// <param name="mailMessage">A <see cref="MailMessage"/> that contains the message to send.</param>
    /// <returns><c>True</c> if the message could be successfully sent. Otherwise <c>False</c>.</returns>
    public bool SendMail(MailMessage mailMessage)
    {
      if (this.registeredMailServers.Count > 0)
      {
        return this.registeredMailServers[0].SendMail(mailMessage);
      }

      log.ErrorFormat("Mail could not be send because there are no mail servers registered.");
      return false;
    }
  }
}
