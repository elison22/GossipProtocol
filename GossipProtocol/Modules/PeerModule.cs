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

            Get["/peer/add"] = _ => View["peer_add", makeView(getCurUser())];
            Post["/peer/add"] = _ =>
            {
                User user = getCurUser();

                PeerParams peerParams = this.Bind<PeerParams>();
                
                user.AddPeer(peerParams.Endpoint);

                return Response.AsRedirect("/chat");
            };

            // ========= STILL =========
            // TODO: Finish this one!!!!
            Post["/peer/delete"] = _ =>
            {
                User user = getCurUser();

                PeerParams peerParams = this.Bind<PeerParams>();
                Peer toRemove = new Peer { Endpoint = peerParams.Endpoint };

                if (!user.Neighbors.Contains(toRemove))
                {
                    return View["error", makeError("You do not have a peer with the following endpoint:\n" + peerParams.Endpoint, "chat", "/chat")];
                }

                user.RemovePeer(toRemove);

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