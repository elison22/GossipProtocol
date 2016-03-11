using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GossipProtocol.Gossip.Message
{
    public interface IGossipMessage
    {
        string ToJson();
    }
}
