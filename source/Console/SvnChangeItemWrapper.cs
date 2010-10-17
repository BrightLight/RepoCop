// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvnChangeItemWrapper.cs" company="Silverseed.de">
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

namespace Silverseed.RepoCop.Subversion
{
  using System;
  using SharpSvn;

  public class SvnChangeItemWrapper : IRepoAffectedItem
  {
    public SvnChangeItemWrapper(SvnChangeItem svnChangeItem)
    {
      this.Action = SvnAction2ItemAction(svnChangeItem.Action);
      this.CopyFromPath = svnChangeItem.CopyFromPath;
      this.CopyFromRevision = svnChangeItem.CopyFromRevision;
      this.NodeKind = SvnNodeKind2ItemNodeKind(svnChangeItem.NodeKind);
      this.Path = svnChangeItem.Path;
      this.RepositoryPath = svnChangeItem.RepositoryPath;
    }

    #region IRepoAffectedItem Members

    public RepositoryItemAction Action { get; private set; }

    public string CopyFromPath { get; private set; }

    public long CopyFromRevision { get; private set; }

    public RepositoryItemNodeKind NodeKind { get; private set; }

    public string Path { get; private set; }

    public Uri RepositoryPath { get; private set; }

    #endregion

    private static RepositoryItemAction SvnAction2ItemAction(SvnChangeAction svnChangeAction)
    {
      switch (svnChangeAction)
      {
        case SvnChangeAction.Add:
          return RepositoryItemAction.Add;
        case SvnChangeAction.Delete:
          return RepositoryItemAction.Delete;
        case SvnChangeAction.Modify:
          return RepositoryItemAction.Modifiy;
        case SvnChangeAction.Replace:
          return RepositoryItemAction.Replace;
      }

      return RepositoryItemAction.None;
    }

    private static RepositoryItemNodeKind SvnNodeKind2ItemNodeKind(SvnNodeKind svnNodeKind)
    {
      switch (svnNodeKind)
      {
        case SvnNodeKind.File:
          return RepositoryItemNodeKind.File;
        case SvnNodeKind.Directory:
          return RepositoryItemNodeKind.Directory;
      }

      return RepositoryItemNodeKind.Unknown;
    }
  }
}
