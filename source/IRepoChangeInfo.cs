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
  using System.Linq;
  using System.Text;

  public interface IRepoChangeInfo
  {
    HookType HookType { get; }
    string Author { get; }
    string LogMessage { get; }
    long Revision { get; }
    DateTime Time { get; }
    ICollection<IRepoAffectedItem> AffectedItems { get; }
  }

  public interface IRepoAffectedItem
  {
    RepositoryItemAction Action { get; }
    RepositoryItemNodeKind NodeKind { get; }
    string Path { get; }
  }

  public enum RepositoryItemAction
  {
    None,

    Add,

    Delete,

    Modifiy,

    Replace
  }

  public enum RepositoryItemNodeKind
  {
    Unknown,

    File,

    Directory
  }
}
