using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Gossip
{
    public class Peer
    {
        public Guid OriginId { get; set; }
        public string Endpoint { get; set; }
    }
}