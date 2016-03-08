using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GossipProtocol.Gossip.Message
{
    public class RumorMessage : IComparable<RumorMessage>
    {
        public RumorMessage() { }
        public RumorMessage(string messageId, string originator, string text, string endpoint)
        {
            EndPoint = endpoint;
            Rumor = new OneRumor
            {
                MessageId = messageId,
                Originator = originator,
                Text = text
            };
        }

        // For Json to be happy
        public OneRumor Rumor { get; set; }
        public string EndPoint { get; set; }

        // Indirectly for Json as well
        public class OneRumor
        {
            public MessageId fullId = new MessageId();

            //For Json to be happy
            public string MessageId
            {
                get
                {
                    return fullId.id;
                }
                set
                {
                    fullId.id = value;
                }
            }
            public string Originator { get; set; }
            public string Text { get; set; }
        }
        
        public string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"Rumor\" : {\"MessageID\": \"");
            builder.Append(Rumor.MessageId);
            builder.Append("\" ,\n\"Originator\": \"");
            builder.Append(Rumor.Originator);
            builder.Append("\",\n\"Text\": \"");
            builder.Append(Rumor.Text);
            builder.Append("\"\n},\n\"EndPoint\": \"");
            builder.Append(EndPoint);
            builder.Append("\"\n}");

            return builder.ToString();
        }

        public int CompareTo(RumorMessage other)
        {
            if (EndPoint == other.EndPoint &&
                Rumor.MessageId == other.Rumor.MessageId &&
                Rumor.Originator == other.Rumor.Originator &&
                Rumor.Text == other.Rumor.Text)
                return 0;
            else return -1;
        }
    }

    /*
        {"Rumor" : {"MessageID": "ABCD-1234-ABCD-1234-ABCD-1234:5" ,            a string containing the unique ID for this message as described above
                    "Originator": "Phil",                                       a string giving the name of the server (or user)
                    "Text": "Hello World!"                                      a string containing the actual message
                    },
         "EndPoint": "https://example.com/gossip/13244"                         URL of the node propagating the rumor
        }   
    */
}