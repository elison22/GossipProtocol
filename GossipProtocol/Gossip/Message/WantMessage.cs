using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GossipProtocol.Gossip.Message
{
    public class WantMessage
    {
        public List<MessageId> WantList { get; set; }

        // For JSon stuff to be happy.
        public Dictionary<string, string> Want
        {
            set
            {
                WantList = new List<MessageId>();
                foreach (KeyValuePair<string, string> pair in value)
                {
                    WantList.Add(new MessageId(pair.Key, pair.Value));
                }
            }

        }
        public string EndPoint { get; set; }
        
        public string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"Want\": {");
            bool contains1 = false;

            foreach(MessageId mi in WantList)
            {
                contains1 = true;
                builder.Append("\"");
                builder.Append(mi.origin.ToString());
                builder.Append("\": ");
                builder.Append(mi.sequence.ToString());
                builder.Append(",\n");
            }

            if(contains1)
                builder.Remove(builder.Length - 2, 2);
            builder.Append("\n} ,\n\"EndPoint\": \"");
            builder.Append(EndPoint);
            builder.Append("\"\n}");

            return builder.ToString();
        }

    }

    /*
        {"Want": {"ABCD-1234-ABCD-1234-ABCD-125A": 3,
                  "ABCD-1234-ABCD-1234-ABCD-129B": 5,           the keys are the origin IDs that the sender knows about. The value 
                  "ABCD-1234-ABCD-1234-ABCD-123C": 10           associated with each key is a numeric value of the highest sequence value from this OriginID that the sender has seen
                 } ,
         "EndPoint": "https://example.com/gossip/asff3"         URL of the node propagating the rumor; you’ll need this for part II below
        }
    
    */

}