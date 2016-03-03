using GossipProtocol.Gossip.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Gossip
{
    public class State
    {
        public State()
        {
            sentMessages = new Dictionary<string, MessageId>();
            receivedMessages = new List<RumorMessage>();
        }

        public Dictionary<string, MessageId> sentMessages { get; set; }
        public List<RumorMessage> receivedMessages { get; set; }

        public void addMessage(RumorMessage message)
        {
            if (receivedMessages.Contains(message))
                return;
            receivedMessages.Add(message);
        }

        public WantMessage getWant()
        {
            WantMessage want = new WantMessage();

            List<MessageId> receivedIDs = (
                from rm in receivedMessages
                select rm.Rumor.fullId).ToList();

            List<Guid> distinctOrigins = (
                from rid in receivedIDs
                select rid.origin).Distinct().ToList();

            foreach(Guid orig in distinctOrigins)
            {
                List<MessageId> groupedMessages = (
                    from rm in receivedMessages
                    where rm.Rumor.fullId.origin == orig
                    orderby rm.Rumor.fullId.sequence descending
                    select rm.Rumor.fullId).ToList();

                want.WantList.Add(groupedMessages[0]);
            }

            return want;
        }



    }
}