using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerMTW.Processes
{
    public class ProcessSample : IDisposable
    {
        Process systemProcess;
        string browserPath;
        string url;

        public ProcessSample()
        {
            systemProcess = new Process();
            browserPath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
            url = "https://www.prppg.ufpr.br/siga/login";
        }

        public void Dispose()
        {
           systemProcess.Dispose();
        }

        // Opens the Internet Explorer application.
        public void OpenApplication()
        {
            // Start Internet Explorer. Defaults to the home page.
            Process.Start(browserPath);
        }

        public void OpenWithArguments()
        {
            // url's are not considered documents. They can only be opened
            // by passing them as arguments.
            Process.Start(browserPath, url);
        }

        public void OpenWithStartInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(browserPath);
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start(startInfo);

            startInfo.Arguments = url;

            Process.Start(startInfo);
        }
    }
}
