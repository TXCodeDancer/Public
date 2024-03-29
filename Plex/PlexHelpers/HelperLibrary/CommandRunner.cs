﻿using System.Text;

namespace HelperLibrary;

public class CommandRunner
{
    public static async Task<string> RunCommandAsync(string filename, string? arguments = null)
    {
        var process = new System.Diagnostics.Process();

        process.StartInfo.FileName = filename;
        if (!string.IsNullOrEmpty(arguments))
        {
            process.StartInfo.Arguments = arguments;
        }

        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        var stdOutput = new StringBuilder();
        process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

        string? stdError = null;
        try
        {
            process.Start();
            process.BeginOutputReadLine();
            stdError = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
        }
        catch (Exception e)
        {
            throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
        }

        if (process.ExitCode == 0)
        {
            return stdOutput.ToString();
        }
        else
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(stdError))
            {
                message.AppendLine(stdError);
            }

            if (stdOutput.Length != 0)
            {
                message.AppendLine("Std output:");
                message.AppendLine(stdOutput.ToString());
            }

            throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
        }
    }

    private static string Format(string filename, string? arguments)
    {
        return "'" + filename +
            ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
            "'";
    }
}
