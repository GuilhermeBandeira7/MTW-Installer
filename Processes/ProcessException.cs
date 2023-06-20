using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerMTW.Processes
{
  public class ProcessException : Exception
  {
    public ProcessException(string msg) : base(msg)
    {
    }
  }
}
