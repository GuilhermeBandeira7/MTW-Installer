using CliWrap;
using CliWrap.Buffered;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerMTW.Processes
{
    public static class TerminalManager
    {
        public static async Task OpenTerminal()
        {
            var result = await Cli.Wrap("C:\\Users\\MTW\\Desktop\\IntallerMTW\\commands\\commands.bat")
                .WithWorkingDirectory("C:\\Users\\MTW\\Desktop\\IntallerMTW\\commands")
                //.WithArguments(new[] { "--version" })
                .ExecuteBufferedAsync();
            Console.WriteLine(result.StandardOutput);
        }

        public static async Task OpenLinuxTerminal()
        {
            var result = await Cli.Wrap(@"/home/mwt/MTWInstaller/commands/terminal.sh")
                .WithWorkingDirectory(@"/home/mwt/MTWInstaller/commands/")
                .ExecuteBufferedAsync();
            Console.WriteLine(result.StandardOutput);
        }
    }
}
