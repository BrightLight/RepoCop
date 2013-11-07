// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineInstruction.cs" company="Silverseed.de">
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
  using System.Diagnostics;
  using System.IO;
  using log4net;

  internal class CommandLineInstruction : Instruction
  {
    /// <summary>
    /// A logger used by instances of this class.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineInstruction"/> class.
    /// </summary>
    public CommandLineInstruction()
    {
      this.TimeoutInMilliseconds = 60000; // 60s
      this.ExpectedExitCode = 0;
      this.NewLineReplacement = " ";
      this.StandardOutput = Console.Out;
      this.ErrorOutput = Console.Error;
    }

    public CommandLineInstruction(TextWriter standardOutput, TextWriter errorOutput)
      : this()
    {
      if (standardOutput != null)
      {
        this.StandardOutput = standardOutput;
      }

      if (errorOutput != null)
      {
        this.ErrorOutput = errorOutput;
      }
    }

    protected override bool InternalExecute()
    {
      ProcessStartInfo processStartInfo = new ProcessStartInfo();
      processStartInfo.ErrorDialog = false;
      processStartInfo.CreateNoWindow = true;
      processStartInfo.UseShellExecute = false;
      processStartInfo.RedirectStandardOutput = true;
      processStartInfo.RedirectStandardError = true;
      processStartInfo.FileName = RepositoryInfoHub.Instance.ParseTokens(this.FileName);
      processStartInfo.Arguments = RepositoryInfoHub.Instance.ParseTokens(this.Arguments).Replace(Environment.NewLine, this.NewLineReplacement);

      var process = new Process();
      process.StartInfo = processStartInfo;
      process.OutputDataReceived += this.process_OutputDataReceived;
      process.ErrorDataReceived += this.process_ErrorDataReceived;
      try
      {
        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // MSDN: When standard output has been redirected to asynchronous event handlers, it is possible that output processing will not have 
        // completed when this method returns. To ensure that asynchronous event handling has been completed, call the WaitForExit()
        // overload that takes no parameter after receiving a true from this overload.
        if (process.WaitForExit(this.TimeoutInMilliseconds))
        {
          process.WaitForExit();
          return process.ExitCode == this.ExpectedExitCode;
        }
      }
      finally
      {
        process.Close();
      }

      log.WarnFormat("Process took longer than the allotted {0} ms. Process was killed.", this.TimeoutInMilliseconds);
      return false;
    }

    private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      this.StandardOutput.Write(e.Data);
    }

    private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      this.ErrorOutput.Write(e.Data);
    }

    public int TimeoutInMilliseconds { get; set; }

    public string FileName { get; set; }

    public string Arguments { get; set; }

    public int ExpectedExitCode { get; set; }

    public string NewLineReplacement { get; set; }

    private TextWriter StandardOutput { get; set; }

    private TextWriter ErrorOutput { get; set; }
  }
}
