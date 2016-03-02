using Nancy.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using GossipProtocol.UserManagement;

namespace GossipProtocol.Gossip
{
    public class GossipLoop
    {
        private static float secondPerIteration;
        private static Timer aTimer;
        private static bool initialized = false;

        public static void init()
        {
            // Instantiate the timer
            secondPerIteration = 1;
            aTimer = new Timer((int)secondPerIteration * 1000);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // mark as initialized
            initialized = true;
        }

        public static void poke(string username)
        {
            if (!initialized || aTimer == null || aTimer.Enabled)
                return;

            User user = UserManager.get().getUser(username);
            if (user == null)
                return;

            user.resetRemainingCycles();
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // stuff for debugging
            string message = "The Elapsed event was raised at " + e.SignalTime;
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);

            List<User> users = UserManager.get().getUsers();
            users = (
                from u in users
                where u.remainingCycles > 0
                select u).ToList();

            if (users.Count == 0)
                aTimer.Enabled = false;

            // send a want or rumor using the following algorithm
            /*
            while true {
              q = getPeer(state)                    
              s = prepareMsg(state, q)       
              <url> = lookup(q)
              send (<url>, s)                 
              sleep n
            }
            */

        }

    }
}