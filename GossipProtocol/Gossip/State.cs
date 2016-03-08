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
            NewestSentMessages = new Dictionary<Peer, List<MessageId>>();
            ReceivedMessages = new List<RumorMessage>();
            rand = new Random();
            CurSequence = 0;
        }

        public Dictionary<Peer, List<MessageId>> NewestSentMessages { get; set; }     // endpoint to id
        public List<RumorMessage> ReceivedMessages { get; set; }
        public int CurSequence { get; set; }
        private Random rand;


        public void AddMessage(RumorMessage message)
        {
            if (ReceivedMessages.Contains(message))
                return;
            ReceivedMessages.Add(message);
        }

        public void AddMessage(Guid id, int sequence, string originator, string text, string endpoint)
        {
            string messageId = id.ToString() + ":" + sequence;
            RumorMessage newMessage = new RumorMessage(messageId, originator, text, endpoint);
            ReceivedMessages.Add(newMessage);
        }

        public void AddMyMessage(Guid id, string originator, string text, string endpoint)
        {
            AddMessage(id, CurSequence, originator, text, endpoint);
            CurSequence++;
        }

        
        public WantMessage GetWant()
        {
            WantMessage want = new WantMessage();

            List<MessageId> receivedIDs = (
                from rm in ReceivedMessages
                select rm.Rumor.fullId).ToList();

            List<Guid> distinctOrigins = (
                from rid in receivedIDs
                select rid.origin).Distinct().ToList();

            foreach(Guid orig in distinctOrigins)
            {
                List<MessageId> groupedMessages = (
                    from rm in ReceivedMessages
                    where rm.Rumor.fullId.origin == orig
                    orderby rm.Rumor.fullId.sequence descending
                    select rm.Rumor.fullId).ToList();

                want.WantList.Add(groupedMessages[0]);
            }

            return want;
        }

        public RumorMessage GetRandMessage(Peer peer)
        {
            List<MessageId> sentSoFarToEndpoint = NewestSentMessages[peer];

            List<Guid> sentSoFarOrigins = (from ssfte in sentSoFarToEndpoint select ssfte.origin).ToList();

            List<RumorMessage> unsentMessages = (
                from rm in ReceivedMessages
                where !sentSoFarToEndpoint.Contains(rm.Rumor.fullId)
                select rm).ToList();

            List<RumorMessage> newerReceived = (
                from um in unsentMessages
                where !sentSoFarOrigins.Contains(um.Rumor.fullId.origin) ||
                um.Rumor.fullId.sequence > (
                    from ssf in sentSoFarToEndpoint
                    where ssf.origin == um.Rumor.fullId.origin
                    orderby ssf.sequence descending
                    select ssf).FirstOrDefault().sequence
                select um).ToList();

            if (newerReceived.Count == 0)
                return null;
            
            RumorMessage toReturn = newerReceived[new Random().Next(newerReceived.Count)];
                        
            return toReturn;
        }

    }
}