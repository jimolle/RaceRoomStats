using System.Collections.Generic;

namespace RaceRoomStats.Models
{
    public class Session
    {
        public string Type { get; set; }
        public List<Player> Players { get; set; }
    }
}