using System;
using System.Collections.Generic;
using System.Linq;
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
                foreach (KeyValuePair<string, string> pair in value)
                {
                    WantList.Add(new MessageId(pair.Key, pair.Value));
                }
            }

        }
        public string EndPoint { get; set; }
        
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