using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GossipProtocol.Network;

namespace GossipProtocol
{
    public class Bookstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, UserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>(),
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            if (context.CurrentUser != null)
            {
                GossipLoop.poke(context.CurrentUser.UserName);
            }


        }

        //protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        //{
        //    base.ApplicationStartup(container, pipelines);

        //    System.Diagnostics.Trace.WriteLine("MADE IT TO THE STARTUP METHOD");
        //}
    }
}