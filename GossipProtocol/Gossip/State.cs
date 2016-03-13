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
            SentMessages = new Dictionary<Peer, List<MessageId>>();
            ReceivedMessages = new List<RumorMessage>();
            Rand = new Random();
            NextSequence = 0;
        }

        public Dictionary<Peer, List<MessageId>> SentMessages { get; set; }     // endpoint to id
        public List<RumorMessage> ReceivedMessages { get; set; }
        public int NextSequence { get; set; }
        private Random Rand;


        public void AddMessage(RumorMessage message)
        {
            if (ReceivedMessages.Contains(message))
                return;
            message.ReceivedStamp = DateTime.Now;
            ReceivedMessages.Add(message);
        }

        public void AddMessage(Guid id, int sequence, string originator, string text, string endpoint)
        {
            string messageId = id.ToString() + ":" + sequence;
            RumorMessage newMessage = new RumorMessage(messageId, originator, text, endpoint);
            newMessage.ReceivedStamp = DateTime.Now;
            ReceivedMessages.Add(newMessage);
        }

        public void AddMyMessage(Guid id, string originator, string text, string endpoint)
        {
            AddMessage(id, NextSequence, originator, text, endpoint);
            NextSequence++;
        }

        public void AddSentTo(Peer peer, RumorMessage message)
        {
            SentMessages[peer].Add(message.Rumor.FullId);
        }

        public void AddSentTo(string peer, RumorMessage message)
        {
            SentMessages[new Peer { Endpoint = peer }].Add(message.Rumor.FullId);
        }

        
        public WantMessage GetWant()
        {
            WantMessage want = new WantMessage();

            List<MessageId> receivedIDs = (
                from rm in ReceivedMessages
                select rm.Rumor.FullId).ToList();

            List<string> distinctOrigins = (
                from rid in receivedIDs
                select rid.origin).Distinct().ToList();

            foreach(string orig in distinctOrigins)
            {
                List<MessageId> groupedMessages = (
                    from rm in ReceivedMessages
                    where rm.Rumor.FullId.origin == orig
                    orderby rm.Rumor.FullId.sequence descending
                    select rm.Rumor.FullId).ToList();

                want.AddWant(groupedMessages[0]);
            }

            return want;
        }

        public RumorMessage GetRandMessage(Peer peer)
        {
            List<MessageId> sentSoFarToEndpoint = SentMessages[peer];

            List<string> sentSoFarOrigins = (from ssfte in sentSoFarToEndpoint select ssfte.origin).ToList();

            List<RumorMessage> unsentMessages = (
                from rm in ReceivedMessages
                where !sentSoFarToEndpoint.Contains(rm.Rumor.FullId)
                select rm).ToList();

            List<IGrouping<string, RumorMessage>> newerReceived = (
                from um in unsentMessages
                where !sentSoFarOrigins.Contains(um.Rumor.FullId.origin) ||
                um.Rumor.FullId.sequence > (
                    from ssf in sentSoFarToEndpoint
                    where ssf.origin == um.Rumor.FullId.origin
                    orderby ssf.sequence descending
                    select ssf).FirstOrDefault().sequence
                group um by um.Rumor.Originator).ToList();
            

            if (newerReceived.Count == 0)
                return null;

            IGrouping<string, RumorMessage> oneGroup = newerReceived[Rand.Next(newerReceived.Count)];

            return (
                from r in oneGroup
                orderby r.Rumor.FullId.sequence ascending
                select r).FirstOrDefault();
        }

        public List<RumorMessage> GetMissingRumors(List<MessageId> fromWant)
        {
            List<string> fromWantOrigins = (
                from fw in fromWant
                select fw.origin).ToList();

            List<RumorMessage> missingRumors = (
                from rm in ReceivedMessages
                where !fromWantOrigins.Contains(rm.Rumor.FullId.origin) ||
                rm.Rumor.FullId.sequence > (
                    from fw in fromWant
                    where fw.origin == rm.Rumor.FullId.origin
                    select fw).FirstOrDefault().sequence
                select rm).ToList();

            return missingRumors;
        }

    }
}