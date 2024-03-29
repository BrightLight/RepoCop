﻿// --------------------------------------------------------------------------------------------------------------------
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

  /// <summary>
  /// Represents the entry point of the application.
  /// </summary>
  class Program
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// The name of the command line parameter that specifies the path to the SVN binary folder.
    /// </summary>
    private const string SvnBinParamKey = "-svnbin";

    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    static void Main(string[] args)
    {
      if (!InitializeLog4Net())
      {
        Environment.ExitCode = 1;
        return;
      }

      try
      {
        log.DebugFormat("{0} started", AppDomain.CurrentDomain.FriendlyName);
        if (args.Length > 2)
        {
          log.DebugFormat("Argument0: [{0}] - Argument1: [{1}] - Argument2: [{2}]", args[0], args[1], args[2]);
        }

        var actionArgument = args.Length > 0 ? args[0] : string.Empty;

        if (!ProcessParameters(args))
        {
          Environment.ExitCode = 1;
          return;
        }

        var configurationFile = HookManager.FindHookConfigurationFile();
        if (!File.Exists(configurationFile))
        {
          Console.Error.WriteLine($"Configuration file was not found ({configurationFile}).");
          Environment.ExitCode = 4;
          return;
        }

        var instructions = HookManager.ReadHookConfiguration(configurationFile);
        IRepoChangeInfo repoChangeInfo = null;
        switch (actionArgument)
        {
          case "validate":
            if (HookManager.Validate(instructions, out var text))
            {
              Console.Out.WriteLine("Configuration file was successfully parsed.");
              Console.Out.WriteLine(text);
              Environment.ExitCode = 0;
            }
            else
            {
              Console.Out.WriteLine("Validation encountered an error. Please check log for details.");
              Environment.ExitCode = 1;
            }

            break;
          case "start-commit":
            if (args.Length > 3)
            {
              var repositoryPath = args[1];
              var username = args[2];
              var capabilities = args[3];
              log.Debug("start-commit hook using these settings:");
              log.DebugFormat("Repository path: [{0}]", repositoryPath);
              log.DebugFormat("Username: [{0}]", username);
              log.DebugFormat("Client capabilities: [{0}]", capabilities);
              
              repoChangeInfo = new SvnLookRepoChangeInfo(HookType.StartCommit, username, string.Empty, -1, DateTime.MinValue, null, capabilities.Split());
            }
            else
            {
              log.Error("Insufficient parameters for start-commit.");
            }

            break;
          case "pre-commit":
            if (args.Length > 2)
            {
              var repositoryPath = args[1];
              var transaction = args[2];
              repoChangeInfo = SvnLook.Transaction(HookType.PreCommit, repositoryPath, transaction);
            }
            else
            {
              log.Error("Insufficient parameters for start-commit.");
            }

            break;
          case "post-commit":
            if (args.Length > 2)
            {
              var repositoryPath = args[1];
              if (long.TryParse(args[2], out var revision))
              {
                repoChangeInfo = SvnLook.Revision(HookType.PostCommit, repositoryPath, revision);
              }
              else
              {
                Console.Error.WriteLine("Could not parse {0} as revision number.", args[2]);
                Environment.ExitCode = 2;
              }
            }
            else
            {
              log.Error("Insufficient parameters for start-commit.");
            }

            break;
          default:
            Console.Error.WriteLine("Unsupported hook type. Supported types are \"start-commit\", \"pre-commit\" and \"post-commit\".");
            break;
        }

        if (repoChangeInfo != null
            && !HookManager.Execute(instructions, repoChangeInfo))
        {
          Environment.ExitCode = 1;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occured: {0}", e);
        log.Error("An error occured", e);
      }
    }

    /// <summary>
    /// Processes the command line parameters that were passed to the application.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    /// <returns><c>true</c> if the parameters were processed successfully; otherwise <c>false</c>.</returns></returns>
    private static bool ProcessParameters(params string[] args)
    {
      const int CustomParamStartIndex = 3;

      if (args.Length < CustomParamStartIndex + 1)
      {
        return true;
      }

      for (var i = CustomParamStartIndex; i < args.Length; i++)
      {
        // get custom SVN binary folder from command line
        if (args[i].StartsWith(SvnBinParamKey, StringComparison.InvariantCultureIgnoreCase) && i + 1 < args.Length)
        {
          // the param indicates that the next argument contains the actual value
          var svnbin = args[i + 1];
          if (!Directory.Exists(svnbin))
          {
            Environment.ExitCode = 1;
            log.Error($"The specified svn binary folder '{svnbin}' doesn't exist!");
            return false;
          }

          SvnLook.SetSvnBinFolder(svnbin);
        }
      }

      return true;
    }

    /// <summary>
    /// Initialize log4net.
    /// </summary>
    private static bool InitializeLog4Net()
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
        return true;
      }
      else
      {
        Console.WriteLine($"Log4net configuration not found (expected: {log4netConfigFile.Name})");
        return false;
      }
    }
  }
}
