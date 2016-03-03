using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Writing
{
    public class Write
    {
        public static void WriteLine(string toWrite)
        {
            System.Diagnostics.Trace.WriteLine(toWrite);
            Console.WriteLine(toWrite);
        }
    }
}