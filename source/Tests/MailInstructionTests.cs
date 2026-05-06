// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailInstructionTests.cs" company="Silverseed.de">
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

namespace Silverseed.RepoCop.Tests
{
  using System;
  using System.Net.Mail;
  using NUnit.Framework;

  [TestFixture]
  public class MailInstructionTests
  {
    private class MockMailServer : IMailServer
    {
      public Guid Id { get; } = Guid.NewGuid();
      public string Name { get; } = "MockMailServer";
      public MailMessage LastMessage { get; private set; }
      public bool Result { get; set; } = true;

      public bool SendMail(MailMessage mailMessage)
      {
        this.LastMessage = mailMessage;
        return this.Result;
      }
    }

    [Test]
    public void ExecuteSendsMailViaMailManager()
    {
      MailManager.Flush();
      var mockMailServer = new MockMailServer();
      MailManager.Instance.RegisterMailServer(mockMailServer);
      try
      {
        var instruction = new MailInstruction
        {
          FromMailAddress = "from@example.com",
          ToMailAddresses = "to@example.com",
          Subject = "Test Subject",
          Body = "Test Body"
        };

        var result = instruction.Execute();

        Assert.Multiple(() =>
        {
          Assert.That(result, Is.True);
          Assert.That(mockMailServer.LastMessage, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
          Assert.That(mockMailServer.LastMessage.From.Address, Is.EqualTo("from@example.com"));
          Assert.That(mockMailServer.LastMessage.To[0].Address, Is.EqualTo("to@example.com"));
          Assert.That(mockMailServer.LastMessage.Subject, Is.EqualTo("Test Subject"));
          Assert.That(mockMailServer.LastMessage.Body, Is.EqualTo("Test Body"));
        });
      }
      finally
      {
        MailManager.Instance.UnregisterMailServer(mockMailServer);
      }
    }

    [Test]
    public void PropertiesAreStored()
    {
      var instruction = new MailInstruction
      {
        FromMailAddress = "from@example.com",
        ReplyToMailAddress = "replyto@example.com",
        ToMailAddresses = "to1@example.com,to2@example.com",
        CcMailAddresses = "cc@example.com",
        BccMailAddresses = "bcc@example.com",
        Subject = "Subject",
        Body = "Body",
        BodyTemplateFile = "Template.txt"
      };

      Assert.That(instruction.FromMailAddress, Is.EqualTo("from@example.com"));
      Assert.That(instruction.ReplyToMailAddress, Is.EqualTo("replyto@example.com"));
      Assert.That(instruction.ToMailAddresses, Is.EqualTo("to1@example.com,to2@example.com"));
      Assert.That(instruction.CcMailAddresses, Is.EqualTo("cc@example.com"));
      Assert.That(instruction.BccMailAddresses, Is.EqualTo("bcc@example.com"));
      Assert.That(instruction.Subject, Is.EqualTo("Subject"));
      Assert.That(instruction.Body, Is.EqualTo("Body"));
      Assert.That(instruction.BodyTemplateFile, Is.EqualTo("Template.txt"));
    }
  }
}
