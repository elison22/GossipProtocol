using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Gossip
{
    public class Peer : IComparable<Peer>
    {
        public string Endpoint { get; set; }
        public string PeerName { get; set; }

        public int CompareTo(Peer other)
        {
            return Endpoint.CompareTo(other.Endpoint);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Peer))
                return false;
            return Endpoint == ((Peer)obj).Endpoint;
        }

        public override int GetHashCode()
        {
            return Endpoint.GetHashCode();
        }
    }
}