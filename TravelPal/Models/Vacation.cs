using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models
{
    public class Vacation : Travel
    {

        public bool AllInclusive { get; set; }
        public Vacation(string destination, Countries country, int travellers, DateTime startDate, DateTime endingDate, bool allInclusive, User owner) : base(destination, country, travellers, startDate, endingDate, owner)
        {
            AllInclusive = allInclusive;
        }

        // Returns interpolated string containing information about the travel (Destination country and travel duration)
        public override string GetInfo()
        {
            return base.GetInfo();
        }
    }
}
