using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore
{
    [Serializable()]
    public class BOARD
    {
        public string TYPE = null;
        public string VERS = null;
    }


    public enum BOARDS
    {
        CPU,
        ETH,
        EB,
        AC,
        AU
    }

}
