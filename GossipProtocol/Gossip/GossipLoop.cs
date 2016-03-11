using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using GossipProtocol.UserManagement;
using GossipProtocol.Writing;
using GossipProtocol.Gossip.Message;

namespace GossipProtocol.Gossip
{
    public class GossipLoop
    {
        private static float SecondsPerIteration;
        private static Timer LoopTimer;
        private static bool Initialized = false;
        private static Random Rand;

        public static void Init()
        {
            // Instantiate the timer
            SecondsPerIteration = 1;
            Rand = new Random();
            LoopTimer = new Timer((int)SecondsPerIteration * 1000);

            // Hook up the Elapsed event for the timer. 
            LoopTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            LoopTimer.AutoReset = true;

            // mark as initialized
            Initialized = true;
        }

        public static void Poke(string Username)
        {
            if (!Initialized || LoopTimer == null || LoopTimer.Enabled)
                return;

            User user = UserManager.get().getUser(Username);
            if (user == null)
                return;

            user.ResetRemainingCycles();
            LoopTimer.Enabled = true;
        }


        private static void OnTimedEvent(object Source, ElapsedEventArgs E)
        {
            // stuff for debugging
            string message = "The Elapsed event was raised at " + E.SignalTime;
            Write.WriteLine(message);

            List<User> users = (
                from u in UserManager.get().getUsers()
                where u.RemainingCycles > 0
                select u).ToList();

            if (users.Count == 0)
            {
                Write.WriteLine("No active users");
                LoopTimer.Enabled = false;
            }

            foreach(User u in users)
            {
                Write.WriteLine("User " + u.FirstName + " has " + u.RemainingCycles + " remaining cycles");
                if (u.Neighbors == null || u.Neighbors.Count == 0)
                    continue;
                Peer randNeigbor = u.Neighbors[Rand.Next(u.Neighbors.Count)];

                int messageType = Rand.Next(2);
                
                if(messageType == 0)
                {
                    SentRumor(randNeigbor, u);
                }
                else
                {
                    SendWant(randNeigbor, u);
                }
                
                u.DecCycles();
            }
        }

        private static void SendWant(Peer Neighbor, User Me)
        {
            WantMessage want = Me.MessageState.GetWant();
            want.EndPoint = Me.Endpoint;
            Sender.SendMessage(Neighbor.Endpoint, want);
        }

        private static void SentRumor(Peer Neighbor, User Me)
        {
            RumorMessage rumor = Me.MessageState.GetRandMessage(Neighbor);
            if (rumor == null)
            {
                SendWant(Neighbor, Me);
            }

            rumor.EndPoint = Me.Endpoint;
            bool successful = Sender.SendMessage(Neighbor.Endpoint, rumor);
            if (successful)
            {
                Me.MessageState.SentMessages[Neighbor].Add(rumor.Rumor.FullId);
            }
        }

    }
}