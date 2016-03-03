using Nancy;
using Nancy.Security;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.ModelBinding;
using GossipProtocol.Gossip;

namespace GossipProtocol.Modules
{
    public class PeerModule : MyNancyModule
    {
        public PeerModule()
        {
            this.RequiresAuthentication();

            Get["/peer/add"] = _ => View["peer_add"];
            Post["/peer/add"] = _ =>
            {
                User user = getCurUser();
                /*
                bind to the form fields
                create a peer
                add it to the current user
                */
                PeerParams peerParams = this.Bind<PeerParams>();

                Peer peer = new Peer
                {
                    Endpoint = peerParams.Endpoint
                };

                user.Neighbors.Add(peer);

                return Response.AsRedirect("/chat");
            };

            // TODO: Finish this one!!!!
            Post["/peer/delete"] = _ =>
            {
                User user = getCurUser();
                //user.neighbors.Remove("FILL IN THE WAY TO REMOVE NEIGHBORS");

                return Response.AsRedirect("/chat");
            };
            
        }

        private long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }
        
        private class PeerParams
        {
            public string Endpoint { get; set; }
        }
    }
}