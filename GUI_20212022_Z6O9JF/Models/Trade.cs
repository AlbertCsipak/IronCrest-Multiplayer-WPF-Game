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
        public List<int> SelectedOfferIndexes { get; set; }
        public int[] Position { get; set; }
        public int OwnerId { get; set; }
        public Trade(Offer[] Offers)
        {
            this.Offers = Offers;
            SelectedOfferIndexes = new List<int>();
            Position = new int[2];
        }

        public void Move(int[] pos)
        {
            throw new NotImplementedException();
        }
    }
}
