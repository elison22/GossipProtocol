using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class GossipModule : MyNancyModule
    {

        public static string baseURL = "";

        public GossipModule()
        {
            Post["/gossip/{id}"] = _ =>
            {
                // this is the endpoint that external nodes will use
                // it must implement the following algorithm:
                /*
                    t = getMessage();
                    if (  isRumor(t)  ) {
                         store(t)
                    } elsif ( isWant(t) ) {
                        work_queue = addWorkToQueue(t)
                        foreach w work_queue {
                          s = prepareMsg(state, w)
                          <url> = getUrl(w)
                          send(<url>, s)
                          state = update(state, s)
                        }
                    }
                */
                return null;
            };

        }

    }
}