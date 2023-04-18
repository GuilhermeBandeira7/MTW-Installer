using InstallerMTW.Processes;
using System.Diagnostics;
using System.Xml.Serialization;

namespace InstallerMTW {
    class Program {
        static void Main(string[] args) {
            DialogManager dialogManager = new DialogManager();
            dialogManager.StartTerminalDialog();

        }
    }
}