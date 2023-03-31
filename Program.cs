using InstallerMTW.Processes;
using System.Diagnostics;

namespace InstallerMTW
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DialogManager runApp = new DialogManager();
            runApp.StartTerminalDialog();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}