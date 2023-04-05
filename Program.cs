using InstallerMTW.Processes;
using System;
using System.Diagnostics;


namespace InstallerMTW
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DialogManager runApp = new DialogManager();

            //runApp.StartTerminalDialog();

            //TestRedirectOutput();

            //Console.WriteLine("Press any key to exit.");

            //Console.ReadKey();

            runApp.terminal.OpenTerminal();
            Console.ReadKey();
        }

        private static void TestRedirectOutput()
        {
            Process process = new Process();

            process.StartInfo.FileName = "ipconfig.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            // Write the redirected output to this application's window.
            Console.WriteLine(output);

            process.WaitForExit();
        }
    }
}