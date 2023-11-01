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
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using log4net;

  public static class SvnLook
  {
    private const string CmdAuthor = "author";
    private const string CmdLog = "log";
    private const string CmdDate = "date";
    private const string CmdChanged = "changed";
    private const string CmdArgCopyInfo = " --copy-info";

    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private static string svnBinFolder;

    private static bool CustomSvnBinFolder => !string.IsNullOrWhiteSpace(svnBinFolder);

    public static void SetSvnBinFolder(string folder)
    {
      svnBinFolder = folder;
    }

    public static IRepoChangeInfo Revision(HookType hookType, string repository, long revision)
    {
      if (!TryToGetCommandOutput(RevCommand(repository, revision, CmdAuthor), out var author))
      {
        return null;
      }

      if (!TryToGetCommandOutput(RevCommand(repository, revision, CmdLog), out var svnlog))
      {
        return null;
      }

      if (!TryToGetCommandOutput(RevCommand(repository, revision, CmdDate), out var date))
      {
        return null;
      }

      date = ParseDateTimeOffsetString(date).Trim();
      var offset = DateTimeOffset.Parse(date);

      var items = GetAffectedItems(repository, revision);
      if (items.Count == 0)
      {
        return null;
      }

      return new SvnLookRepoChangeInfo(hookType, author.Trim(), svnlog.Trim(), revision, offset.DateTime, items);
    }

    public static IRepoChangeInfo Transaction(HookType hookType, string repository, string transaction)
    {
      if (!TryToGetCommandOutput(RevTransaction(repository, transaction, CmdAuthor), out var author))
      {
        return null;
      }

      if (!TryToGetCommandOutput(RevTransaction(repository, transaction, CmdLog), out var svnlog))
      {
        return null;
      }

      if (!TryToGetCommandOutput(RevTransaction(repository, transaction, CmdDate), out var date))
      {
        return null;
      }

      date = ParseDateTimeOffsetString(date).Trim();
      var offset = DateTimeOffset.Parse(date);

      var items = GetAffectedItems(repository, transaction);
      if (items.Count == 0)
      {
        return null;
      }

      return new SvnLookRepoChangeInfo(hookType, author.Trim(), svnlog.Trim(), -1, offset.DateTime, items);
    }

    private static ICollection<IRepoAffectedItem> GetAffectedItems(string repository, long rev)
    {
      return !TryToGetCommandOutput(RevCommand(repository, rev, CmdChanged, CmdArgCopyInfo), out var changed) ? new List<IRepoAffectedItem>() : ParseAffectedItems(changed);
    }

    private static ICollection<IRepoAffectedItem> GetAffectedItems(string repository, string transaction)
    {
      return !TryToGetCommandOutput(RevTransaction(repository, transaction, CmdChanged, CmdArgCopyInfo), out var changed) ? new List<IRepoAffectedItem>() : ParseAffectedItems(changed);
    }

    internal static ICollection<IRepoAffectedItem> ParseAffectedItems(string changed)
    {
      const int ActionLength = 2;


      const int CopyInfoIndicatorLength = 1;
      const int MinLineLength = ActionLength + CopyInfoIndicatorLength;

      var items = new List<IRepoAffectedItem>();
      var lines = changed.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

      var copySourceItems = new List<SvnLookRepoAffectedItem>();
      foreach (var line in lines)
      {
        if (string.IsNullOrWhiteSpace(line) || line.Length < MinLineLength)
        {
          continue;
        }

        var path = line.Substring(MinLineLength).Trim();
        if (string.IsNullOrWhiteSpace(path))
        {
          continue;
        }

        var nodeKind = EvaluateNodeKind(path);
        var action = ParseAction(line.Substring(0, ActionLength));
        if (action == RepositoryItemAction.None)
        {
          var parsedPath = ParseReplacedPath(path);
          if (string.IsNullOrWhiteSpace(parsedPath))
          {
            continue;
          }

          copySourceItems.Add(new SvnLookRepoAffectedItem(action, nodeKind, $"/{parsedPath}"));
          continue;
        }

        // apparently the behavior of SharpSVN was to return paths with a starting '/'.
        // to be backward compatible to older configurations, we just append it is well, so the behavior stays the same
        items.Add(new SvnLookRepoAffectedItem(action, nodeKind, $"/{path}"));
      }

      foreach(var copySource in copySourceItems)
      {
        // lines that indicate a replace operation contain a "+"
        // e.g. A + trunk/vendors/baker/toast.txt
        //          (from trunk/vendors/baker/bread.txt:r63)
        //
        // there needs to be a counterpart with a delete action for a copysource item to consider it as replaced
        var alreadyInList = items.FirstOrDefault(x => x.Path == copySource.Path);
        if (alreadyInList != null)
        {
          copySource.Action = RepositoryItemAction.Replace;
          items.Remove(alreadyInList);
          items.Add(copySource);
        }
        else
        {
          // otherwise just track the item as copy source with an action type of "None"
          items.Add(copySource);
        }
      }

      return items.Distinct().ToList();
    }

    internal static RepositoryItemNodeKind EvaluateNodeKind(string path)
    {
      // if has trailing slash then it's a directory
      return new[] { "\\", "/" }.Any(path.EndsWith) ? RepositoryItemNodeKind.Directory : RepositoryItemNodeKind.File;
    }

    internal static string ParseDateTimeOffsetString(string svnDateString)
    {
      // this processes a string like "2018-08-01 19:21:10 +0200 (Mi, 01 Aug 2018)"
      return RegexParse(svnDateString, @"(.*)\s*\(.*\)");
    }

    internal static string ParseReplacedPath(string path)
    {
      // this processes a string like "     (from trunk/vendors/baker/bread.txt:r123)"
      return RegexParse(path, @"[\(]\w*\s(.*):r\d*[\)]");
    }

    internal static RepositoryItemAction ParseAction(string action)
    {
      // see http://svnbook.red-bean.com/de/1.5/svn.ref.svnlook.c.changed.html
      const string Add = "A ";
      const string Delete = "D ";
      const string FileUpdate = "U ";
      const string PropertyUpdate = "_U";
      const string FileAndPropertyUpdate = "UU";

      switch (action)
      {
        case Add:
          return RepositoryItemAction.Add;
        case Delete:
          return RepositoryItemAction.Delete;
        case FileUpdate:
        case PropertyUpdate:
        case FileAndPropertyUpdate:
          return RepositoryItemAction.Modify;
        default:
          return RepositoryItemAction.None;
      }
    }

    private static string RegexParse(string input, string pattern)
    {
      var match = Regex.Match(input, pattern);
      if (!match.Success || match.Groups.Count == 0)
      {
        return null;
      }

      return match.Groups[1].Value;
    }

    private static string RevCommand(string repository, long revision, string subCommand, string args = "")
    {
      return $"{subCommand} {repository} --revision {revision}{args}";
    }

    private static string RevTransaction(string repository, string transaction, string subCommand, string args = "")
    {
      return $"{subCommand} {repository} --transaction {transaction}{args}";
    }


    private static bool TryToGetCommandOutput(string command, out string output)
    {
      try
      {
        var process = new Process();
        // Redirect the output stream of the child process.
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        // force to use default encoding since svnlook always uses the system's default encoding for its output
        process.StartInfo.StandardOutputEncoding = Encoding.Default;
        process.StartInfo.StandardErrorEncoding = Encoding.Default;
        if (CustomSvnBinFolder)
        {
          process.StartInfo.FileName = Path.Combine(svnBinFolder, "svnlook.exe");
          process.StartInfo.Arguments = $" {command}";
        }
        else
        {
          process.StartInfo.FileName = "cmd.exe";
          process.StartInfo.Arguments = $"/C svnlook {command}";
        }

        process.Start();

        output = process.StandardOutput.ReadToEnd();
        process.WaitForExit(10000);

        if (process.ExitCode == 0)
        {
          return true;
        }

        var error = process.StandardError.ReadToEnd();
        log.Error($"Error while calling svnlook command with process '{process.StartInfo.FileName}' and arguments '{process.StartInfo.Arguments}'.{Environment.NewLine}{error}");
        return false;
      }
      catch (Exception e)
      {
        output = null;

        log.Error(e.Message, e);
        return false;
      }
    }
  }
}