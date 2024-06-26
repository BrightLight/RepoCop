﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorConditionXmlHandler.cs" company="Silverseed.de">
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
  using System.Collections.Generic;
  using System.Linq;
  using Silverseed.Core;

  internal class AuthorConditionXmlHandler : ConditionXmlHandler<AuthorCondition>
  {
    private const string DefaultSeparator = ";";

    private string separator = DefaultSeparator;

    public override void ProcessText(string value)
    {
      if (this.Condition != null)
      {
        this.Condition.Authors = value.Split(this.separator.ToCharArray()).ToList();
      }

      base.ProcessText(value);
    }

    protected override AuthorCondition CreateCondition(Dictionary<string, string> attributes)
    {
      this.separator = attributes.GetValueOrDefault("Separator", DefaultSeparator);
      var caseSensitive = attributes.GetValueOrDefault("CaseSensitive", "false") == "true";
      return new AuthorCondition(string.Empty) { CaseSensitive = caseSensitive };
    }
  }
}
