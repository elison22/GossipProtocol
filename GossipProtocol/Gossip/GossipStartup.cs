using Nancy.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace GossipProtocol.Gossip
{
    public class GossipStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            GossipLoop.init();
        }
    }
}