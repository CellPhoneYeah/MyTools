using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using TestMEFInterface;

namespace TestMEF
{
    [Export(typeof(ILog))]
    public class Log:ILog
    {
        public string Prefix()
        {
            return "实现ILog";
        }
    }
}
