// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplacementTokenXmlHandler.cs" company="Silverseed.de">
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
  using System.Linq;
  using System.Text;
  using log4net;
  using Silverseed.Core;
  using Silverseed.Core.Xml;
  using System.Text.RegularExpressions;

  internal class ReplacementTokenXmlHandler : NullXmlHandler
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void ProcessStartElement(string name, Dictionary<string, string> attributes)
    {
      base.ProcessStartElement(name, attributes);

      var tokenName = '#' + attributes.GetValueOrDefault("TokenName", string.Empty) + '#';
      var value = attributes.GetValueOrDefault("Value", string.Empty);
      var regExPattern = attributes.GetValueOrDefault("RegExPattern", string.Empty);
      if (string.IsNullOrEmpty(tokenName))
      {
        log.Warn(string.Format("No 'TokenName' was specified for {0}", name));
        return;
      }

      if (string.IsNullOrEmpty(value))
      {
        log.Warn(string.Format("No 'Value' was specified for {0}", name));
        return;
      }

      if (string.IsNullOrEmpty(regExPattern))
      {
        RepositoryInfoHub.Instance.AddToken(tokenName, () => value);
      }
      else
      {
        var regex = new Regex(regExPattern, RegexOptions.Compiled);
        RepositoryInfoHub.Instance.AddToken(tokenName, () =>
          {
            var match = regex.Match(RepositoryInfoHub.Instance.ParseTokens(value));
            if ((match != null)
              && (match.Groups.Count > 1))
            {
              return match.Groups[1].Value;
            }

            return string.Empty;
          });
      }
    }
  }
}
