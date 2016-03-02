using Nancy;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class HomeModule : MyNancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => Response.AsRedirect("/home");
            Get["/home"] = _ =>
            {
                Console.WriteLine("\nWelcome to the home page!");

                if (loggedIn())
                    return Response.AsRedirect("/chat");

                return View["home"];
            };
        }

        
        
    }
}