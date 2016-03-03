using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using GossipProtocol.Writing;
using Newtonsoft.Json.Linq;
using GossipProtocol.Gossip.Message;

namespace GossipProtocol
{
    public class TestMain
    {
        public static void Main(string[] args)
        {
            string jsonString = "{\"Rumor\" : {" +
                                    "\"MessageID\": \"" + Guid.NewGuid().ToString() + ":4\"," +
                                    "\"Originator\": \"Phil\" ," +
                                    "\"Text\": \"Hello World!\"" +
                                "}," +
                                "\"EndPoint\": \"https://example.com/gossip/13244\"}";
            
            //Write.WriteLine(jsonString);
            RumorMessage rm = JsonConvert.DeserializeObject<RumorMessage>(jsonString);
            //Write.WriteLine(rm.ToJson());

            jsonString = "{\"Want\": { " +
                            "\"" + Guid.NewGuid().ToString() + "\": 3," +
                            "\"" + Guid.NewGuid().ToString() + "\": 5," +
                            "\"" + Guid.NewGuid().ToString() + "\": 10" +
                        "}," +
                        "\"EndPoint\": \"https://example.com/gossip/asff3\"}";

            //Write.WriteLine(jsonString);
            WantMessage wm = JsonConvert.DeserializeObject<WantMessage>(jsonString);

            //Write.WriteLine(wm.ToJson());

            //Write.WriteLine("\n\n");
            //Write.WriteLine(JsonConvert.SerializeObject(wm));



        }
    }
}