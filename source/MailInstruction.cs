// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailInstruction.cs" company="Silverseed.de">
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
  using System.IO;
  using System.Net.Mail;
  using log4net;

  /// <summary>
  /// Sends e-mails using the data specified via its properties.
  /// </summary>
  public class MailInstruction : Instruction
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string FromMailAddress { get; set; }

    public string ReplyToMailAddress { get; set; }

    public string ToMailAddresses { get; set; }

    public string CcMailAddresses { get; set; }

    public string BccMailAddresses { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string BodyTemplateFile { get; set; }

    protected override bool InternalExecute()
    {
      return MailManager.Instance.SendMail(this.BuildMailMessage());
    }

    /// <summary>
    /// Builds the mail address using the specified <paramref name="mailAddress"/>.
    /// </summary>
    /// <param name="mailAddress">The mail address for which to return a <see cref="MailAddress"/> instance.</param>
    /// <returns>A <see cref="MailAddress"/> instance if the specified <paramref name="mailAddress"/> is a valie mail address.
    /// Otherwise null.</returns>
    private static MailAddress BuildMailAddress(string mailAddress)
    {
      if (!String.IsNullOrEmpty(mailAddress))
      {
        return new MailAddress(mailAddress);
      }

      return null;
    }

    /// <summary>
    /// Builds the mail message to send.
    /// </summary>
    /// <returns>A <see cref="MailMessage"/> redy to be send.</returns>
    /// <remarks>
    /// The properties <see cref="FromMailAddress"/>, <see cref="ToMailAddresses"/>, <see cref="CcMailAddresses"/>,
    /// <see cref="BccMailAddresses"/>, <see cref="Subject"/> and <see cref="Body"/> are parsed replacing all
    /// tokens with their actual current values.
    /// </remarks>
    private MailMessage BuildMailMessage()
    {
      var mailMessage = new MailMessage();
      mailMessage.From = BuildMailAddress(SubversionInfoHub.Instance.ParseTokens(this.FromMailAddress));
      
      var toMailAddresses = SubversionInfoHub.Instance.ParseTokens(this.ToMailAddresses);
      if (!String.IsNullOrEmpty(toMailAddresses))
      {
        this.CheckMailAddresses(toMailAddresses);
        mailMessage.To.Add(toMailAddresses);
      }

      var ccMailAddresses = SubversionInfoHub.Instance.ParseTokens(this.CcMailAddresses);
      if (!String.IsNullOrEmpty(ccMailAddresses))
      {
        this.CheckMailAddresses(ccMailAddresses);
        mailMessage.CC.Add(ccMailAddresses);
      }

      var bccMailAddresses = SubversionInfoHub.Instance.ParseTokens(this.BccMailAddresses);
      if (!String.IsNullOrEmpty(bccMailAddresses))
      {
        this.CheckMailAddresses(bccMailAddresses);
        mailMessage.Bcc.Add(bccMailAddresses);
      }

      mailMessage.Subject = SubversionInfoHub.Instance.ParseTokens(this.Subject);
      if (!String.IsNullOrEmpty(this.BodyTemplateFile))
      {
        if (File.Exists(this.BodyTemplateFile))
        {
          try
          {
            this.Body = File.ReadAllText(this.BodyTemplateFile);
          }
          catch (Exception exception)
          {
            log.ErrorFormat("An exception occured when trying to read the mail body template file [{0}]:\n{1}", this.BodyTemplateFile, exception);
            throw;
          }
        }
        else
        {
          log.WarnFormat("The template file [{0}] for the mail body could not be found.", this.BodyTemplateFile);
        }
      }

      mailMessage.Body = SubversionInfoHub.Instance.ParseTokens(this.Body);
      mailMessage.ReplyTo = BuildMailAddress(SubversionInfoHub.Instance.ParseTokens(this.ReplyToMailAddress));

      return mailMessage;
    }

    private void CheckMailAddresses(string mailAddresses)
    {
      if (mailAddresses.Contains(";"))
      {
        log.WarnFormat("[{0}] contains ';'. To specify multiple e-mail addresses, you must use ',' as separator.");
      }
    }
  }
}
