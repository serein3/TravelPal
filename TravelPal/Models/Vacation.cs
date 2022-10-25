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
        public Vacation(string destination, Countries country, int travellers, List<IPackingListItem> packingList, DateTime startDate, DateTime endingDate, int travelDays, bool allInclusive) : base(destination, country, travellers, packingList, startDate, endingDate, travelDays)
        {
            AllInclusive = allInclusive;
        }

        public override string GetInfo()
        {
            return base.GetInfo();
        }
    }
}
