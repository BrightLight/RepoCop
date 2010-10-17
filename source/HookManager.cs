// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HookManager.cs" company="Silverseed.de">
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
  using System.IO;
  using log4net;
  using Silverseed.ComponentModel.Xml;
  using Silverseed.RepoCop.Xml;

  public enum HookType
  {
    Undefined,

    StartCommit,

    PreCommit,

    PostCommit
  }

  public static class HookManager
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static bool Execute(IRepoChangeInfo repoChangeInfo)
    {
      var instructions = BuildInstructions();
      SubversionInfoHub.Instance.RepoChangeInfo = repoChangeInfo;
      return instructions.Execute();
    }

    private static Instruction BuildInstructions()
    {
      var configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
      if (configurationFile.Contains(".vshost."))
      {
        configurationFile = configurationFile.Replace(".vshost.", ".");
      }

      configurationFile = configurationFile + ".HookConfig.xml";
      log.DebugFormat("Looking for configuration file {0}", configurationFile);
      if (File.Exists(configurationFile))
      {
        log.Debug("Configuration file found.");
        using (var configXmlStream = new FileStream(configurationFile, FileMode.Open, FileAccess.Read))
        {
          var xmlHub = new XmlHub(new HookConfigServiceLocator());

          var instructions = new MacroInstruction();
          ObjectFactory.Instance.ObjectStack.Push(instructions);
          xmlHub.Process(configXmlStream);

          if (ObjectFactory.Instance.ObjectStack.Count == 0)
          {
            throw new NotSupportedException("Es wurden keine Instructions erzeugt.");
          }

          if (ObjectFactory.Instance.ObjectStack.Count > 1)
          {
            throw new NotSupportedException(
              "Bei der Erstellung der Anweisungen kam es zu einem Fehler. Der interne ObjectStack wurde nicht komplett geleert.");
          }

          return instructions;
          ////return ObjectFactory.Instance.ObjectStack.Peek() as Instruction;
        }
      }

      return new NullInstruction();
    }
  }
}
