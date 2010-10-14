// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvnChangeInfoEventArgsWrapper.cs" company="Silverseed.de">
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

namespace Silverseed.SubversionHook.Console
{
  using System;
  using System.Collections.Generic;
  using SharpSvn;

  internal class SvnChangeInfoEventArgsWrapper : IRepoChangeInfo
  {
    public SvnChangeInfoEventArgsWrapper(HookType hookType, SvnChangeInfoEventArgs svnChangeInfoEventArgs)
    {
      this.HookType = hookType;
      this.BaseRevision = svnChangeInfoEventArgs.BaseRevision;
      this.Author = svnChangeInfoEventArgs.Author;
      this.LogMessage = svnChangeInfoEventArgs.LogMessage;
      this.Revision = svnChangeInfoEventArgs.Revision;
      this.Time = svnChangeInfoEventArgs.Time;
      this.AffectedItems = new List<IRepoAffectedItem>();
      foreach (SvnChangeItem item in svnChangeInfoEventArgs.ChangedPaths)
      {
        this.AffectedItems.Add(new SvnChangeItemWrapper(item));
      }
    }

    #region IRepoChangeInfo Members

    public HookType HookType { get; private set; }

    public long BaseRevision { get; private set; }

    public string Author { get; private set; }

    public string LogMessage { get; private set; }

    public long Revision { get; private set; }

    public DateTime Time { get; private set; }

    public ICollection<IRepoAffectedItem> AffectedItems { get; private set; }

    #endregion
  }
}
