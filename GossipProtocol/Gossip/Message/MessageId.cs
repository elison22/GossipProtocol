﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Gossip.Message
{
    public class MessageId : IComparable<MessageId>
    {
        public MessageId(){}
        public MessageId(string origin, string sequence)
        {
            this.origin = origin;
            this.sequence = int.Parse(sequence);
        }

        public string origin { get; set; }
        public int sequence { get; set; }

        public string id
        {
            get
            {
                return origin.ToString() + ":" + sequence;
            }
            set
            {
                string[] splitVals = value.Split(new char[] { ':' });
                origin = splitVals[0];
                sequence = int.Parse(splitVals[1]);
            }
        }

        public int CompareTo(MessageId other)
        {
            return id.CompareTo(other.id);
        }
    }
}