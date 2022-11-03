using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Models
{
    public class OtherItem : IPackingListItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public OtherItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }


        // Returns interpolated string containing information about the item (Quantity and name of item)
        public string GetInfo()
        {
            return $"{Quantity}x {Name}";
        }
    }
}
