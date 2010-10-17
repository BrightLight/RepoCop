// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Silverseed.de">
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
  using System.IO;
  using log4net;
  using SharpSvn;

  class Program
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    static void Main(string[] args)
    {
      InitializeLog4Net();
      try
      {
        ////args = new string[] { "pre", @"C:\temp\DummyRepo\", "kilgkfhb" };
        ////args = new string[] { "post-commit", @"C:\temp\repositories\dummy", "35" };
        log.ErrorFormat("{0} started", AppDomain.CurrentDomain.FriendlyName);
        if (args.Length > 2)
        {
          log.DebugFormat("Argument0: [{0}] - Argument1: [{1}] - Argument2: [{2}]", args[0], args[1], args[2]);
        }

        string hookTypeArgument = null;
        if (args.Length > 0)
        {
          hookTypeArgument = args[0];
        }

        IRepoChangeInfo repoChangeInfo = null;
        string repositoryPath;
        switch (hookTypeArgument)
        {
          case "start-commit":
            repositoryPath = args[1];
            var username = args[2];
            var capabilities = args[3];
            log.Debug("start-commit hook using these settings:");
            log.DebugFormat("Repository path: [{0}]", repositoryPath);
            log.DebugFormat("Username: [{0}]", username);
            log.DebugFormat("Client capabilities: [{0}]", capabilities);
            break;
          case "pre-commit":
            repositoryPath = args[1];
            var transaction = args[2];
            repoChangeInfo = GetPreCommitRepoChangeInfo(repositoryPath, transaction);
            
            break;
          case "post-commit":
            repositoryPath = args[1];
            long revision;
            if (Int64.TryParse(args[2], out revision))
            {
              repoChangeInfo = GetPostCommitRepoChangeInfo(repositoryPath, revision);
            }
            else
            {
              System.Console.Error.WriteLine("Could not parse {0} as revision number.", args[2]);
              Environment.ExitCode = 2;
            }

            break;
          default:
            System.Console.Error.WriteLine("Unsupported hook type. Use either pre oder post as first argument.");
            break;
        }

        if (repoChangeInfo != null)
        {
          if (!HookManager.Execute(repoChangeInfo))
          {
            Environment.ExitCode = 1;
          }
        }
      }
      catch(Exception e)
      {
        Console.WriteLine("Es ist ein Fehler aufgetreten: {0}", e);
        log.Error("Es ist ein Fehler aufgetreten", e);
      }
    }

    /// <summary>
    /// Initialize log4ne.
    /// </summary>
    private static void InitializeLog4Net()
    {
      var executableName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
      if (executableName.Contains(".vshost."))
      {
        executableName = executableName.Replace(".vshost.", ".");
      }

      var log4netConfigFile = new FileInfo(executableName + ".log4net.xml");
      if (log4netConfigFile.Exists)
      {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(log4netConfigFile);
      }
      else
      {
        log4net.Config.XmlConfigurator.Configure();
      }
    }

    private static IRepoChangeInfo GetPreCommitRepoChangeInfo(string repositoryPath, string transactionName)
    {
      // SvnInfo ermitteln
      var svnLookClient = new SvnLookClient();
      var svnLookOrigin = new SvnLookOrigin(repositoryPath, transactionName);
      SvnChangeInfoEventArgs svnChangeInfoEventArgs;
      if (svnLookClient.GetChangeInfo(svnLookOrigin, out svnChangeInfoEventArgs))
      {
        return new SvnChangeInfoEventArgsWrapper(HookType.PreCommit, svnChangeInfoEventArgs);
      }

      return null;
    }

    private static IRepoChangeInfo GetPostCommitRepoChangeInfo(string repositoryPath, long revision)
    {
      // SvnInfo ermitteln
      var svnLookClient = new SvnLookClient();
      var svnLookOrigin = new SvnLookOrigin(repositoryPath, revision);
      SvnChangeInfoEventArgs svnChangeInfoEventArgs;
      if (svnLookClient.GetChangeInfo(svnLookOrigin, out svnChangeInfoEventArgs))
      {
        return new SvnChangeInfoEventArgsWrapper(HookType.PostCommit, svnChangeInfoEventArgs);
      }

      return null;
    }
  }
}
