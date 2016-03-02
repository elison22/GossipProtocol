using GossipProtocol.UserManagement;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class ChatModule : MyNancyModule
    {
        public ChatModule()
        {
            this.RequiresAuthentication();

            Get["/chat"] = _ =>
            {
                // get the current user
                User user = getCurUser();

                // Check if the user has a peer yet
                // if not, go to the add a peer page
                if (user.neighbors.Count == 0)
                    return Response.AsRedirect("/peer/add");

                // if so, get the messages for that user and display them
                // TODO: do this once all of the model code is implemented


                return View["chat"/*MODEL DATA (LOTS OF MODEL DATA)*/];
            };

            Post["/chat/new"] = _ =>
            {
                // get the current user
                User user = getCurUser();

                // make a new peer object
                // add it to the list of peers

                // TODO: implement this once the model code is completed

                return View["chat"/*MODEL DATA (LOTS OF MODEL DATA)*/];
            };



        }

    }
}