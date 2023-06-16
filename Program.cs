
using EntityMtwServer;
using InstallerMTW.Processes;

namespace InstallerMTW
{
  class Program
  {
    static void Main(string[] args)
    {
      MasterServerContext masterServerContext = new MasterServerContext();
      Console.WriteLine(masterServerContext.Users.Where(u => u.Id == 1).First().Name);
      Console.ReadLine();
      DialogManager dialogManager = new DialogManager();
      dialogManager.StartTerminalDialog();
    }
  }
}