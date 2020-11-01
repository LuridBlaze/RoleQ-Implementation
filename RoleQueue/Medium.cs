using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RoleQueue
{
    //This is the place where we do most of the heavy work, assigning roles, generating players, shutting down and booting up sessions.
    public class Medium
    {

        public Random random = new Random();
        public List<Player> allPllayers = new List<Player>();
        public Medium()
        {
        }

        //Copied off of StackOverflow, I think it was the random Enum picker something something
        public Role randomRole()
        {
            int pick = random.Next(Enum.GetValues(typeof(Role)).Length);
            return (Role)Enum.GetValues(typeof(Role)).GetValue(pick);
        }

        public Player CreatePlayer()
        {
            Player addedEntity = new Player(random.Next(), randomRole());

            return addedEntity;
        }

        //Basically we get 6 players off the queue(that is if there are enough)
        //I will probably implement the check after we add any players, so that we literally can't create the thing if
        //there just aren't enough players in the queue for all roles to be filled for either (or neither) team.
        //Update: I did actually do it, gosh I know I'm so good at programming.
        public Session AssembleSession(List<Player> RoleQ)
        {
            //In theory neither of those exceptions should ever be able to even come up, like ever.
            //(Since we execute all the checks for the available session creation in a ton of other steps)
            //But just in case I left them here, idk. Probably violated the DRY rule, might comment them out later when I'm sure the thing actually works.

            //if (RoleQ.Count() < 6)
            //    throw new Exception("Too few players are in queue right now to start a Session!");
            if (!sessionAvailable(RoleQ))
                throw new Exception("Not enough players to fill every role for two teams");
            List<Player> T1 = new List<Player>()
            {
                RoleQ.FirstOrDefault(player=>player.Role == Role.Tank),
                RoleQ.FirstOrDefault(player=>player.Role == Role.DamageDealer),
                RoleQ.FirstOrDefault(player=>player.Role == Role.Support)
            };

            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.Tank));
            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.DamageDealer));
            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.Support));

            List<Player> T2 = new List<Player>()
            {
                RoleQ.FirstOrDefault(player=>player.Role == Role.Tank),
                RoleQ.FirstOrDefault(player=>player.Role == Role.DamageDealer),
                RoleQ.FirstOrDefault(player=>player.Role == Role.Support)
            };

            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.Tank));
            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.DamageDealer));
            RoleQ.Remove(RoleQ.FirstOrDefault(player => player.Role == Role.Support));
            if (T1.Contains(default(Player)))
                throw new Exception("Not enough players to fill up a session");
            Session addedSession = new Session(T1, T2) { StartTime = DateTime.Now };
            return addedSession;
        }

        //Returning the players back into the queue, also the session can be manually closed -> means it was cancelled and the players get priority queue util their nexzt game starts.
        public void ShutdownSession(Session sessionToShutdown, List<Player> qToFill)
        {
            if (sessionToShutdown.Loser != null)
            {
                foreach (var loser in sessionToShutdown.Loser)
                {
                    loser.PriorityQueue = false;
                    loser.AddLoss();
                    qToFill.Add(loser);
                }

                foreach (var winner in sessionToShutdown.Winner)
                {
                    winner.PriorityQueue = false;
                    winner.AddWin();
                    qToFill.Add(winner);
                }
            } 
            else
            {
                sessionToShutdown.Cancelled = true;
                foreach (var person in sessionToShutdown.Team1)
                {   
                    person.PriorityQueue = true;
                    qToFill.Insert(0, person);
                   
                }

                foreach (var person in sessionToShutdown.Team2)
                {
                    person.PriorityQueue = true;
                    qToFill.Insert(0, person);

                }
            }
            sessionToShutdown.EndTime = DateTime.Now;
            sessionToShutdown.Duration = sessionToShutdown.EndTime - sessionToShutdown.StartTime;
        }


        public bool sessionAvailable(List<Player> RoleQ)
        {
            return (RoleQ.Count() > 6 &
                RoleQ.Where(p => p.Role == Role.Tank).Count() > 1 &
                RoleQ.Where(p => p.Role == Role.DamageDealer).Count() > 1 &
                RoleQ.Where(p => p.Role == Role.Support).Count() > 1);
        }
    }
}
