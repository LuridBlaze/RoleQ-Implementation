using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RoleQueue
{
    //[JsonObject]
    public class Session
    {
        [JsonIgnore]
        private Random random = new Random();
        [JsonProperty]
        public List<Player> Team1 { get; private set; }
        [JsonProperty]
        public List<Player> Team2 { get; private set; }
        [JsonProperty]
        //Winner and Loser is basically suppoosed to be a reference to either of the teams,
        //hopefully I can come up with a solution that doesn't waste to much space on these two variables
        public List<Player> Winner { get; private set; }
        [JsonProperty]
        public List<Player> Loser { get; private set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        //Unless the match(session) ends properly, with one team as a winner,
        //the session will be considered cancelled
        public bool Cancelled = true;

        public Session(List<Player> team1, List<Player> team2)
        {
            Team1 = team1;
            Team2 = team2;
            //StartTime = DateTime.Now;
            //Duration = new TimeSpan(0, 0, random.Next(7,20));
            //EndTime = StartTime.Add(Duration);
        }

        public void SessionResults(bool t1Won)
        {
            if (t1Won)
            {
                Winner = Team1;
                Loser = Team2;
            }
            else
            {
                Winner = Team2;
                Loser = Team1;
            }
        }
    }
}
