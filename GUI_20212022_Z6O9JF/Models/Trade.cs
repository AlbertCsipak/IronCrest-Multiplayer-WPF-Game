using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Offer
    {
        public string Text { get; set; }
        public Dictionary<string, int> Cost { get; set; }
        public Dictionary<string, int> Gain { get; set; }

        public Offer(string text, Dictionary<string, int> Cost, Dictionary<string, int> Gain)
        {
            Text = text;
            this.Cost = Cost;
            this.Gain = Gain;
        }
    }
    public class Trade
    {
        public Offer[] Offers { get; set; }
        public int[] SelectedOfferIndex { get; set; }
        public Trade(Offer[] Offers)
        {
            this.Offers = Offers;
            SelectedOfferIndex = new int[2];
        }
        
    }
}
