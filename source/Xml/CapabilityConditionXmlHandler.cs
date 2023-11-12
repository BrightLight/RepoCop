// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CapabilityConditionXmlHandler.cs" company="Silverseed.de">
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
  using Silverseed.Core;

  /// <summary>
  /// This class is responsible for creating a <see cref="CapabilityCondition"/> from the XML configuration.
  /// </summary>
  internal class CapabilityConditionXmlHandler : ConditionXmlHandler<CapabilityCondition>
  {
    /// <summary>
    /// Creates a <see cref="CapabilityCondition"/> from the XML configuration.
    /// </summary>
    /// <param name="attributes">The attributes of the XML element.</param>
    /// <returns>A <see cref="CapabilityCondition"/> instance.</returns>
    protected override CapabilityCondition CreateCondition(Dictionary<string, string> attributes)
    {
      var capability = attributes.GetValueOrDefault("Capability", string.Empty);
      return new CapabilityCondition { Capability = capability };
    }
  }
}