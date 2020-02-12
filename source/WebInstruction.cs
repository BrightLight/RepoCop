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
namespace Silverseed.RepoCop
{
  using System;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using log4net;

  /// <summary>
  /// Calls the specified <see cref="Url"/> with the <see cref="Content"/>.
  /// </summary>
  public class WebInstruction : Instruction
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected override bool InternalExecute()
    {
      try
      {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;

        using (var httpClient = new HttpClient(handler))
        {
          var requestUri = RepositoryInfoHub.Instance.ParseTokens(this.Url);
          using (var request = new HttpRequestMessage(new HttpMethod(this.HttpMethod), requestUri))
          {
            var content = RepositoryInfoHub.Instance.ParseTokens(this.Content);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(this.ContentType);

            var response = httpClient.SendAsync(request);
            return response.Result.IsSuccessStatusCode;
          }
        }
      }
      catch(Exception exception)
      {
        log.Error($"Error while executing {nameof(WebInstruction)}.", exception);
        return false;
      }
    }

    public string Url { get; set; }

    public string HttpMethod { get; set; }

    public string ContentType { get; set; }

    public string Content { get; set; }
  }
}
