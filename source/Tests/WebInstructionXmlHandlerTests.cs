// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebInstruction.cs" company="Silverseed.de">
//    (c) 2020 Markus Hastreiter @ Silverseed.de
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
  using System.Collections.Generic;
  using System.Net;
  using System.Net.Sockets;
  using NUnit.Framework;
  using WireMock;
  using WireMock.RequestBuilders;
  using WireMock.ResponseBuilders;
  using WireMock.Server;

  /// <summary>
  /// Unit tests for the <see cref="WebInstruction"/> class.
  /// </summary>
  [TestFixture]
  public class WebInstructionXmlHandlerTests
  {
    /// <summary>
    /// Checks that <see cref="WebInstruction"/> actually calls the specified URL.
    /// </summary>
    [Test]
    public void UrlIsSuccessfullyCalled()
    {
      var callCounter = 0;
      var server = WireMockServer.Start();
            
      server
        .Given(Request.Create().WithPath("/generic-webhook-trigger/invoke").UsingPost())
        .RespondWith(Response.Create()
          .WithStatusCode(200)
          .WithCallback(request =>
          {
            callCounter++;
            return new ResponseMessage { BodyOriginal = "Post was called" };
          }));
  
      try
      {
        var webInstruction = new WebInstruction
        {
          Url = $"{server.Urls[0]}/generic-webhook-trigger/invoke",
          HttpMethod = "POST",
          ContentType = "application/json",
          Content = "This is a test",
        };

        Assert.That(webInstruction.Execute(), Is.True);
        Assert.That(callCounter, Is.EqualTo(1));
      }
      finally
      {
        server.Stop();
        server.Dispose();
      }
    }

    /// <summary>
    /// Finds a free TCP port and returns it.
    /// </summary>
    /// <remarks>
    /// Found here: https://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
    /// </remarks>
    /// <returns>A free TCP port.</returns>
    private static int GetFreeTcpPort()
    {
      var tcpListener = new TcpListener(IPAddress.Loopback, 0);
      tcpListener.Start();
      var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
      tcpListener.Stop();
      return port;
    }
  }
}
