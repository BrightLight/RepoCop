// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepoChangeInfo.cs" company="Silverseed.de">
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
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Represents the information about a change in a repository.
  /// </summary>
  public interface IRepoChangeInfo
  {
    /// <summary>
    /// Gets the type of the hook for which RepoCop was called.
    /// </summary>
    HookType HookType { get; }

    /// <summary>
    /// Gets the author of the change.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Gets the log message of the change.
    /// </summary>
    string LogMessage { get; }

    /// <summary>
    /// Gets the revision of the change.
    /// </summary>
    long Revision { get; }

    /// <summary>
    /// Gets the timestamp of the change.
    /// </summary>
    DateTime Time { get; }

    /// <summary>
    /// Gets all affected items of the change.
    /// </summary>
    IReadOnlyCollection<IRepoAffectedItem> AffectedItems { get; }

    /// <summary>
    /// Gets the capabilities of the client that connects to the SCM server. 
    /// </summary>
    IReadOnlyCollection<string> ClientCapabilities { get; }
  }
}
