// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Silverseed.de">
//    (c) 2018 Steffen Wilke, Markus Hastreiter @ Silverseed.de
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

namespace Silverseed.RepoCop.Subversion
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Represents the information about a change in a Subversion repository.
  /// </summary>
  public class SvnLookRepoChangeInfo : IRepoChangeInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SvnLookRepoChangeInfo"/> class.
    /// </summary>
    /// <param name="hookType">The type of the hook for which RepoCop was called.</param>
    /// <param name="author">The author of the change.</param>
    /// <param name="logMessage">The log message of the change.</param>
    /// <param name="revision">The revision of the change.</param>
    /// <param name="time">The timestamp of the change.</param>
    /// <param name="affectedItems">All affected items of the change.</param>
    /// <param name="capabilities">The capabilities of the client that connects to the SCM server.</param>
    public SvnLookRepoChangeInfo(HookType hookType, string author, string logMessage, long revision, DateTime time, IReadOnlyCollection<IRepoAffectedItem> affectedItems, IReadOnlyCollection<string> capabilities)
    {
      this.HookType = hookType;
      this.Author = author.Trim();
      this.LogMessage = logMessage.Trim();
      this.Revision = revision;
      this.Time = time;
      this.AffectedItems = affectedItems;
      this.ClientCapabilities = capabilities;
    }

    /// <inheritdoc />
    public HookType HookType { get; }

    /// <inheritdoc />
    public string Author { get; }

    /// <inheritdoc />
    public string LogMessage { get; }

    /// <inheritdoc />
    public long Revision { get; }

    /// <inheritdoc />
    public DateTime Time { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IRepoAffectedItem> AffectedItems { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> ClientCapabilities { get; }
  }
}