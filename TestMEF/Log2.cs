using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using TestMEFInterface;

namespace TestMEF
{
    [Export("TestMEF", typeof(ILog))]
    public class Log2:ILog
    {
        public string Prefix()
        {
            return "另一个实现ILog";
        }
    }
}
