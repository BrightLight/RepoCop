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
  using Silverseed.Core.Xml;
  using Silverseed.RepoCop.Xml;

  public static class HookManager
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static bool Validate(Instruction instructions, out string instructionsAsText)
    {
      try
      {
        instructionsAsText = instructions.ToString();
        return true;
      }
      catch (Exception exception)
      {
        log.Error("Validating encountered an error. Please check log for details.", exception);
        instructionsAsText = null;
        return false;
      }
    }

    public static bool Execute(Instruction instructions, IRepoChangeInfo repoChangeInfo)
    {
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo;
      return instructions.Execute();
    }

    public static string FindHookConfigurationFile()
    {
      var configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
      if (configurationFile.Contains(".vshost."))
      {
        configurationFile = configurationFile.Replace(".vshost.", ".");
      }

      return configurationFile + ".HookConfig.xml";
    }

    /// <summary>
    /// Builds the instructions according to the hook configuration file.
    /// </summary>
    /// <returns>A <see cref="MacroInstruction"/> containing all the defined instructions.</returns>
    public static Instruction ReadHookConfiguration(string configurationFile)
    {
      using var configXmlStream = new FileStream(configurationFile, FileMode.Open, FileAccess.Read);
      return ReadHookConfiguration(configXmlStream);
    }

    public static Instruction ReadHookConfiguration(Stream configXmlStream)
    {
      var xmlHub = new XmlHub(new HookConfigServiceLocator());

      var instructions = new MacroInstruction();
      ObjectFactory.Instance.ObjectStack.Push(instructions);
      xmlHub.Process(configXmlStream);

      if (ObjectFactory.Instance.ObjectStack.Count != 1)
      {
        throw new NotSupportedException("Something went wrong while parsing the configuration file.");
      }

      return instructions;
    }
  }
}
