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
  /// <summary>
  /// Represents an item that was affected by a change in a repository.
  /// </summary>
  public class SvnLookRepoAffectedItem : IRepoAffectedItem
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SvnLookRepoAffectedItem"/> class.
    /// </summary>
    /// <param name="action">The action that was performed on the item.</param>
    /// <param name="nodeKind">The kind of the item (file or directory).</param>
    /// <param name="path">The path of the item within the repository.</param>
    public SvnLookRepoAffectedItem(RepositoryItemAction action, RepositoryItemNodeKind nodeKind, string path)
    {
      this.Action = action;
      this.NodeKind = nodeKind;
      this.Path = path;
    }

    /// <inheritdoc />
    public RepositoryItemAction Action { get; set; }

    /// <inheritdoc />
    public RepositoryItemNodeKind NodeKind { get; set; }

    /// <inheritdoc />
    public string Path { get; set; }
  }
}