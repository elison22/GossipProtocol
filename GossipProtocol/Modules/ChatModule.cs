using GossipProtocol.Gossip.Message;
using GossipProtocol.UserManagement;
using Nancy;
using Nancy.ModelBinding;
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
                if (user.Neighbors.Count == 0)
                    return Response.AsRedirect("/peer/add");

                // if so, get the messages for that user and display them
                List<RumorMessage> messages = user.Messages;
                // TODO: do this once all of the model code is implemented
                
                return View["chat", makeView(messages)];
            };

            Post["/chat/new"] = _ =>
            {
                // get the current user
                User user = getCurUser();

                // Check if the user has a peer yet
                // if not, go to the add a peer page
                if (user.Neighbors.Count == 0)
                    return Response.AsRedirect("/peer/add");

                // add this message to the current list of messages
                // bind the form elements
                MessageParams messageParams = this.Bind<MessageParams>();


                // TODO: implement this once the model code is completed

                return Response.AsRedirect("/chat");
            };

        }

        private class MessageParams
        {
            public string Text { get; set; }
        }
    }
}