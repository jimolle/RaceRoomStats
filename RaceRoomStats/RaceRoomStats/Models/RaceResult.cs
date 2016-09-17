using System;
using System.Collections.Generic;

namespace RaceRoomStats.Models
{
    public class RaceResult
    {
        public string Server { get; set; }
        public DateTime Time { get; set; }
        public string Experience { get; set; }
        public string Difficulty { get; set; }
        public string FuelUsage { get; set; }
        public string TireWear { get; set; }
        public string MechanicalDamage { get; set; }
        public string FlagRules { get; set; }
        public string CutRules { get; set; }
        public string RaceSeriesFormat { get; set; }
        public string WreckerPrevention { get; set; }
        public string MandatoryPitstop { get; set; }
        public string Track { get; set; }
        public List<Session> Sessions { get; set; }
    }
}