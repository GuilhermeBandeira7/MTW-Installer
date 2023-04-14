using InstallerMTW.Processes;
using System.Diagnostics;

namespace InstallerMTW {
    class Program {
        static void Main(string[] args) {
            DialogManager dialogManager = new DialogManager();
            dialogManager.StartTerminalDialog();
        }
    }
}