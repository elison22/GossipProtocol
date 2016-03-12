using Nancy.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using GossipProtocol.Writing;

namespace GossipProtocol.Gossip
{
    public class GossipStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            Write.WriteLine("Initializing the gossip loop in GossipStartup");
            GossipLoop.Init();
        }
    }
}