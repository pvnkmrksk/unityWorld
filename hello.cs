using System;
using System.IO;
using System.Diagnostics;

class StandardOutputExample
{
    public static void Main()
    {

        System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo("rosnode", "list");
        procInfo.UseShellExecute = false;
        procInfo.RedirectStandardOutput = true;

        System.Diagnostics.Process list_cmd = System.Diagnostics.Process.Start(procInfo); // Start that process
        StreamReader reader = list_cmd.StandardOutput;
        string list_output = reader.ReadToEnd();

        foreach (string outs in list_output.Split("\n"))
            if (outs.StartsWith("/record"))
            {
                Console.WriteLine(outs);
            }
    }
}
