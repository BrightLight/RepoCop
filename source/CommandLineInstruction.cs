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
      this.ExecutionMode = ExecutionMode.WaitForExit;
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

    protected ProcessStartInfo CreateProcessStartInfo()
    {
      var processStartInfo = new ProcessStartInfo();
      processStartInfo.FileName = RepositoryInfoHub.Instance.ParseTokens(this.FileName);
      processStartInfo.Arguments = RepositoryInfoHub.Instance.ParseTokens(this.Arguments).Replace(Environment.NewLine, this.NewLineReplacement);
      processStartInfo.ErrorDialog = false;

      log.DebugFormat("FileName (Unprocessed): [{0}]", this.FileName);
      log.DebugFormat("FileName (Processed): [{0}]", processStartInfo.FileName);
      log.DebugFormat("Arguments (Unprocessed): [{0}]", this.Arguments);
      log.DebugFormat("Arguments (Processed): [{0}]", processStartInfo.Arguments);

      return processStartInfo;
    }

    protected override bool InternalExecute()
    {
      switch (ExecutionMode)
      {
        case ExecutionMode.WaitForExit:
          return ExecuteWaitForExitInstruction();
        case ExecutionMode.FireAndForget:
          return ExecuteFireAndForgetInstruction();
        default:
          log.ErrorFormat("Could not process commandline instruction [{0}]!\nUnsupported execution mode [{1}]", this.FileName, this.ExecutionMode);
          return false;
      }
    }

    /// <summary>
    /// Executes the command line instruction and does not wait for the process to exit.
    /// </summary>
    /// <returns>Always true.</returns>
    /// <remarks>
    /// <see cref="ProcessStartInfo.CreateNoWindow"/> is ignored when <see cref="ProcessStartInfo.UseShellExecute"/> is set to true
    /// (source: <see href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.createnowindow"/>).
    /// </remarks>
    private bool ExecuteFireAndForgetInstruction()
    {
      var processStartInfo = this.CreateProcessStartInfo();
      processStartInfo.UseShellExecute = true;
      Process.Start(processStartInfo);
      return true;
    }

    /// <summary>
    /// Executes the command line instruction and waits for the process to exit.
    /// </summary>
    /// <returns>True if the process exited with the expected exit code, false otherwise.</returns>
    /// <remarks>
    /// For more information regarding UseShellExecute see
    /// <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-diagnostics-processstartinfo-useshellexecute"/>.
    /// </remarks>
    private bool ExecuteWaitForExitInstruction()
    {
      var processStartInfo = this.CreateProcessStartInfo();
      processStartInfo.CreateNoWindow = true;
      processStartInfo.UseShellExecute = false; // Required for redirecting output and error streams
      processStartInfo.RedirectStandardOutput = true;
      processStartInfo.RedirectStandardError = true;

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
          var wasSuccess = process.ExitCode == this.ExpectedExitCode;
          if (!wasSuccess)
          {
            var logMessage = $"Process exited with unexpected exit code [{process.ExitCode}]. Expected exit code was [{this.ExpectedExitCode}].\n"
              + $"Filename: [{processStartInfo.FileName}]\n"
              + $"Arguments: [{processStartInfo.Arguments}]";
            log.WarnFormat(logMessage);
          }

          return wasSuccess;
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
      log.Debug(e.Data);
      this.StandardOutput.Write(e.Data);
    }

    private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      log.Error(e.Data);
      this.ErrorOutput.Write(e.Data);
    }

    public int TimeoutInMilliseconds { get; set; }

    public string FileName { get; set; }

    public string Arguments { get; set; }

    public int ExpectedExitCode { get; set; }

    public ExecutionMode ExecutionMode { get; set; }

    /// <summary>
    /// A character or string which will replace <see cref="Environment.NewLine"/> in the executed command line.
    /// </summary>
    public string NewLineReplacement { get; set; }

    private TextWriter StandardOutput { get; }

    private TextWriter ErrorOutput { get; }
  }
}
