using System.Collections.Generic;

namespace RaceRoomStats.Models
{
    public class Player
    {
        public string FullName { get; set; }
        public string Car { get; set; }
        public int Position { get; set; }
        public int BestLapTime { get; set; }
        public int TotalTime { get; set; }
        public string FinishStatus { get; set; }
        public List<RaceSessionLap> RaceSessionLaps { get; set; }
    }
}