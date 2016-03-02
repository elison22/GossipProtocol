using Nancy;
using Nancy.Security;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

                return null;
            };

            Post["/peer/delete"] = _ =>
            {
                User user = getCurUser();
                user.neighbors.Remove("FILL IN THE WAY TO REMOVE NEIGHBORS");

                return View["chat"/*chat data*/];
            };



        }

        private long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

    }
}