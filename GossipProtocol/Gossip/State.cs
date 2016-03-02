using GossipProtocol.Gossip.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Gossip
{
    public class State
    {
        public Dictionary<string, MessageId> sentMessages = new Dictionary<string, MessageId>();
        public List<RumorMessage> receivedMessages = new List<RumorMessage>();

        public void addMessage(RumorMessage message)
        {

        }

    }
}